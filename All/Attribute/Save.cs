using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Attribute
{
    public enum Statue
    {
        /// <summary>
        /// 无附加信息,保存数据但不作为关键数据,即数据库中普通数据
        /// </summary>
        None = 0,
        /// <summary>
        /// 关键属性,保存数据,且当前属性在数据库中设置为Key
        /// </summary>
        Key = 1,
        /// <summary>
        /// 自动保存类,程序初始化时,会将当前类自动序列化为数据库
        /// </summary>
        AutoSave = 2
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class SaveAttribute:System.Attribute
    {
        public readonly Statue Statue = Statue.None;
        /// <summary>
        /// 设置项目信息
        /// </summary>
        /// <param name="statue">项目的状态</param>
        public SaveAttribute(Statue statue)
        {
            this.Statue = statue;
        }
    }

}
