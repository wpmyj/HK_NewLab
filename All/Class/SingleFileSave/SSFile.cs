using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace All.Class.SingleFileSave
{
    /// <summary>
    /// XML文件格式保存文本
    /// </summary>
    public class SSFile
    {
        [Serializable]
        public class Nodes//此处因为用到序列化,所以必须为public
        {
            [Serializable]
            public class Node
            {
                /// <summary>
                /// 标题
                /// </summary>
                public string Title
                { get; set; }
                /// <summary>
                /// 数据
                /// </summary>
                public string Value
                { get; set; }
                public Node()
                {
                    Title = "";
                    Value = "";
                }
                public Node(string title, string value)
                {
                    this.Title = title;
                    this.Value = value;
                }
            }
            /// <summary>
            /// 所有数据
            /// </summary>
            public List<Node> AllNode
            { get; set; }
            public Nodes()
            {
                AllNode = new List<Node>();
            }
            /// <summary>
            /// 序列化为字符串
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                string result = "";
                XmlSerializer xs = new XmlSerializer(typeof(Nodes));
                using (MemoryStream stream = new MemoryStream())
                {
                    xs.Serialize(stream, this);
                    stream.Position = 0;
                    StreamReader sr = new StreamReader(stream);
                    result = sr.ReadToEnd();
                }
                return result;
            }
            /// <summary>
            /// 字符串解析为值
            /// </summary>
            /// <param name="value"></param>
            public void Init(string value)
            {
                XmlSerializer xs = new XmlSerializer(typeof(Nodes));
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.Write(value);
                        sw.Flush();
                        stream.Position = 0;
                        Nodes tmp = (Nodes)xs.Deserialize(stream);
                        this.AllNode = tmp.AllNode;
                    }
                }
            }
        }
        /// <summary>
        /// 将字典转化为标准字符串
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static string Dictionary2Text(Dictionary<string, string> buff)
        {
            Nodes nodes = new Nodes();
            Nodes.Node node;
            buff.Keys.ToList().ForEach(key =>
            {
                node = new Nodes.Node(key, buff[key]);
                nodes.AllNode.Add(node);
            });
            return nodes.ToString();
        }
        /// <summary>
        /// 将字符串转化为字典
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Text2Dictionary(string value)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                Nodes nodes = new Nodes();
                nodes.Init(value);
                nodes.AllNode.ForEach(node =>
                {
                    if (!result.ContainsKey(node.Title))
                    {
                        result.Add(node.Title, node.Value);
                    }
                });
            }
            catch (Exception e)
            {
                All.Class.Error.Add(e);
                All.Class.Error.Add("出错字符", value);
            }
            return result;
        }
        /// <summary>
        /// 将任意类型转化为数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Object2Byte(object value)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, value);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// 将数组还原为类型
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static object Byte2Object(byte[] buff)
        {
            return Byte2Object(buff, 0, buff.Length);
        }
        /// <summary>
        /// 将数组还原为类型
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static object Byte2Object(byte[] buff,int start,int len)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(buff, start, len);
                ms.Position = 0;
                return bf.Deserialize(ms);
            }
        }
    }
}
