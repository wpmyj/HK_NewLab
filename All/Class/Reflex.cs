using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace All.Class
{
    public class Reflex<T> where T : class
    {
        string fileName = "";
        string className = "";
        /// <summary>
        /// 从类命名空间和类名称反射类
        /// </summary>
        /// <param name="NameSpace">反射的文件名称,不包含文件扩展名</param>
        /// <param name="ClassName">完整的类名称,即包含命令空间的类名称</param>
        public Reflex(string fileName, string className)
        {
            this.fileName = fileName;
            this.className = className;
        }
        /// <summary>
        /// 获取反射类
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            if (fileName == "" || className == "")
            {
                return null;
            }
            T tmp = (System.Reflection.Assembly.Load(fileName).CreateInstance(className)) as T;
            return tmp;
        }
    }
}
