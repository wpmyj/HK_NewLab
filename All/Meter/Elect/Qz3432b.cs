using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    public class Qz3432b:Elect.Elect
    {
        Dictionary<string, string> initParm;

        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        byte address = 0;
        /// <summary>
        /// 模块通讯地址
        /// </summary>
        public byte Address
        {
            get { return address; }
            set { address = value; }
        }
        public override void Init(Dictionary<string, string> initParm)
        {
            InitParm = initParm;
            if (InitParm.ContainsKey("Text"))
            {
                this.Text = InitParm["Text"];
            }
            if (initParm.ContainsKey("TimeOut"))
            {
                this.TimeOut = All.Class.Num.ToInt(initParm["TimeOut"]);
            }
            if (initParm.ContainsKey("ErrorCount"))
            {
                this.ErrorCount = All.Class.Num.ToInt(initParm["ErrorCount"]);
            }
            if (!InitParm.ContainsKey("Address"))
            {
                All.Class.Error.Add("参数中没有地址", Environment.StackTrace);
            }
            else
            {
                address = All.Class.Num.ToByte(InitParm["Address"]);
            }

        }
        /// <summary>
        /// 按指令读取指定数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">读取返回数据</param>
        /// <param name="parm">读取参数,必须包含Code,Address,Len三个参数</param>
        /// <returns></returns>
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                value = new List<T>();
                try
                {
                    int Phase = 0;//单相
                    if (parm.ContainsKey("Code"))
                    {
                        switch (parm["Code"].ToUpper())
                        {
                            case "SINGLEPHASE":
                            case "1":
                            case "单相":
                            case "单":
                                Phase = 1;
                                break;
                            case "THREEPHASE":
                            case "3":
                            case "三相":
                            case "三":
                                Phase = 3;
                                break;
                        }
                    }
                    byte[] sendBuff = new byte[8];
                    sendBuff[0] = address;
                    sendBuff[1] = 0x03;
                    sendBuff[2] = 0x01;
                    sendBuff[3] = 0x00;
                    sendBuff[4] = 0x00;
                    sendBuff[5] = 0x78;
                    All.Class.Check.Crc16(sendBuff, 6, out sendBuff[6], out sendBuff[7]);
                    byte[] readBuff;
                    if (WriteAndRead<byte[], byte[]>(sendBuff, 245, out readBuff))
                    {
                        switch (All.Class.TypeUse.GetType<T>())
                        {
                            case Class.TypeUse.TypeList.Float:
                                switch (Phase)
                                {
                                    case 0://全体数据
                                        //相电压
                                        value.Add((T)(object)(float)(((readBuff[92 + 3] << 24) + (readBuff[93 + 3] << 16) + (readBuff[94 + 3] << 8) + (readBuff[95 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[96 + 3] << 24) + (readBuff[97 + 3] << 16) + (readBuff[98 + 3] << 8) + (readBuff[99 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[100 + 3] << 24) + (readBuff[101 + 3] << 16) + (readBuff[102 + 3] << 8) + (readBuff[103 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[108 + 3] << 24) + (readBuff[109 + 3] << 16) + (readBuff[110 + 3] << 8) + (readBuff[111 + 3] << 0)) * 0.1f));
                                        //线电压
                                        value.Add((T)(object)(float)(((readBuff[76 + 3] << 24) + (readBuff[77 + 3] << 16) + (readBuff[78 + 3] << 8) + (readBuff[79 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[80 + 3] << 24) + (readBuff[81 + 3] << 16) + (readBuff[82 + 3] << 8) + (readBuff[83 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[84 + 3] << 24) + (readBuff[85 + 3] << 16) + (readBuff[86 + 3] << 8) + (readBuff[87 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[88 + 3] << 24) + (readBuff[89 + 3] << 16) + (readBuff[90 + 3] << 8) + (readBuff[91 + 3] << 0)) * 0.1f));
                                        //电流
                                        value.Add((T)(object)(float)(((readBuff[0 + 3] << 24) + (readBuff[1 + 3] << 16) + (readBuff[2 + 3] << 8) + (readBuff[3 + 3] << 0)) * 0.001f));
                                        value.Add((T)(object)(float)(((readBuff[4 + 3] << 24) + (readBuff[5 + 3] << 16) + (readBuff[6 + 3] << 8) + (readBuff[7 + 3] << 0)) * 0.001f));
                                        value.Add((T)(object)(float)(((readBuff[8 + 3] << 24) + (readBuff[9 + 3] << 16) + (readBuff[10 + 3] << 8) + (readBuff[11 + 3] << 0)) * 0.001f));
                                        value.Add((T)(object)(float)(((readBuff[16 + 3] << 24) + (readBuff[17 + 3] << 16) + (readBuff[18 + 3] << 8) + (readBuff[19 + 3] << 0)) * 0.001f));
                                        //有功功率
                                        value.Add((T)(object)(float)(((readBuff[160 + 3] << 24) + (readBuff[161 + 3] << 16) + (readBuff[162 + 3] << 8) + (readBuff[163 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[164 + 3] << 24) + (readBuff[165 + 3] << 16) + (readBuff[166 + 3] << 8) + (readBuff[167 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[168 + 3] << 24) + (readBuff[169 + 3] << 16) + (readBuff[170 + 3] << 8) + (readBuff[171 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[172 + 3] << 24) + (readBuff[173 + 3] << 16) + (readBuff[174 + 3] << 8) + (readBuff[175 + 3] << 0)) * 0.1f));
                                        //无功功率
                                        value.Add((T)(object)(float)(((readBuff[176 + 3] << 24) + (readBuff[177 + 3] << 16) + (readBuff[178 + 3] << 8) + (readBuff[179 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[180 + 3] << 24) + (readBuff[181 + 3] << 16) + (readBuff[182 + 3] << 8) + (readBuff[183 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[184 + 3] << 24) + (readBuff[185 + 3] << 16) + (readBuff[186 + 3] << 8) + (readBuff[187 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[188 + 3] << 24) + (readBuff[189 + 3] << 16) + (readBuff[190 + 3] << 8) + (readBuff[191 + 3] << 0)) * 0.1f));
                                        //视在功率
                                        value.Add((T)(object)(float)(((readBuff[192 + 3] << 24) + (readBuff[193 + 3] << 16) + (readBuff[194 + 3] << 8) + (readBuff[195 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[196 + 3] << 24) + (readBuff[197 + 3] << 16) + (readBuff[198 + 3] << 8) + (readBuff[199 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[200 + 3] << 24) + (readBuff[201 + 3] << 16) + (readBuff[202 + 3] << 8) + (readBuff[203 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[204 + 3] << 24) + (readBuff[205 + 3] << 16) + (readBuff[206 + 3] << 8) + (readBuff[207 + 3] << 0)) * 0.1f));
                                        //功率因素,因为暂时没有用到,所以暂时不读取,以后补齐
                                        value.Add((T)(object)0f);
                                        value.Add((T)(object)0f);
                                        value.Add((T)(object)0f);
                                        value.Add((T)(object)0f);
                                        //频率
                                        value.Add((T)(object)(float)(((readBuff[104 + 3] << 24) + (readBuff[105 + 3] << 16) + (readBuff[106 + 3] << 8) + (readBuff[107 + 3] << 0)) * 0.01f));
                                        break;
                                    case 3://只取三相数据
                                        //相电压
                                        value.Add((T)(object)(float)(((readBuff[92 + 3] << 24) + (readBuff[93 + 3] << 16) + (readBuff[94 + 3] << 8) + (readBuff[95 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[96 + 3] << 24) + (readBuff[97 + 3] << 16) + (readBuff[98 + 3] << 8) + (readBuff[99 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[100 + 3] << 24) + (readBuff[101 + 3] << 16) + (readBuff[102 + 3] << 8) + (readBuff[103 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[108 + 3] << 24) + (readBuff[109 + 3] << 16) + (readBuff[110 + 3] << 8) + (readBuff[111 + 3] << 0)) * 0.1f));
                                        //电流
                                        value.Add((T)(object)(float)(((readBuff[0 + 3] << 24) + (readBuff[1 + 3] << 16) + (readBuff[2 + 3] << 8) + (readBuff[3 + 3] << 0)) * 0.001f));
                                        value.Add((T)(object)(float)(((readBuff[4 + 3] << 24) + (readBuff[5 + 3] << 16) + (readBuff[6 + 3] << 8) + (readBuff[7 + 3] << 0)) * 0.001f));
                                        value.Add((T)(object)(float)(((readBuff[8 + 3] << 24) + (readBuff[9 + 3] << 16) + (readBuff[10 + 3] << 8) + (readBuff[11 + 3] << 0)) * 0.001f));
                                        value.Add((T)(object)(float)(((readBuff[16 + 3] << 24) + (readBuff[17 + 3] << 16) + (readBuff[18 + 3] << 8) + (readBuff[19 + 3] << 0)) * 0.001f));
                                        //有功功率
                                        value.Add((T)(object)(float)(((readBuff[160 + 3] << 24) + (readBuff[161 + 3] << 16) + (readBuff[162 + 3] << 8) + (readBuff[163 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[164 + 3] << 24) + (readBuff[165 + 3] << 16) + (readBuff[166 + 3] << 8) + (readBuff[167 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[168 + 3] << 24) + (readBuff[169 + 3] << 16) + (readBuff[170 + 3] << 8) + (readBuff[171 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(float)(((readBuff[172 + 3] << 24) + (readBuff[173 + 3] << 16) + (readBuff[174 + 3] << 8) + (readBuff[175 + 3] << 0)) * 0.1f));
                                        //功率因素,因为暂时没有用到,所以暂时不读取,以后补齐
                                        value.Add((T)(object)0f);
                                        value.Add((T)(object)0f);
                                        value.Add((T)(object)0f);
                                        //频率
                                        value.Add((T)(object)(float)(((readBuff[104 + 3] << 24) + (readBuff[105 + 3] << 16) + (readBuff[106 + 3] << 8) + (readBuff[107 + 3] << 0)) * 0.01f));
                                        break;
                                    case 1://只取单相数据
                                        //相电压
                                        value.Add((T)(object)(float)(((readBuff[92 + 3] << 24) + (readBuff[93 + 3] << 16) + (readBuff[94 + 3] << 8) + (readBuff[95 + 3] << 0)) * 0.1f));
                                        //电流
                                        value.Add((T)(object)(float)(((readBuff[0 + 3] << 24) + (readBuff[1 + 3] << 16) + (readBuff[2 + 3] << 8) + (readBuff[3 + 3] << 0)) * 0.001f));
                                        //有功功率
                                        value.Add((T)(object)(float)(((readBuff[160 + 3] << 24) + (readBuff[161 + 3] << 16) + (readBuff[162 + 3] << 8) + (readBuff[163 + 3] << 0)) * 0.1f));
                                        //功率因素,因为暂时没有用到,所以暂时不读取,以后补齐
                                        value.Add((T)(object)0f);
                                        //频率
                                        value.Add((T)(object)(float)(((readBuff[104 + 3] << 24) + (readBuff[105 + 3] << 16) + (readBuff[106 + 3] << 8) + (readBuff[107 + 3] << 0)) * 0.01f));
                                        break;
                                }
                                break;
                            case Class.TypeUse.TypeList.Double:
                                switch (Phase)
                                {
                                    case 0:
                                        //相电压
                                        value.Add((T)(object)(double)(((readBuff[92 + 3] << 24) + (readBuff[93 + 3] << 16) + (readBuff[94 + 3] << 8) + (readBuff[95 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[96 + 3] << 24) + (readBuff[97 + 3] << 16) + (readBuff[98 + 3] << 8) + (readBuff[99 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[100 + 3] << 24) + (readBuff[101 + 3] << 16) + (readBuff[102 + 3] << 8) + (readBuff[103 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[108 + 3] << 24) + (readBuff[109 + 3] << 16) + (readBuff[110 + 3] << 8) + (readBuff[111 + 3] << 0)) * 0.1f));
                                        //线电压
                                        value.Add((T)(object)(double)(((readBuff[76 + 3] << 24) + (readBuff[77 + 3] << 16) + (readBuff[78 + 3] << 8) + (readBuff[79 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[80 + 3] << 24) + (readBuff[81 + 3] << 16) + (readBuff[82 + 3] << 8) + (readBuff[83 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[84 + 3] << 24) + (readBuff[85 + 3] << 16) + (readBuff[86 + 3] << 8) + (readBuff[87 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[88 + 3] << 24) + (readBuff[89 + 3] << 16) + (readBuff[90 + 3] << 8) + (readBuff[91 + 3] << 0)) * 0.1f));
                                        //电流
                                        value.Add((T)(object)(double)(((readBuff[0 + 3] << 24) + (readBuff[1 + 3] << 16) + (readBuff[2 + 3] << 8) + (readBuff[3 + 3] << 0)) * 0.001f));
                                        value.Add((T)(object)(double)(((readBuff[4 + 3] << 24) + (readBuff[5 + 3] << 16) + (readBuff[6 + 3] << 8) + (readBuff[7 + 3] << 0)) * 0.001f));
                                        value.Add((T)(object)(double)(((readBuff[8 + 3] << 24) + (readBuff[9 + 3] << 16) + (readBuff[10 + 3] << 8) + (readBuff[11 + 3] << 0)) * 0.001f));
                                        value.Add((T)(object)(double)(((readBuff[16 + 3] << 24) + (readBuff[17 + 3] << 16) + (readBuff[18 + 3] << 8) + (readBuff[19 + 3] << 0)) * 0.001f));
                                        //有功功率
                                        value.Add((T)(object)(double)(((readBuff[160 + 3] << 24) + (readBuff[161 + 3] << 16) + (readBuff[162 + 3] << 8) + (readBuff[163 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[164 + 3] << 24) + (readBuff[165 + 3] << 16) + (readBuff[166 + 3] << 8) + (readBuff[167 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[168 + 3] << 24) + (readBuff[169 + 3] << 16) + (readBuff[170 + 3] << 8) + (readBuff[171 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[172 + 3] << 24) + (readBuff[173 + 3] << 16) + (readBuff[174 + 3] << 8) + (readBuff[175 + 3] << 0)) * 0.1f));
                                        //无功功率
                                        value.Add((T)(object)(double)(((readBuff[176 + 3] << 24) + (readBuff[177 + 3] << 16) + (readBuff[178 + 3] << 8) + (readBuff[179 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[180 + 3] << 24) + (readBuff[181 + 3] << 16) + (readBuff[182 + 3] << 8) + (readBuff[183 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[184 + 3] << 24) + (readBuff[185 + 3] << 16) + (readBuff[186 + 3] << 8) + (readBuff[187 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[188 + 3] << 24) + (readBuff[189 + 3] << 16) + (readBuff[190 + 3] << 8) + (readBuff[191 + 3] << 0)) * 0.1f));
                                        //视在功率
                                        value.Add((T)(object)(double)(((readBuff[192 + 3] << 24) + (readBuff[193 + 3] << 16) + (readBuff[194 + 3] << 8) + (readBuff[195 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[196 + 3] << 24) + (readBuff[197 + 3] << 16) + (readBuff[198 + 3] << 8) + (readBuff[199 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[200 + 3] << 24) + (readBuff[201 + 3] << 16) + (readBuff[202 + 3] << 8) + (readBuff[203 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[204 + 3] << 24) + (readBuff[205 + 3] << 16) + (readBuff[206 + 3] << 8) + (readBuff[207 + 3] << 0)) * 0.1f));
                                        //功率因素,因为暂时没有用到,所以暂时不读取,以后补齐
                                        value.Add((T)(object)(double)0);
                                        value.Add((T)(object)(double)0);
                                        value.Add((T)(object)(double)0);
                                        value.Add((T)(object)(double)0);
                                        //频率
                                        value.Add((T)(object)(double)(((readBuff[104 + 3] << 24) + (readBuff[105 + 3] << 16) + (readBuff[106 + 3] << 8) + (readBuff[107 + 3] << 0)) * 0.01f));
                                        break;
                                    case 3:
                                        //相电压
                                        value.Add((T)(object)(double)(((readBuff[92 + 3] << 24) + (readBuff[93 + 3] << 16) + (readBuff[94 + 3] << 8) + (readBuff[95 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[96 + 3] << 24) + (readBuff[97 + 3] << 16) + (readBuff[98 + 3] << 8) + (readBuff[99 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[100 + 3] << 24) + (readBuff[101 + 3] << 16) + (readBuff[102 + 3] << 8) + (readBuff[103 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[108 + 3] << 24) + (readBuff[109 + 3] << 16) + (readBuff[110 + 3] << 8) + (readBuff[111 + 3] << 0)) * 0.1f));
                                        //电流
                                        value.Add((T)(object)(double)(((readBuff[0 + 3] << 24) + (readBuff[1 + 3] << 16) + (readBuff[2 + 3] << 8) + (readBuff[3 + 3] << 0)) * 0.001f));
                                        value.Add((T)(object)(double)(((readBuff[4 + 3] << 24) + (readBuff[5 + 3] << 16) + (readBuff[6 + 3] << 8) + (readBuff[7 + 3] << 0)) * 0.001f));
                                        value.Add((T)(object)(double)(((readBuff[8 + 3] << 24) + (readBuff[9 + 3] << 16) + (readBuff[10 + 3] << 8) + (readBuff[11 + 3] << 0)) * 0.001f));
                                        value.Add((T)(object)(double)(((readBuff[16 + 3] << 24) + (readBuff[17 + 3] << 16) + (readBuff[18 + 3] << 8) + (readBuff[19 + 3] << 0)) * 0.001f));
                                        //有功功率
                                        value.Add((T)(object)(double)(((readBuff[160 + 3] << 24) + (readBuff[161 + 3] << 16) + (readBuff[162 + 3] << 8) + (readBuff[163 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[164 + 3] << 24) + (readBuff[165 + 3] << 16) + (readBuff[166 + 3] << 8) + (readBuff[167 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[168 + 3] << 24) + (readBuff[169 + 3] << 16) + (readBuff[170 + 3] << 8) + (readBuff[171 + 3] << 0)) * 0.1f));
                                        value.Add((T)(object)(double)(((readBuff[172 + 3] << 24) + (readBuff[173 + 3] << 16) + (readBuff[174 + 3] << 8) + (readBuff[175 + 3] << 0)) * 0.1f));
                                        //功率因素,因为暂时没有用到,所以暂时不读取,以后补齐
                                        value.Add((T)(object)(double)0);
                                        value.Add((T)(object)(double)0);
                                        value.Add((T)(object)(double)0);
                                        //频率
                                        value.Add((T)(object)(double)(((readBuff[104 + 3] << 24) + (readBuff[105 + 3] << 16) + (readBuff[106 + 3] << 8) + (readBuff[107 + 3] << 0)) * 0.01f));
                                        break;
                                    case 1:
                                        //相电压
                                        value.Add((T)(object)(double)(((readBuff[92 + 3] << 24) + (readBuff[93 + 3] << 16) + (readBuff[94 + 3] << 8) + (readBuff[95 + 3] << 0)) * 0.1f));
                                        //电流
                                        value.Add((T)(object)(double)(((readBuff[0 + 3] << 24) + (readBuff[1 + 3] << 16) + (readBuff[2 + 3] << 8) + (readBuff[3 + 3] << 0)) * 0.001f));
                                        //有功功率
                                        value.Add((T)(object)(double)(((readBuff[160 + 3] << 24) + (readBuff[161 + 3] << 16) + (readBuff[162 + 3] << 8) + (readBuff[163 + 3] << 0)) * 0.1f));
                                        //功率因素,因为暂时没有用到,所以暂时不读取,以后补齐
                                        value.Add((T)(object)(double)0);
                                        //频率
                                        value.Add((T)(object)(double)(((readBuff[104 + 3] << 24) + (readBuff[105 + 3] << 16) + (readBuff[106 + 3] << 8) + (readBuff[107 + 3] << 0)) * 0.01f));
                                        break;
                                }
                                break;
                            default:
                                All.Class.Error.Add("读取参数中返回数据类型不正确", Environment.StackTrace);
                                return false;
                        }
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(e);
                    result = false;
                }
                return result;
            }
        }
        public override bool Test()
        {
            ushort value = 0;
            return Read<ushort>(out value, 0);
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                All.Class.Error.Add("青智3432B暂时没有写入参数方法", Environment.StackTrace);
                return false;
            }
        }
    }
}
