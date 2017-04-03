using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Communicate.Data
{
    /// <summary>
    /// 读取数据内容
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadDataStyle<T>
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="index">数据序列号</param>
        /// <param name="lastValue">前一值</param>
        /// <param name="value">当前值</param>
        public delegate void ValueReadHandle(int index, T lastValue, T value);
        /// <summary>
        /// 当前值
        /// </summary>
        public List<T> Value
        { get; set; }
        /// <summary>
        /// 数据名称
        /// </summary>
        public List<string> Info
        { get; set; }
        public event ValueReadHandle ValueChange;
        /// <summary>
        /// 数据读取完毕事件
        /// </summary>
        public event ValueReadHandle ValueReadOver;
        public ReadDataStyle()
        {
            Value = new List<T>();
            Info = new List<string>();
        }
        /// <summary>
        /// 清空所有数据
        /// </summary>
        public void Clear()
        {
            Value.Clear();
            Info.Clear();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void SetValue(int index, T value)
        {
            if (Value == null || index < 0 || index >= Value.Count)
            {
                return;
            }
            if (ValueChange != null && !Value[index].Equals(value))
            {
                ValueChange(index, Value[index], value);
            }
            if (ValueReadOver != null)
            {
                ValueReadOver(index, Value[index], value);
            }
        }
        /// <summary>
        /// 添加一组数据
        /// </summary>
        /// <param name="info"></param>
        public void Add(string info)
        {
            switch (All.Class.TypeUse.GetType<T>())
            {
                case Class.TypeUse.TypeList.Boolean:
                    Value.Add((T)(object)false);
                    Info.Add(info);
                    break;
                case Class.TypeUse.TypeList.DateTime:
                    Value.Add((T)(object)DateTime.Now);
                    Info.Add(info);
                    break;
                case Class.TypeUse.TypeList.Long:
                case Class.TypeUse.TypeList.Byte:
                case Class.TypeUse.TypeList.Double:
                case Class.TypeUse.TypeList.Float:
                case Class.TypeUse.TypeList.Int:
                case Class.TypeUse.TypeList.UShort:
                    Value.Add((T)(object)0);
                    Info.Add(info);
                    break;
                case Class.TypeUse.TypeList.String:
                    Value.Add((T)(object)"");
                    Info.Add(info);
                    break;
                case Class.TypeUse.TypeList.Bytes:
                    Value.Add((T)(object)null);
                    Info.Add(info);
                    break;
                case Class.TypeUse.TypeList.UnKnow:
                    All.Class.Error.Add("未知类型", string.Format("DataStyle.Add出现未知数据类型,数据名为:{0}", info));
                    break;
            }
        }
        /// <summary>
        /// 从字符串中,解析出下标,可以解析1,2,3,4这种以','号分隔,或者'数据名[0->9]'这种格式的数据名
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Dictionary<int,string> GetIndexFromSet(string index, string text)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            List<int> indexs = new List<int>();
            List<string> texts = new List<string>();

            string[] tmpBuff = index.Split(',');
            string[] tmp;

            if (tmpBuff == null || tmpBuff.Length == 0)
            {
                All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号列为空");
                All.Class.Error.Add("数据列值", index);
                return result;
            }
            int start = 0, end = 0;
            for (int i = 0; i < tmpBuff.Length; i++)
            {
                tmp = tmpBuff[i].Split(new string[] { "->", "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
                switch (tmp.Length)
                {
                    case 0:
                        break;
                    case 1:
                        indexs.Add(tmp[0].ToInt());
                        break;
                    case 2:
                        if(!All.Class.Check.isFix(tmp[0],Class.Check.RegularList.整数) ||
                            !All.Class.Check.isFix(tmp[1],Class.Check.RegularList.整数))
                        {
                            All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号中数值不能转化为整数");
                            All.Class.Error.Add("数据列值", index);
                        }
                        start = tmp[0].ToInt();
                        end = tmp[1].ToInt();
                        if (start > end)
                        {
                            All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号列起始值大于结果值");
                            All.Class.Error.Add("数据列值", index);
                            return result;
                        }
                        for (int j = start; j <= end; j++)
                        {
                            indexs.Add(j);
                        }
                        break;
                }
            }
            tmpBuff = text.Split(',');
            if (tmpBuff == null || tmpBuff.Length == 0)
            {
                All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中名称列为空");
                All.Class.Error.Add("数据名值", text);
                return result;
            }
            string[] temp;
            int[] tempIndex;
            for (int i = 0; i < tmpBuff.Length; i++)
            {
                tmp = tmpBuff[i].Split(new string[] { "->", "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
                if (tmp.Length == 0 || tmp.Length > 4)//最多为这种类型  ***[xx->xx]***
                {
                    All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet标志字符过多出现,只能有'->','['或']'");
                    All.Class.Error.Add("数据名值", text);
                    continue;
                }
                temp = new string[] { "", "", "", "" };
                tempIndex = new int[] { tmpBuff[i].IndexOf("->"), tmpBuff[i].IndexOf("["), tmpBuff[i].IndexOf("]") };
                if ((tempIndex[1] >= 0 && tempIndex[2] < 0) ||
                    (tempIndex[1] < 0 && tempIndex[2] >= 0))
                {
                    All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet单独出现了标志字符'['或']'");
                    All.Class.Error.Add("数据名值", text);
                    continue;
                }
                if ((tempIndex[1] >= 0 || tempIndex[2] >= 0) &&
                    (tempIndex[1] > tempIndex[0] ||
                    tempIndex[0] > tempIndex[2]))
                {
                    All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet出现错误标志符,必须为 ***[xx->xx]***这种");
                    All.Class.Error.Add("数据名值", text);
                    continue;
                }
                if (tempIndex[0] < 0)//没有->
                {
                    texts.Add(tmp[0]);
                    continue;
                }
                if (tempIndex[1] < 0 && tempIndex[2] < 0)//有->,没有[,]
                {
                    if (tmp.Length >= 3)
                    {
                        All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet出现不对等数据");
                        All.Class.Error.Add("数据名值", text);
                        continue;
                    }
                    temp[1] = tmp[0];//把数据放入temp格式中
                    temp[2] = tmp[1];
                }
                if (tempIndex[1] >= 0 && tempIndex[2] >= 0)//都有 '->','['和']'三个符号
                {
                    temp[0] = tmpBuff[i].Substring(0, tempIndex[1]);
                    temp[1] = tmpBuff[i].Substring(tempIndex[1] + 1, tempIndex[0] - tempIndex[1] - 1);
                    temp[2] = tmpBuff[i].Substring(tempIndex[0] + 2, tempIndex[2] - tempIndex[0] - 2);
                    temp[3] = tmpBuff[i].Substring(tempIndex[2] + 1);
                }
                if (!All.Class.Check.isFix(temp[1], Class.Check.RegularList.整数) ||
                    !All.Class.Check.isFix(temp[2], Class.Check.RegularList.整数))
                {
                    All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号中数值不能转化为整数");
                    All.Class.Error.Add("数据名值", text);
                    continue;
                }
                start = temp[1].ToInt();
                end = temp[2].ToInt();
                if (start > end)
                {
                    All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号列起始值大于结果值");
                    All.Class.Error.Add("数据名值", text);
                    continue;
                }
                for (int j = start; j <= end; j++)
                {
                    texts.Add(string.Format("{0}{1}{2}", temp[0], j, temp[3]));
                }
            }
            if (indexs.Count == texts.Count)
            {
                for (int i = 0; i < indexs.Count; i++)
                {
                    if (result.ContainsKey(i))
                    {
                        All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号列出现重复的数据列值");
                        All.Class.Error.Add("数据列值", index);
                        continue;
                    }
                    result.Add(indexs[i], texts[i]);
                }
            }
            else
            {
                All.Class.Error.Add("数据量错", "DataStyle.GetIndexFromSet中序号列和名称列数量不等");
                All.Class.Error.Add("数据列值", index);
                All.Class.Error.Add("数据名值", text);
            }
            return result;
        }
    }
}
