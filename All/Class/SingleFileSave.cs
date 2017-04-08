using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace All.Class
{
    /// <summary>
    /// 本地单个文件存储数据
    /// </summary>
    public class SingleFileSave
    {
        /// <summary>
        /// INI文件格式保存文本
        /// </summary>
        public static class Ini
        {
            /// <summary>
            /// 将指定数据写入指定ini文件
            /// </summary>
            /// <param name="fileName"></param>
            /// <param name="title"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public static void Write(string fileName, string section, string key, string value)
            {
                All.Class.Api.WritePrivateProfileString(section, key, value, fileName);
            }
            /// <summary>
            /// 从文件读取指定数据
            /// </summary>
            /// <param name="fileName"></param>
            /// <param name="section"></param>
            /// <param name="key"></param>
            /// <param name="defaultValue"></param>
            /// <returns></returns>
            public static string Read(string fileName, string section, string key, string defaultValue)
            {
                StringBuilder result = new StringBuilder(255);
                try
                {
                    All.Class.Api.GetPrivateProfileString(section, key, defaultValue, result, 255, fileName);
                }
                catch { }
                return result.ToString().Trim();
            }
            /// <summary>
            /// 将字典转化为标准字符串
            /// </summary>
            /// <param name="buff"></param>
            /// <returns></returns>
            public static string Dictionary2Text(string section,Dictionary<string, string> buff)
            {
                StringBuilder result = new StringBuilder(500);
                result.Append(string.Format("[{0}]\r\n", section));
                buff.Keys.ToList().ForEach(key =>
                {
                    result.Append(string.Format("{0}={1}\r\n", key, buff[key].Trim()));
                });
                return result.ToString().Trim();
            }
            public static Dictionary<string, string> Text2Dictionary(string section, string value)
            {
                Dictionary<string, string> buff = new Dictionary<string, string>();
                string[] tmp = value.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                if (tmp != null && tmp.Length > 0)
                {
                    for (int i = 0; i < tmp.Length - 1; i++)
                    {
                        if (tmp[i].ToUpper() == section.ToUpper())
                        {
                            string[] tmpSection = tmp[i+1].Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            if (tmpSection != null && tmpSection.Length > 0)
                            {
                                string key, data;
                                for (int j = 0; j < tmp.Length; j++)
                                {
                                    key = tmpSection[j].Substring(0, tmpSection[j].IndexOf('=')).Trim();
                                    data = tmpSection[j].Substring(tmpSection[j].IndexOf('=') + 1).Trim();
                                    if (!buff.ContainsKey(key))
                                    {
                                        buff.Add(key, data);
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
                return buff;
            }
        }
        /// <summary>
        /// XML文件格式保存文本
        /// </summary>
        public class SSFile
        {
            [Serializable]
            public class Nodes
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
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(buff, 0, buff.Length);
                    ms.Position = 0;
                    return bf.Deserialize(ms);
                }
            }
        }
    }
}
