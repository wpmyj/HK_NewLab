using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
namespace All.Class
{
    //<cTodaySet>

    //  <mTodaySet SwitchTime="12:00">
    //  </mTodaySet>

    //  <TodayCount>
    //    <Hour9>431</Hour9>
    //    <Hour10>563</Hour10>
    //  </TodayCount>

    //</cTodaySet>

    //GetInner("TodayCount", "Hour9")返回"431"
    //GetAttribute("mTodaySet", "SwitchTime")返回"12:00"
    public static class XmlHelp
    {
        /// <summary>
        /// 读取xml文档
        /// </summary>
        /// <param name="filePath">文档位置</param>
        public static XmlNode GetXmlNode(string filePath)
        {
            XmlDocument tmp = new XmlDocument();
            XmlNode tmpNode = null;
            try
            {
                tmp.Load(filePath);
                if (tmp.ChildNodes.Count >= 2)
                {
                    if (tmp.ChildNodes[0].Value.ToUpper() == "VERSION=\"1.0\" ENCODING=\"GB2312\"" ||
                       tmp.ChildNodes[0].Value.ToUpper() == "VERSION=\"1.0\" ENCODING=\"UTF-8\"")
                    {
                        tmpNode = tmp.ChildNodes[1];
                    }
                    else
                    {
                        Error.Add("文档错误", "XML文档编码必须为<?xml version=\"1.0\" encoding=\"GB2312\" ?>");
                    }
                }
            }
            catch (Exception e)
            {
                Error.Add(e);
            }
            return tmpNode;
        }
        /// <summary>
        /// 获取父节点下所有子节点名
        /// </summary>
        /// <param name="patherNode">父节点名称</param>
        /// <returns>子节点集合</returns>
        public static string[] GetNodeName(XmlNode patherNode)
        {
            string[] nodesName = new string[patherNode.ChildNodes.Count];
            for (int i = 0; i < nodesName.Length; i++)
            {
                nodesName[i] = patherNode.ChildNodes[i].Name;
            }
            return nodesName;
        }
        /// <summary>
        /// 获取xml文档中的所有附加数据
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAttribute(XmlNode tmpNode)
        {
            Dictionary<string, string> value = new Dictionary<string, string>();
            foreach (XmlAttribute tmpAttribute in tmpNode.Attributes)
            {
                value.Add(tmpAttribute.Name, tmpAttribute.Value);
            }
            return value;
        }
        ///// <summary>
        ///// 获取xml文档中的附加数据
        ///// </summary>
        ///// <param name="nodeName">子节点名</param>
        ///// <param name="attribute">附加属性名</param>
        ///// <returns>附加属性值</returns>
        //public string GetAttribute(string nodeName, string attributeName)
        //{
        //    if (FatherNode == null)
        //    {
        //        return "";
        //    }
        //    string value = string.Empty;
        //    foreach (XmlNode tmpNode in FatherNode.ChildNodes)
        //    {
        //        if (tmpNode.Name == nodeName)
        //        {
        //            if (tmpNode.Attributes[attributeName] != null)
        //            {
        //                value = tmpNode.Attributes[attributeName].Value;
        //            }
        //            else
        //            {
        //                value = "";
        //            }
        //            break;
        //        }
        //    }
        //    return value;
        //}
        /// <summary>
        /// 获取指定节点的Inner值
        /// </summary>
        /// <param name="tmpNode"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetInner(XmlNode tmpNode)
        {
            Dictionary<string, string> tmpValue = new Dictionary<string, string>();
            foreach (XmlNode tmp in tmpNode.ChildNodes)
            {
                if (tmp.NodeType == XmlNodeType.Element)
                {
                    tmpValue.Add(tmp.Name, tmp.InnerText);
                }
            }
            return tmpValue;
        }
        ///// <summary>
        ///// 获取xml文档二级Node的Inner值
        ///// </summary>
        ///// <param name="nodeName">Node名称</param>
        ///// <param name="InnerName">获取Inner值的二级Node名称</param>
        ///// <returns>Inner值</returns>
        //public static string GetInner(string nodeName, string InnerName)
        //{
        //    if (FatherNode == null)
        //    {
        //        return "";
        //    }
        //    string value = string.Empty;
        //    foreach (XmlNode tmpNode in FatherNode.ChildNodes)
        //    {
        //        if (tmpNode.Name == nodeName)
        //        {
        //            if (tmpNode[InnerName] != null)
        //            {
        //                value = tmpNode[InnerName].InnerText;
        //            }
        //            break;
        //        }
        //    }
        //    return value;
        //}
        /// <summary>
        /// 保存数据到XML文件，反射的类一定要包含无参数的构造函数，否则反射失败
        /// </summary>
        /// <param name="filePath">string,要保存的文件路径</param>
        /// <param name="type">Type,数据类型</param>
        /// <param name="ob">object,要保存的数据</param>
        public static bool SaveXml(string filePath, Type type, object ob)
        {
            bool isOk = false;
            try
            {
                string DirectoryPath = filePath.Substring(0, filePath.LastIndexOf("\\")) + "\\";
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
                XmlSerializer xs = new XmlSerializer(type);
                Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                xs.Serialize(stream, ob);
                stream.Close();
                isOk = true;
            }
            catch { }
            return isOk;
        }
        /// <summary>
        ///  读取XML文件到数据类
        /// </summary>
        /// <param name="filePath">string,要读取的文件路径</param>
        /// <param name="type">Type,数据类型</param>
        /// <param name="DefaultData">object,读取默认的数据</param>
        /// <returns>object,读取到的数据</returns>
        public static object ReadXml(string filePath, Type type, object DefaultData)
        {
            object o = new object();
            try
            {
                XmlSerializer xs = new XmlSerializer(type);
                using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    o = xs.Deserialize(stream);
                    stream.Close();
                }
            }
            catch
            {
                SaveXml(filePath, type, DefaultData);
                return DefaultData;
            }
            return o;
        }
    }
}
