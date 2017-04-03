using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter.Elect
{
    public abstract class Elect:Meter
    {
        /// <summary>
        /// 单相表
        /// </summary>
        public class SinglePhase
        {
            /// <summary>
            /// 电压
            /// </summary>
            public float Vol
            { get; set; }
            /// <summary>
            /// 电流
            /// </summary>
            public float Cur
            { get; set; }
            /// <summary>
            /// 功率
            /// </summary>
            public float Power
            { get; set; }
            /// <summary>
            /// 功率因素
            /// </summary>
            public float Factor
            { get; set; }
            /// <summary>
            /// 频率
            /// </summary>
            public float Hz
            { get; set; }
            public SinglePhase()
            {
                this.Vol = 0;
                this.Cur = 0;
                this.Power = 0;
                this.Factor = 0;
                this.Hz = 0;
            }
            /// <summary>
            /// 取三相表中的单相数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static SinglePhase Parse(ThreePhase value)
            {
                SinglePhase result = new SinglePhase();
                result.Vol = value.Vol[0];
                result.Cur = value.Cur[0];
                result.Power = value.Power[0];
                result.Factor = value.Factor[0];
                result.Hz = value.Hz;
                return result;
            }
            /// <summary>
            /// 取所有数据中的三相数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static SinglePhase Parse(ReadValue value)
            {
                SinglePhase result = new SinglePhase();
                result.Vol = value.Vol[0];
                result.Cur = value.Cur[0];
                result.Power = value.Power[0];
                result.Factor = value.Factor[0];
                result.Hz = value.Hz;
                return result;
            }
        }
        /// <summary>
        /// 三相表
        /// </summary>
        public class ThreePhase
        {
            /// <summary>
            /// 单相电压
            /// </summary>
            public float[] Vol
            { get; set; }
            /// <summary>
            /// 单相电流
            /// </summary>
            public float[] Cur
            { get; set; }
            /// <summary>
            /// 单相功率
            /// </summary>
            public float[] Power
            { get; set; }
            /// <summary>
            /// 单相功率因素
            /// </summary>
            public float[] Factor
            { get; set; }
            /// <summary>
            /// 频率
            /// </summary>
            public float Hz
            { get; set; }
            /// <summary>
            /// 总电流
            /// </summary>
            public float SumCur
            {
                get
                {
                    float result = 0;
                    if (Cur != null && Cur.Length > 0)
                    {
                        Cur.ToList().ForEach(value => result += value);
                    }
                    return result;
                }
            }
            /// <summary>
            /// 平均电压
            /// </summary>
            public float AvgVol
            {
                get
                {
                    float result = 0;
                    if (Vol != null && Vol.Length > 0)
                    {
                        result = Vol.ToList().Average();
                    }
                    return result;
                }
            }
            /// <summary>
            /// 总功率
            /// </summary>
            public float SumPower
            {
                get
                {
                    float result = 0;
                    if (Power != null && Power.Length > 0)
                    {
                        Power.ToList().ForEach(value => result += value);
                    }
                    return result;
                }
            }
            public ThreePhase()
            {
                this.Vol = new float[3];
                this.Cur = new float[3];
                this.Power = new float[3];
                this.Factor = new float[3];
                this.Hz = 0;
            }
            /// <summary>
            /// 取所有数据中的三相数据
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static ThreePhase Parse(ReadValue value)
            {
                ThreePhase result = new ThreePhase();
                result.Vol[0] = value.Vol[0];
                result.Vol[1] = value.Vol[1];
                result.Vol[2] = value.Vol[2];
                result.Cur[0] = value.Cur[0];
                result.Cur[1] = value.Cur[1];
                result.Cur[2] = value.Cur[2];
                result.Power[0] = value.Power[0];
                result.Power[1] = value.Power[1];
                result.Power[2] = value.Power[2];
                result.Factor[0] = value.Factor[0];
                result.Factor[1] = value.Factor[1];
                result.Factor[2] = value.Factor[2];
                result.Hz = value.Hz;
                return result;
            }
        }
        public class ReadValue
        {
            /// <summary>
            /// A-B线电压,B-C线电压,C-A线电压,平均线电压
            /// </summary>
            public float[] VolLine
            { get; set; }
            /// <summary>
            /// A相电压,B相电压,C相电压,平均相电压
            /// </summary>
            public float[] Vol
            { get; set; }
            /// <summary>
            /// A相电流,B相电流,C相电流,平均电流
            /// </summary>
            public float[] Cur
            { get; set; }
            /// <summary>
            /// A相有功功率,B相有功功率,C相有功功率,总有功功率
            /// </summary>
            public float[] Power
            { get; set; }
            /// <summary>
            /// A相无功功率,B相无功功率,C相无功功率,总无功功率
            /// </summary>
            public float[] PowerNoUse
            { get; set; }
            /// <summary>
            /// A相视在功率,B相视在功率,C相视在功率,总视在功率
            /// </summary>
            public float[] PowerAll
            { get; set; }
            /// <summary>
            /// A相功率因素,B相功率因素,C相功率因素,总功率因素
            /// </summary>
            public float[] Factor
            { get; set; }
            /// <summary>
            /// 频率
            /// </summary>
            public float Hz
            { get; set; }
            public ReadValue()
            {
                this.Vol = new float[4];
                this.VolLine = new float[4];
                this.Cur = new float[4];
                this.Power = new float[4];
                this.PowerNoUse = new float[4];
                this.PowerAll = new float[4];
                this.Factor = new float[4];
                this.Hz = 0;
            }
        }
        /// <summary>
        /// 读取表的单相数据
        /// </summary>
        /// <param name="reading"></param>
        /// <returns></returns>
        public virtual bool Reading(out SinglePhase reading)
        {
            ReadValue value = new ReadValue();
            bool result = Reading(out value);
            reading = SinglePhase.Parse(value);
            return result;
        }
        /// <summary>
        /// 读取表的三相数据
        /// </summary>
        /// <param name="reading"></param>
        /// <returns></returns>
        public virtual bool Reading(out ThreePhase reading)
        {
            ReadValue value = new ReadValue();
            bool result = Reading(out value);
            reading = ThreePhase.Parse(value);
            return result;
 
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reading"></param>
        /// <returns></returns>
        public virtual bool Reading(out ReadValue reading)
        {
            List<float> value = new List<float>();
            bool result = Read<float>(out value, null);
            reading = new ReadValue();
            //Vol*4,VolLine*4,Cur*4,Power*4,PowerNoUse*4,PowerAll*4,Factor*4,Hz*1,.....固定格式
            if (result && value != null && value.Count == 29)
            {
                int index = 0;
                for (int i = 0; i < reading.Vol.Length; i++)
                {
                    reading.Vol[i] = value[index++];
                }
                for (int i = 0; i < reading.VolLine.Length; i++)
                {
                    reading.VolLine[i] = value[index++];
                }
                for (int i = 0; i < reading.Cur.Length; i++)
                {
                    reading.Cur[i] = value[index++];
                }
                for (int i = 0; i < reading.Power.Length; i++)
                {
                    reading.Power[i] = value[index++];
                }
                for (int i = 0; i < reading.PowerNoUse.Length; i++)
                {
                    reading.PowerNoUse[i] = value[index++];
                }
                for (int i = 0; i < reading.PowerAll.Length; i++)
                {
                    reading.PowerAll[i] = value[index++];
                }
                for (int i = 0; i < reading.Factor.Length; i++)
                {
                    reading.Factor[i] = value[index++];
                }
                reading.Hz = value[index++];
            }
            return result;
        }
    }
}
