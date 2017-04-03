using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace All.Class
{
    public class Net
    {
        /// <summary>
        /// Ping线程数量
        /// </summary>
        const int PingThreadCount = 30;
        public class PingResult
        {
            /// <summary>
            /// 序号
            /// </summary>
            public int Index
            {get;set;}
            /// <summary>
            /// IP地址
            /// </summary>
            public string IpAddress
            {get;set;}
            /// <summary>
            /// 测试结果
            /// </summary>
            public bool Result
            {get;set;}
            public PingResult(int index, string ipAddress, bool result)
            {
                this.Index = index;
                this.IpAddress = ipAddress;
                this.Result = result;
            }
        }
        public event Action<PingResult> PingResultArgs;
        public event Action<List<PingResult>> PingAllResultArgs;
        Thread[] thPing = new Thread[PingThreadCount];
        List<int>[] InPing = new List<int>[PingThreadCount];//分组
        bool[] OverPing = new bool[PingThreadCount];//是否Ping完成
        List<PingResult> allResult = new List<PingResult>();
        public void Ping(string start,string end)
        {
            Stop();
            string[] buffStart = start.Split('.');
            string[] buffEnd = end.Split('.');
            if (!Check.isFix(start, Check.RegularList.IP地址)
                || !Check.isFix(end, Check.RegularList.IP地址) ||
                buffStart == null || buffStart.Length != 4 ||
                buffEnd == null || buffEnd.Length != 4)
            {
                throw new Exception("当前的IP地址不正确");
            }
            if (buffStart[0] != buffEnd[0] ||
                buffStart[1] != buffEnd[1] ||
                buffStart[2] != buffEnd[2])
            {
                throw new Exception("测试IP地址不能跨网段");
            }
            int indexStart = buffStart[3].ToInt();
            int indexEnd = buffEnd[3].ToInt();
            if (indexStart > indexEnd)
            {
                indexStart = indexStart + indexEnd;
                indexEnd = indexStart - indexEnd;
                indexStart = indexStart - indexEnd;
            }
            Start(buffStart[0].ToByte(), buffStart[1].ToByte(), buffStart[2].ToByte(), indexStart, indexEnd);
        }
        private void Start(byte one, byte two, byte three, int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                InPing[i % PingThreadCount].Add(i);
            }
            for (int i = 0; i < PingThreadCount; i++)
            {
                int index = i;
                thPing[i] = new Thread(() => EveryThreadPing(string.Format("{0}.{1}.{2}.", one, two, three), index));
                thPing[i].IsBackground = true;
                thPing[i].Start();
            }
        }
        public void Stop()
        {
            allResult.Clear();
            for (int i = 0; i < PingThreadCount; i++)
            {
                OverPing[i] = false;
                if (InPing[i] != null)
                {
                    InPing[i].Clear();
                    InPing[i] = null;
                }
                InPing[i] = new List<int>();
                if (thPing[i] != null)
                {
                    if ((thPing[i].ThreadState & ThreadState.Running)==ThreadState.Running)
                    {
                        thPing[i].Abort();
                    }
                    thPing[i] = null;
                }
            }
        }
        private void EveryThreadPing(string host,int index)
        {
            for (int i = 0; i < InPing[index].Count; i++)
            {
                Ping(string.Format("{0}{1}", host, InPing[index][i]), InPing[index][i]);
            }
            Over(index);
        }
        private void Ping(string ipAddress, int index)
        {
            PingResult result = new PingResult(index, ipAddress, false);
            using (System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping())
            {
                try
                {
                    result.Result = (p.Send(ipAddress, 2000).Status == System.Net.NetworkInformation.IPStatus.Success);
                }
                catch { }
            }
            allResult.Add(result);
            if (PingResultArgs != null)
            {
                PingResultArgs(result);
            }
        }
        private void Over(int index)
        {
            OverPing[index] = true;
            if (OverPing.ToList().TrueForAll(tmp => tmp))
            {
                if (PingAllResultArgs != null)
                {
                    PingAllResultArgs(allResult);
                }
            }
        }
    }
}
