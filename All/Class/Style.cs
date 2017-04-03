using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Class
{
    public static class Style
    {
        /// <summary>
        /// 背景色
        /// </summary>
        public enum BackColors : uint
        {
            /// <summary>
            /// 白
            /// </summary>
            White = 0xFFF0F0F0,
            /// <summary>
            /// 黑
            /// </summary>
            Black = 0xFF000000
        }
        /// <summary>
        /// 前景色
        /// </summary>
        public enum FrontColors : uint
        {
            /// <summary>
            /// 玫红
            /// </summary>
            Magenta = 0xFFFF0097,
            /// <summary>
            /// 紫
            /// </summary>
            Purple = 0xFF6E155F,
            /// <summary>
            /// 茶绿色
            /// </summary>
            Teal = 0xFF4EB3B9,
            /// <summary>
            /// 柠檬绿
            /// </summary>
            Lime = 0xFF96B232,
            /// <summary>
            /// 棕色
            /// </summary>
            Brown = 0xFFA05000,
            /// <summary>
            /// 粉色
            /// </summary>
            Pink = 0xFFE671BB,
            /// <summary>
            /// 桔红色
            /// </summary>
            Orange = 0xFFF09609,
            /// <summary>
            /// 蓝色
            /// </summary>
            Blue = 0xFF1BA1E2,
            /// <summary>
            /// 红色
            /// </summary>
            Red = 0xFFE51400,
            /// <summary>
            /// 绿色
            /// </summary>
            Green = 0xFF339933,
            /// <summary>
            /// 海蓝色
            /// </summary>
            Marine = 0xFF034888
        }
        static List<ChangeTheme> allStyle = new List<ChangeTheme>();
        /// <summary>
        /// 所有可以改变主题的接口继承项
        /// </summary>
        public static List<ChangeTheme> AllStyle
        {
            get { return allStyle; }
            set { allStyle = value; }
        }
        #region//主体颜色
        static BackColors back = 0;
        /// <summary>
        /// 背景色
        /// </summary>
        public static BackColors Back
        {
            get
            {
                if (back == 0)
                {
                    back = GetBackColor();
                    SetBackColor();
                }
                return back;
            }
            set
            {
                if (back != value)
                {
                    back = value;
                    SetBackColor();
                }
            }
        }
        /// <summary>
        ///背景颜色
        /// </summary>
        public static System.Drawing.Color BackColor
        {
            get
            {
                uint tmp = (uint)Back;
                return System.Drawing.Color.FromArgb((int)tmp);
            }
        }
        static FrontColors board = 0;
        /// <summary>
        /// 前景色
        /// </summary>
        public static FrontColors Board
        {
            get
            {
                if (board == 0)
                {
                    board = GetFrontColor();
                    SetFrontColor();
                }
                return board;
            }
            set
            {
                if (board != value)
                {
                    board = value;
                    SetFrontColor();
                }
            }
        }
        /// <summary>
        ///前景颜色
        /// </summary>
        public static System.Drawing.Color BoardColor
        {
            get
            {
                uint tmp = (uint)Board;
                return System.Drawing.Color.FromArgb((int)tmp);
            }
        }
        #endregion
        #region//被动生成颜色 
        /// <summary>
        /// 字体色
        /// </summary>
        public static System.Drawing.Color FontColor
        {
            get
            {
                uint tmp = (uint)Back;
                tmp = (tmp ^ 0xFFFFFF);
                return System.Drawing.Color.FromArgb((int)tmp);
            }
        }
        /// <summary>
        /// 标题颜色 
        /// </summary>
        public static System.Drawing.Color TitleColor
        {
            get 
            {
                switch (All.Class.Style.Back)
                {
                    case Class.Style.BackColors.Black:
                        return System.Windows.Forms.ControlPaint.LightLight(BoardColor);
                    case BackColors.White:
                        return System.Windows.Forms.ControlPaint.LightLight(System.Windows.Forms.ControlPaint.Light(BoardColor));
                }
                return System.Drawing.Color.Silver;
            }
        }
        /// <summary>
        /// 控件不可用颜色
        /// </summary>
        public static System.Drawing.Color UEnableColor
        {
            get
            {
                switch (All.Class.Style.Back)
                {
                    case BackColors.Black:
                        return System.Drawing.Color.FromArgb(100, 100, 100);
                    case BackColors.White:
                        return System.Drawing.Color.LightGray;
                }
                return System.Drawing.Color.Silver;
            }
        }
        #endregion
        #region//颜色改变
        /// <summary>
        /// 改变背景色
        /// </summary>
        /// <param name="backColor"></param>
        public static void ChangeBack(BackColors backColor)
        {
            
            Back = backColor;
            allStyle.ForEach(
                all =>
                {
                    all.ChangeBack(backColor);
                });
        }
        /// <summary>
        /// 改变前景色
        /// </summary>
        /// <param name="frontColor"></param>
        public static void ChangeFront(FrontColors frontColor)
        {
            Board = frontColor;
            allStyle.ForEach(
                all =>
                {
                    all.ChangeFront(frontColor);
                });
        }
        #endregion
        #region//从注册表取保存数据
        /// <summary>
        /// 从注册表取上次保存值
        /// </summary>
        /// <returns></returns>
        static BackColors GetBackColor()
        {
            BackColors result = BackColors.White;
            try
            {
                result = (BackColors)Enum.Parse(typeof(BackColors), Region.ReadValue("StyleBackColor"), true);
            }
            catch
            { }
            return result;
        }
        /// <summary>
        /// 保存当前设置值
        /// </summary>
        static void SetBackColor()
        {
            Region.WriteValue("StyleBackColor", back.ToString());
        }

        /// <summary>
        /// 从注册表取上次保存值
        /// </summary>
        /// <returns></returns>
        static FrontColors GetFrontColor()
        {
            FrontColors result = FrontColors.Magenta;
            try
            {
                result = (FrontColors)Enum.Parse(typeof(FrontColors), Region.ReadValue("StyleFrontColor"), true);
            }
            catch
            { }
            return result;
        }
        /// <summary>
        /// 保存当前设置值 
        /// </summary>
        static void SetFrontColor()
        {
            Region.WriteValue("StyleFrontColor", board.ToString());
        }
#endregion
        #region//接口
        /// <summary>
        /// 主题接口
        /// </summary>
        public interface ChangeTheme
        {
            /// <summary>
            /// 改变背景色
            /// </summary>
            /// <param name="backColor"></param>
            void ChangeBack(BackColors backColor);
            /// <summary>
            /// 改变前景色
            /// </summary>
            /// <param name="frontColor"></param>
            void ChangeFront(FrontColors frontColor);
        }
        #endregion
    }
}
