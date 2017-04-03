using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Factory
{
    /// <summary>
    /// 德国博世品牌
    /// </summary>
    public class Boss
    {
        /// <summary>
        /// 将正常日期转化为博世日期
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetBossTime(DateTime time)
        {
            string result = "";
            switch (time.Year)
            {
                case 2011:
                    result = string.Format("{0:D3}", 101 - 1 + time.Month);
                    break;
                case 2012:
                    result = string.Format("{0:D3}", 201 - 1 + time.Month);
                    break;
                case 2013:
                    result = string.Format("{0:D3}", 301 - 1 + time.Month);
                    break;
                case 2014:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 417 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 453 - 5 + time.Month);
                    }
                    break;
                case 2015:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 517 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 553 - 5 + time.Month);
                    }
                    break;
                case 2016:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 617 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 653 - 5 + time.Month);
                    }
                    break;
                case 2017:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 717 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 753 - 5 + time.Month);
                    }
                    break;
                case 2018:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 817 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 853 - 5 + time.Month);
                    }
                    break;
                case 2019:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 917 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 953 - 5 + time.Month);
                    }
                    break;
                case 2020:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 037 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 073 - 5 + time.Month);
                    }
                    break;
                case 2021:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 137 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 173 - 5 + time.Month);
                    }
                    break;
                case 2022:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 237 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 273 - 5 + time.Month);
                    }
                    break;
                case 2023:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 337 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 373 - 5 + time.Month);
                    }
                    break;
                case 2024:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 437 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 473 - 5 + time.Month);
                    }
                    break;
                case 2025:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 537 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 573 - 5 + time.Month);
                    }
                    break;
                case 2026:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 637 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 673 - 5 + time.Month);
                    }
                    break;
                case 2027:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 737 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 773 - 5 + time.Month);
                    }
                    break;
                case 2028:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 837 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 873 - 5 + time.Month);
                    }
                    break;
                case 2029:
                    if (time.Month <= 4)
                    {
                        result = string.Format("{0:D3}", 937 - 1 + time.Month);
                    }
                    else
                    {
                        result = string.Format("{0:D3}", 973 - 5 + time.Month);
                    }
                    break;
            }
            return result;
        }
    }
}

