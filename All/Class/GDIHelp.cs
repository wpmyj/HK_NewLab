using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace All.Class
{
    public class GDIHelp
    {
        /// <summary>
        /// 初始化为高质量绘画,主要用于画直线
        /// </summary>
        /// <param name="g"></param>
        public static void Init(System.Drawing.Graphics g)
        {
            if (g != null)
            {
                //g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
            }
        }
        /// <summary>
        /// 指定消除锯齿,主要用于画斜线等
        /// </summary>
        /// <param name="g"></param>
        public static void InitAntiAlias(Graphics g)
        {
            if (g != null)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
            }
        }
        /// <summary>
        /// 获取居中位置的设置
        /// </summary>
        /// <returns></returns>
        public static StringFormat StringFormat(ContentAlignment contentAlignment)
        {
            StringFormat result = new StringFormat();
            switch (contentAlignment)
            {
                case ContentAlignment.TopLeft:
                    result.Alignment = StringAlignment.Near;
                    result.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                    result.Alignment = StringAlignment.Near;
                    result.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    result.Alignment = StringAlignment.Near;
                    result.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.TopCenter:
                    result.Alignment = StringAlignment.Center;
                    result.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleCenter:
                    result.Alignment = StringAlignment.Center;
                    result.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomCenter:
                    result.Alignment = StringAlignment.Center;
                    result.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.TopRight:
                    result.Alignment = StringAlignment.Far;
                    result.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleRight:
                    result.Alignment = StringAlignment.Far;
                    result.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomRight:
                    result.Alignment = StringAlignment.Far;
                    result.LineAlignment = StringAlignment.Far;
                    break;
            }
            return result;
        }
    }
}
