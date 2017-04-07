using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace All.Class
{
    public class Code
    {
        public const string Key = "ShuaiShuai19881015";
        public class Region
        {
            public enum ErrorList
            {
                注册码合格,
                格式码错误,
                本地码错误,
                注册码错误,
                注册已到期,
                日期被更改,
                校验错误
            }
            public const string RegionLocalCode = "RegionLocalCode";
            public const string RegionCode = "RegionCode";
            public const string RegionData = "RegionData";
            /// <summary>
            /// 生成本地码
            /// </summary>
            /// <returns>本地码</returns>
            public static string Encryption()
            {
                string localCode = "";
                string str = MD5.Encryption(string.Format("{0}{1}", Key, HardInfo.GetHardInfo(HardInfo.HardList.主板序列号)));
                localCode = str.Substring(4, 4);
                for (int i = 8; i < str.Length - 4; i = i + 4)
                {
                    localCode = localCode + "-" + str.Substring(i, 4);
                }
                return localCode;
            }
            /// <summary>
            /// 从本地码与时间计算出注册码
            /// </summary>
            /// <param name="Code">本地码</param>
            /// <param name="endTime">注册到期时间</param>
            /// <returns>注册码</returns>
            public static string Decryption(string Code, DateTime endTime)
            {
                string localCode = "";
                string str = MD5.Encryption(MD5.Encryption(string.Format("{0}{1}", Key, Code)));
                localCode = GetDataHex(endTime);
                for (int i = 8; i < str.Length - 8; i = i + 4)
                {
                    localCode = localCode + "-" + str.Substring(i, 4);
                }
                string[] tmpChecks = localCode.Split('-');
                int tmpSum = 0;
                for (int i = 0; i < tmpChecks.Length; i++)
                {
                    tmpSum = tmpSum + Convert.ToInt16(tmpChecks[i], 16);
                }
                tmpSum = ((tmpSum & 0xFFFF) ^ 0xAAAA);
                localCode = localCode + "-" + string.Format("{0:X4}", tmpSum);
                return localCode;
            }
            /// <summary>
            /// 判断注册码是否正确
            /// </summary>
            /// <param name="LocalCode">本地码</param>
            /// <param name="Code">注册码</param>
            /// <returns>是否正确注册码</returns>
            public static ErrorList isRegion(string LocalCode, string Code, out string ErrorStr)
            {
                ErrorList errorList = ErrorList.注册码合格;
                ErrorStr = "注册码校验正确通过";
                if (Encryption() != LocalCode)
                {
                    ErrorStr = "本地码不匹配错误";
                    return ErrorList.本地码错误;
                }
                string[] ReadValue = Code.Split('-');
                if (ReadValue == null || ReadValue.Length != 6)
                {
                    ErrorStr = "注册码格式不正确";
                    return ErrorList.格式码错误;
                }
                string[] CureValue = Decryption(LocalCode, GetData(ReadValue[0])).Split('-');
                for (int i = 1; i < ReadValue.Length - 1; i++)
                {
                    if (ReadValue[i] != CureValue[i])
                    {
                        ErrorStr = "输入注册码不正确";
                        return ErrorList.注册码错误;
                    }
                }
                TimeSpan ts = GetData(ReadValue[0]) - DateTime.Now;
                if (ts.TotalDays < 0)
                {
                    ErrorStr = "输入注册码已过期";
                    return ErrorList.注册已到期;
                }
                ts = Class.Num.ToDateTime(Class.Region.ReadValue(RegionData)) - DateTime.Now;
                if (ts.TotalDays > 0)
                {
                    ErrorStr = "电脑本地日期被更改,注册校验失败";
                    return ErrorList.日期被更改;
                }
                int sum = 0;
                for (int i = 0; i < ReadValue.Length - 1; i++)
                {
                    sum = sum + Convert.ToInt16(ReadValue[i], 16);
                }
                if (string.Format("{0:X4}", (sum & 0xFFFF) ^ 0xAAAA) != ReadValue[ReadValue.Length - 1])
                {
                    ErrorStr = "校验码被手动更改错误";
                    return ErrorList.校验错误;
                }
                Class.Region.WriteValue(RegionData, DateTime.Now.ToString());
                return errorList;
            }
            /// <summary>
            /// 判断注册码是否正确
            /// </summary>
            /// <returns>是否正确注册码</returns>
            public static ErrorList isRegion()
            {
                string LocalCode = All.Class.Region.ReadValue(RegionLocalCode);
                string Code = All.Class.Region.ReadValue(RegionCode);
                string str = "";
                return isRegion(LocalCode, Code, out str);
            }
            /// <summary>
            /// 将注册信息写入注册表
            /// </summary>
            public static void WriteInfoToReion(string LocalCode, string code)
            {
                All.Class.Region.WriteValue(RegionLocalCode, LocalCode);
                All.Class.Region.WriteValue(RegionCode, code);
            }
            /// <summary>
            /// 生成一个长度为4的字符串,代表注册日期,只能表示1970-1-1 到 2088-12-31 128年时间
            /// [0000 000][0 000][0 0000]
            /// 年占七位,月占四位,日占五位
            /// </summary>
            /// <returns></returns>
            public static string GetDataHex(DateTime tmp)
            {
                string result = "";
                int year = tmp.Year - 1971;
                int month = tmp.Month - 1;
                int day = tmp.Day - 1;
                year = (year << 1) + (month >> 3);
                month = ((month - ((month >> 3) << 3)) << 1) + (day >> 4);
                day = (day - ((day >> 4) << 4));
                int A = year >> 4;
                int B = year - ((year >> 4) << 4);
                int C = month;
                int D = day;
                result = result + string.Format("{0:X}", B);
                result = result + string.Format("{0:X}", A);
                result = result + string.Format("{0:X}", C);
                result = result + string.Format("{0:X}", D);
                return result;
            }
            /// <summary>
            /// 将生成的日期字符串反算成日期
            /// [0000 000][0 000][0 0000]
            /// 年占七位,月占四位,日占五位
            /// </summary>
            /// <returns></returns>
            public static DateTime GetData(string value)
            {
                DateTime result = new DateTime(1971, 1, 1);
                try
                {
                    int B = Convert.ToInt16(value.Substring(0, 1), 16);
                    int A = Convert.ToInt16(value.Substring(1, 1), 16);
                    int C = Convert.ToInt16(value.Substring(2, 1), 16);
                    int D = Convert.ToInt16(value.Substring(3, 1), 16);
                    int year = (A << 3) + (B >> 1) + 1971;
                    int month = ((B - ((B >> 1) << 1)) << 3) + (C >> 1) + 1;
                    int day = ((C - ((C >> 1) << 1)) << 4) + D + 1;
                    result = new DateTime(year, month, day);
                }
                catch { }
                return result;
            }
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        public class MD5
        {
            /// <summary>
            /// MD5加密
            /// </summary>
            /// <param name="Code"></param>
            /// <returns></returns>
            public static string Encryption(string Code)
            {
                string value = "";
                System.Security.Cryptography.MD5 tmpMD5 = System.Security.Cryptography.MD5.Create();
                byte[] tmpBuff = tmpMD5.ComputeHash(Encoding.UTF8.GetBytes(Code));
                for (int i = 0; i < tmpBuff.Length; i++)
                {
                    value = value + string.Format("{0:X2}", tmpBuff[i]);
                }
                return value;
            }
        }
        /// <summary>
        /// BASE64加密,解密方法
        /// </summary>
        public class Base64
        {
            /// <summary>
            /// 加密
            /// </summary>
            /// <param name="Code">string,加密字符串</param>
            /// <returns>string,返回加密后的字符串</returns>
            public static string Encryption(string Code)
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(Code.Trim())).Trim();
            }
            /// <summary>
            /// 解密
            /// </summary>
            /// <param name="Code">string,解密字符串</param>
            /// <returns>string,返回解密后的字符串</returns>
            public static string Decryption(string Code)
            {
                string data = "";
                try
                {
                    data = Encoding.UTF8.GetString(Convert.FromBase64String(Code.Trim()));
                }
                catch (Exception exc)
                {
                    throw exc;
                }
                return data;
            }
        }
        /// <summary>
        /// 用户加密
        /// </summary>
        public class File
        {
            /// <summary>
            /// 账户文件加密，切换用户后，不能操作此文件
            /// </summary>
            /// <param name="filePath"></param>
            public static void Encrypt(string filePath)
            {
                System.IO.File.Encrypt(filePath);
            }
            /// <summary>
            /// 账户文件解密
            /// </summary>
            /// <param name="filePath"></param>
            public static void Decrypt(string filePath)
            {
                System.IO.File.Decrypt(filePath);
            }
        }
        /// <summary>
        /// 字符串16进制加密
        /// </summary>
        public class StringHex
        {
            static byte[] code = Encoding.ASCII.GetBytes(Key);
            /// <summary>
            /// 将字符加密保存到文件
            /// </summary>
            /// <param name="code"></param>
            /// <param name="fileName"></param>
            public static void SaveFile(string code, string fileName)
            {
                System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
                byte[] buff = encryption(code);
                fs.Write(buff, 0, buff.Length);
                fs.Flush();
                fs.Close();
                fs.Dispose();
                fs = null;
            }
            public static string ReadFile(string fileName)
            {
                System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
                byte[] buff = new byte[fs.Length];
                fs.Read(buff, 0, buff.Length);
                fs.Flush();
                fs.Close();
                fs.Dispose();
                fs = null;
                return decryption(buff);
            }
            /// <summary>
            /// 加密
            /// </summary>
            /// <param name="Code">string,加密字符串</param>
            /// <returns>加密字节</returns>
            static byte[] encryption(string Code)
            {
                byte[] buff = Encoding.UTF8.GetBytes(string.Format("{0}{1}", Key, Code));
                int index = 0;
                for (int i = 0; i < buff.Length; i++)
                {
                    buff[i] = (byte)(buff[i] ^ code[index]);
                    index = (index++) % code.Length;
                }
                return buff;
            }
            /// <summary>
            /// 加密
            /// </summary>
            /// <param name="Code">byte[],加密字节</param>
            /// <returns>string,加密码后的字符串</returns>
            public static string Encryption(string Code)
            {
                byte[] buff = encryption(Code);
                string value = "";
                for (int i = 0; i < buff.Length; i++)
                {
                    value = string.Format("{0}{1:X2}", value, buff[i]);
                }
                return value;
            }
            /// <summary>
            /// 将16进制字符串解为Byte数组
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            static string decryption(byte[] buff)
            {
                string value = "Error";
                try
                {
                    int index = 0;
                    for (int i = 0; i < buff.Length; i++)
                    {
                        buff[i] = (byte)(buff[i] ^ code[index]);
                        index = (index++) % code.Length;
                    }
                    value = Encoding.UTF8.GetString(buff);
                }
                catch { }
                if (value.IndexOf(Key) != 0)
                {
                    return @"{\rtf1\ansi\ansicpg936\deff0\deflang1033\deflangfe2052{\fonttbl{\f0\fnil\fcharset134 \'cb\'ce\'cc\'e5;}}\viewkind4\uc1\pard\lang2052\f0\fs18\'ce\'de\'d0\'a7\'ce\'c4\'bc\'fe,\'c7\'eb\'b4\'f2\'bf\'aa\'d6\'b8\'b6\'a8\'b8\'f1\'ca\'bd\'ce\'c4\'bc\'fe\par}";
                }
                return value.Replace(Key, "");
            }
            /// <summary>
            /// 解密
            /// </summary>
            /// <param name="str">要解密的字符串</param>
            /// <returns>string,解密后字符串</returns>
            public static string Decryption(string str)
            {
                if (str.Length % 2 != 0)
                {
                    return null;
                }
                byte[] buff = new byte[str.Length / 2];
                try
                {
                    for (int i = 0; i < buff.Length; i++)
                    {
                        buff[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
                    }
                }
                catch { }

                return decryption(buff);
            }
        }
    }
}
