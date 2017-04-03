using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace All.Class
{
    public class TypeUse
    {
        /// <summary>
        /// 当前使用数据类型列表
        /// </summary>
        public enum TypeList : byte
        {
            UnKnow = 0,
            Bytes,
            Byte,
            String,
            Double,
            UShort,
            Int,
            Float,
            Boolean,
            DateTime,
            Long
        }
        /// <summary>
        /// 将数值转化读取类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TypeList GetType(byte value)
        {
            TypeList result = TypeList.UnKnow;
            if (value >= 0 && value < Enum.GetNames(typeof(TypeList)).Length)
            {
                result = (TypeList)value;
            }
            return result;
        }
        /// <summary>
        /// 将字符转化为读取类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TypeList GetType(string value)
        {
            TypeList result = TypeList.UnKnow;
            switch (value.ToUpper())
            {
                case "BYTES":
                    result = TypeList.Bytes;
                    break;
                case "LONG":
                    result = TypeList.Long;
                    break;
                case "BYTE":
                    result = TypeList.Byte;
                    break;
                case "STRING":
                    result = TypeList.String;
                    break;
                case "DOUBLE":
                    result = TypeList.Double;
                    break;
                case "USHORT":
                    result = TypeList.UShort;
                    break;
                case "INT":
                    result = TypeList.Int;
                    break;
                case "SINGLE":
                case "FLOAT":
                    result = TypeList.Float;
                    break;
                case "BOOL":
                case "BOOLEAN":
                    result = TypeList.Boolean;
                    break;
                case "DATETIME":
                    result = TypeList.DateTime;
                    break;
            }
            return result;
        }
        /// <summary>
        /// 将类型转化为读取类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TypeList GetType<T>()
        {
            TypeList result = TypeList.UnKnow;
            switch (System.Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Object:
                    result = TypeList.Bytes;
                    break;
                case TypeCode.Int64:
                    result = TypeList.Long;
                    break;
                case TypeCode.Byte:
                    result = TypeList.Byte;
                    break;
                case TypeCode.String:
                    result = TypeList.String;
                    break;
                case TypeCode.Double:
                    result = TypeList.Double;
                    break;
                case TypeCode.UInt16:
                    result = TypeList.UShort;
                    break;
                case TypeCode.Int32:
                    result = TypeList.Int;
                    break;
                case TypeCode.Single:
                    result = TypeList.Float;
                    break;
                case TypeCode.Boolean:
                    result = TypeList.Boolean;
                    break;
                case TypeCode.DateTime:
                    result = TypeList.DateTime;
                    break;
            }
            return result;
        }
    }
}
