using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;
namespace All.Communicate
{
    /// <summary>
    /// 自动读取数据
    /// </summary>
    public class AutoReadAndWrite
    {
        /// <summary>
        /// 所有读取数据
        /// </summary>
        public Data.AllData Reads
        { get; set; }
        /// <summary>
        /// 所有通讯类
        /// </summary>
        public List<Data.CommunicateStyle> Communicates
        { get; set; }
        public AutoReadAndWrite()
        {
            this.Reads = new Data.AllData();
            this.Communicates = new List<Data.CommunicateStyle>();
        }
        /// <summary>
        /// 加载默认位置文件,从中反射类
        /// </summary>
        public void Load()
        {
            this.Load("\\Data\\Meter.xml");
        }
        /// <summary>
        /// 加载所有读取类
        /// </summary>
        /// <returns></returns>
        public void Load(string fileName)
        {
            LoadFromXml(fileName);
            InitData();
            StartRead();
        }
        /// <summary>
        /// 开始自动读取数据
        /// </summary>
        private void StartRead()
        {
            if (Communicates == null || Communicates.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < Communicates.Count; i++)
            {
                int index = i;
                new Thread(() => Loop(index))
                {
                    IsBackground = true
                }.Start();
            }
        }
        /// <summary>
        /// 循环读取数据
        /// </summary>
        /// <param name="index"></param>
        private void Loop(int index)
        {
            if (Communicates[index].Meters == null || Communicates[index].Meters.Count <= 0)
            {
                return;
            }
            int start = Environment.TickCount;
            while (true)
            {
                Thread.Sleep(20);
                if ((Environment.TickCount - start) < Communicates[index].Value.FlushTick)
                {
                    continue;
                }
                start = Environment.TickCount;
                for (int i = 0; i < Communicates[index].Meters.Count; i++)
                {
                    if (Communicates[index].Meters[i].Reads == null | Communicates[index].Meters[i].Reads.Count <= 0)
                    {
                        continue;
                    }
                    for (int j = 0; j < Communicates[index].Meters[i].Reads.Count; j++)
                    {
                        //tmpType = All.Class.TypeUse.GetType(Communicates[index].Meters[i].Reads[j]["Data"]);
                        //tmpIndex = Data.XmlIndex.GetIndexFromSet(Communicates[index].Meters[i].Reads[j]["Index"], tmpType);
                        //switch (tmpType)
                        //{
                        //    case Class.TypeUse.TypeList.Boolean:
                        //        if (Communicates[index].Meters[i].Value.Read<bool>(out tmpBool, Communicates[index].Meters[i].Reads[j]))
                        //        {
                        //            for (int k = 0; k < tmpBool.Count && k < tmpIndex.Count; k++)
                        //            {
                        //                Reads.BoolValue.Value[tmpIndex[k]] = tmpBool[k];
                        //            }
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.Byte:
                        //        if (Communicates[index].Meters[i].Value.Read<byte>(out tmpByte, Communicates[index].Meters[i].Reads[j]))
                        //        {
                        //            for (int k = 0; k < tmpByte.Count && k < tmpIndex.Count; k++)
                        //            {
                        //                Reads.ByteValue.Value[tmpIndex[k]] = tmpByte[k];
                        //            }
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.DateTime:
                        //        if (Communicates[index].Meters[i].Value.Read<DateTime>(out tmpDateTime, Communicates[index].Meters[i].Reads[j]))
                        //        {
                        //            for (int k = 0; k < tmpDateTime.Count && k < tmpIndex.Count; k++)
                        //            {
                        //                Reads.DateTimeValue.Value[tmpIndex[k]] = tmpDateTime[k];
                        //            }
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.Double:
                        //        if (Communicates[index].Meters[i].Value.Read<double>(out tmpDouble, Communicates[index].Meters[i].Reads[j]))
                        //        {
                        //            for (int k = 0; k < tmpDouble.Count && k < tmpIndex.Count; k++)
                        //            {
                        //                Reads.DoubleValue.Value[tmpIndex[k]] = tmpDouble[k];
                        //            }
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.Float:
                        //        if (Communicates[index].Meters[i].Value.Read<float>(out tmpFloat, Communicates[index].Meters[i].Reads[j]))
                        //        {
                        //            for (int k = 0; k < tmpFloat.Count && k < tmpIndex.Count; k++)
                        //            {
                        //                Reads.FloatValue.Value[tmpIndex[k]] = tmpFloat[k];
                        //            }
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.Int:
                        //        if (Communicates[index].Meters[i].Value.Read<int>(out tmpInt, Communicates[index].Meters[i].Reads[j]))
                        //        {
                        //            for (int k = 0; k < tmpInt.Count && k < tmpIndex.Count; k++)
                        //            {
                        //                Reads.IntValue.Value[tmpIndex[k]] = tmpInt[k];
                        //            }
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.Long:
                        //        if (Communicates[index].Meters[i].Value.Read<long>(out tmpLong, Communicates[index].Meters[i].Reads[j]))
                        //        {
                        //            for (int k = 0; k < tmpLong.Count && k < tmpIndex.Count; k++)
                        //            {
                        //                Reads.LongValue.Value[tmpIndex[k]] = tmpLong[k];
                        //            }
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.String:
                        //        if (Communicates[index].Meters[i].Value.Read<string>(out tmpString, Communicates[index].Meters[i].Reads[j]))
                        //        {
                        //            for (int k = 0; k < tmpString.Count && k < tmpIndex.Count; k++)
                        //            {
                        //                Reads.StringValue.Value[tmpIndex[k]] = tmpString[k];
                        //            }
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.UShort:
                        //        if (Communicates[index].Meters[i].Value.Read<ushort>(out tmpUshort, Communicates[index].Meters[i].Reads[j]))
                        //        {
                        //            for (int k = 0; k < tmpUshort.Count && k < tmpIndex.Count; k++)
                        //            {
                        //                Reads.UshortValue.Value[tmpIndex[k]] = tmpUshort[k];
                        //            }
                        //        }
                        //        break;
                        //}
                    }
                }
            }
        }
        /// <summary>
        /// 初始化各类型数据到总数据列表
        /// </summary>
        /// <returns></returns>
        private void InitData()
        {
            int maxIndex = 0;
            Dictionary<int,string> tmpIndexs;
            if (Communicates == null || Communicates.Count <= 0)
            {
                All.Class.Error.Add("没有通讯类须要读取");
                return;
            }
            for (int i = 0; i < Communicates.Count; i++)
            {
                if (Communicates[i].Meters == null || Communicates[i].Meters.Count <= 0)//父通讯类下没有设备,则继续
                {
                    All.Class.Error.Add(string.Format("{0},没有通讯设备", Communicates[i].Value.Text));
                    continue;
                }
                for (int j = 0; j < Communicates[i].Meters.Count; j++)
                {
                    if (Communicates[i].Meters[j].Reads == null)//有通讯设备,但不读取数据,则继续
                    {
                        continue;
                    }
                    for (int k = 0; k < Communicates[i].Meters[j].Reads.Count; k++)
                    {
                        maxIndex = -1;
                        if (!Communicates[i].Meters[j].Reads[k].Values.ContainsKey("Data") ||//不包含数据类型,则继续
                            !Communicates[i].Meters[j].Reads[k].Values.ContainsKey("Index"))//没有数据序号,则继续
                        {
                            All.Class.Error.Add(string.Format("{0}.{1},没有数据类型或数据序号", 
                                Communicates[i].Value.Text,
                                Communicates[i].Meters[j].Value.Text));
                            continue;
                        }
                        Communicates[i].Meters[j].Reads[k].ReadType = All.Class.TypeUse.GetType(Communicates[i].Meters[j].Reads[k].Values["Data"]);
                        tmpIndexs = Data.ReadDataStyle<int>.GetIndexFromSet(
                            Communicates[i].Meters[j].Reads[k].Values["Index"], Communicates[i].Meters[j].Reads[k].Values["Text"]);
                        if (tmpIndexs == null || tmpIndexs.Count <= 0)//解析不出序号
                        {
                            All.Class.Error.Add(string.Format("{0}.{1},解析不出序号",
                                Communicates[i].Value.Text,
                                Communicates[i].Meters[j].Value.Text));
                            continue;
                        }
                        switch (Communicates[i].Meters[j].Reads[k].ReadType)
                        {
                            case Class.TypeUse.TypeList.UnKnow:
                                break;
                            case Class.TypeUse.TypeList.Bytes:
                                
                                break;
                        }
                        for (int l = 0; l < tmpIndexs.Count; l++)
                        {
                           
                        }
                        //if (tmpIndexs.Min() < 0)
                        //{
                        //    Communicates[i].Meters[j].Reads[k].NeedToRead = false;
                        //    All.Class.Error.Add(string.Format("{0}.{1},解析序号不正确,出现负数",
                        //        Communicates[i].Value.Text,
                        //        Communicates[i].Meters[j].Value.Text));
                        //    continue;
                        //}
                        //maxIndex = tmpIndexs.Max();
                        //switch (Communicates[i].Meters[j].Reads[k].ReadType)
                        //{
                        //    case Class.TypeUse.TypeList.UnKnow:
                        //        Communicates[i].Meters[j].Reads[k].NeedToRead = false;
                        //        break;
                        //    case Class.TypeUse.TypeList.Bytes:
                        //        for (int l = Reads.BytesValue.Value.Count; l <= maxIndex; l++)
                        //        {
                        //            Reads.BytesValue.Value.Add(null);
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.Boolean:
                        //        for (int l = Reads.BoolValue.Value.Count; l <= maxIndex; l++)
                        //        {
                        //            Reads.BoolValue.Value.Add(false);
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.Byte:
                        //        for (int l = Reads.ByteValue.Value.Count; l <= maxIndex; l++)
                        //        {
                        //            Reads.ByteValue.Value.Add(0);
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.DateTime:
                        //        for (int l = Reads.DateTimeValue.Value.Count; l <= maxIndex; l++)
                        //        {
                        //            Reads.DateTimeValue.Value.Add(DateTime.Now);
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.Double:
                        //        for (int l = Reads.DoubleValue.Value.Count; l <= maxIndex; l++)
                        //        {
                        //            Reads.DoubleValue.Value.Add(0);
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.Float:
                        //        for (int l = Reads.BytesValue.Value.Count; l <= maxIndex; l++)
                        //        {
                        //            Reads.BytesValue.Value.Add(null);
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.Int:
                        //        for (int l = Reads.IntValue.Value.Count; l <= maxIndex; l++)
                        //        {
                        //            Reads.IntValue.Value.Add(0);
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.Long:
                        //        for (int l = Reads.LongValue.Value.Count; l <= maxIndex; l++)
                        //        {
                        //            Reads.LongValue.Value.Add(0);
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.String:
                        //        for (int l = Reads.StringValue.Value.Count; l <= maxIndex; l++)
                        //        {
                        //            Reads.StringValue.Value.Add("");
                        //        }
                        //        break;
                        //    case Class.TypeUse.TypeList.UShort:
                        //        for (int l = Reads.UshortValue.Value.Count; l <= maxIndex; l++)
                        //        {
                        //            Reads.UshortValue.Value.Add(0);
                        //        }
                        //        break;
                        //}
                    }
                }
            }
        }
        /// <summary>
        /// 从XML文件读取设置
        /// </summary>
        private void LoadFromXml(string fileName)
        {
            XmlNode tmpXml = Class.XmlHelp.GetXmlNode(string.Format("{0}\\{1}", Class.FileIO.NowPath,fileName));
            if (tmpXml == null)
            {
                All.Class.Error.Add("加载错误", "从MeterConnect.xml加载文件读取数据失败,不能读取数据");
            }
            Data.CommunicateStyle tmpCommunicateStyle;
            Data.MeterStyle tmpMeterStyle;
            foreach (XmlNode tmpConnectNode in tmpXml.ChildNodes)//取所有Connect
            {
                if (tmpConnectNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                //反射通讯类
                tmpCommunicateStyle = new Data.CommunicateStyle();
                tmpCommunicateStyle.Value = All.Communicate.Communicate.Parse(Class.XmlHelp.GetAttribute(tmpConnectNode));
                if (tmpCommunicateStyle.Value == null)
                {
                    continue;
                }
                foreach (XmlNode tmpMeterNode in tmpConnectNode.ChildNodes)//获取所有设备
                {
                    if (tmpMeterNode.NodeType != XmlNodeType.Element)
                    {
                        continue;
                    }
                    //反射设备类
                    tmpMeterStyle = new Data.MeterStyle();
                    tmpMeterStyle.Value = All.Meter.Meter.Parse(Class.XmlHelp.GetAttribute(tmpMeterNode), tmpCommunicateStyle.Value);
                    if (tmpMeterStyle.Value == null)
                    {
                        continue;
                    }
                    foreach (XmlNode tmpReadAndWriteNode in tmpMeterNode.ChildNodes)
                    {
                        if (tmpReadAndWriteNode.NodeType != XmlNodeType.Element)//不是数据标签则继续
                        {
                            continue;
                        }
                        foreach (XmlNode tmpValue in tmpReadAndWriteNode.ChildNodes)
                        {
                            if (tmpValue.NodeType != XmlNodeType.Element)//排除其他辅助标签
                            {
                                continue;
                            }
                            tmpMeterStyle.Reads.Add(new All.Communicate.Data.MeterStyle.ReadStyle(Class.XmlHelp.GetInner(tmpValue)));
                        }
                    }
                    tmpCommunicateStyle.Meters.Add(tmpMeterStyle);
                }
                Communicates.Add(tmpCommunicateStyle);
            }
        }
    }
}
