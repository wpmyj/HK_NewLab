using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace All.Class
{
    public class Reflex<T> where T : class
    {
        string nameSpace = "";
        string className = "";
        /// <summary>
        /// 从类命名空间和类名称反射类
        /// </summary>
        /// <param name="NameSpace"></param>
        /// <param name="ClassName"></param>
        public Reflex(string NameSpace, string ClassName)
        {
            nameSpace = NameSpace;
            className = ClassName;
        }
        /// <summary>
        /// 获取反射类
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            if (nameSpace == "" || className == "")
            {
                return null;
            }
            T tmp = (System.Reflection.Assembly.Load(nameSpace).CreateInstance(className)) as T;
            return tmp;
        }
    }
}
