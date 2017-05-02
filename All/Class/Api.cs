using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace All.Class
{
    /// <summary>
    /// API函数
    /// </summary>
    public static class Api
    {
        public static readonly IntPtr True = new IntPtr(1);
        public static readonly IntPtr False = IntPtr.Zero;
        /// <summary>
        /// 按下键盘
        /// </summary>
        public const uint WM_KEYDOWN = 0x100;
        /// <summary>
        /// 键盘弹起
        /// </summary>
        public const uint WM_KEYUP = 0x101;
        /// <summary>
        /// 鼠标激活
        /// </summary>
        public const uint WM_MouseActivate = 0x21;
        /// <summary>
        /// 鼠标离开
        /// </summary>
        public const int WM_MOUSELEAVE = 0x2A3;
        /// <summary>
        /// 鼠标移动
        /// </summary>
        public const int WM_MOUSEMove = 0x200;

        /// <summary>
        /// 不激活
        /// </summary>
        public const uint MA_NoActivate = 3;
        public const uint WA_InActive = 0;
        public const uint WM_NcActivate = 0x86;
        /// <summary>
        /// 内存拷贝
        /// </summary>
        public const int ROP_SrcCopy = 0xCC0020;
        //绘画
        public const int WM_PAINT = 0x0F;
        public const int WM_CTLCOLOREDIT = 0x133;
        /// <summary>
        /// 将指定句柄窗体设为激活窗体
        /// </summary>
        /// <param name="handle">窗体句柄</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr handle);
        /// <summary>
        /// 图像直接从源内存拷贝到显示控件内存,矩形对应矩形,即图像可进行大小缩放
        /// </summary>
        /// <param name="hdcDest">目标设备句柄,即显示控件句柄</param>
        /// <param name="nXDest">图像显示的左上角X坐标</param>
        /// <param name="nYDest">图像显示的左上角Y坐标</param>
        /// <param name="nWidth">图像显示的矩形宽度</param>
        /// <param name="nHeight">图像显示的矩形长度</param>
        /// <param name="hdcSrc">源设备句柄,即源图像句柄</param>
        /// <param name="nXSrc">源图像的选取的左上角X坐标</param>
        /// <param name="nYSrc">源图像的选取的左上角Y坐标</param>
        /// <param name="nWidthSrc">源对象的宽度</param>
        /// <param name="nHeightSrc">源对象的长度</param>
        /// <param name="dwRop">光栅的操作值</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern bool StretchBlt
        (
            IntPtr hdcDest, //目标设备的句柄 
            int nXDest, // 目标对象的左上角的X坐标 
            int nYDest, // 目标对象的左上角的Y坐标 
            int nWidth, // 目标对象的矩形的宽度 
            int nHeight, // 目标对象的矩形的长度 
            IntPtr hdcSrc, // 源设备的句柄 
            int nXSrc, // 源对象的左上角的X坐标 
            int nYSrc, // 源对象的左上角的Y坐标 
            int nWidthSrc,//源对象的宽度
            int nHeightSrc,//源对象的高度
            int dwRop // 光栅的操作值 
            );
        /// <summary>
        /// 图像直接从源内存拷贝到显示控件内存,矩形对应点,图像直接原大小拷贝,不能缩放
        /// </summary>
        /// <param name="hdcDest">目标设备句柄,即显示控件句柄</param>
        /// <param name="nXDest">图像显示的左上角X坐标</param>
        /// <param name="nYDest">图像显示的左上角Y坐标</param>
        /// <param name="nWidth">图像显示的矩形宽度</param>
        /// <param name="nHeight">图像显示的矩形长度</param>
        /// <param name="hdcSrc">源设备句柄,即源图像句柄</param>
        /// <param name="nXSrc">源图像的选取的左上角X坐标</param>
        /// <param name="nYSrc">源图像的选取的左上角Y坐标</param>
        /// <param name="dwRop">光栅的操作值</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(
            IntPtr hdcDest, //目标设备的句柄 
            int nXDest, // 目标对象的左上角的X坐标 
            int nYDest, // 目标对象的左上角的Y坐标 
            int nWidth, // 目标对象的矩形的宽度 
            int nHeight, // 目标对象的矩形的长度 
            IntPtr hdcSrc, // 源设备的句柄 
            int nXSrc, // 源对象的左上角的X坐标 
            int nYSrc, // 源对象的左上角的Y坐标 
            int dwRop // 光栅的操作值 
            );
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        /// <summary>
        /// 将图像源指针放入到当前绘画内存
        /// </summary>
        /// <param name="hDC"></param>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        /// <summary>
        /// 将当前绘画内存中的图像源指定换回原来的指针
        /// </summary>
        /// <param name="hObj"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr DeleteObject(IntPtr hObject);

        /// <summary>
        /// 注销
        /// </summary>
        public const int LogOff = 0;
        /// <summary>
        /// 重启
        /// </summary>
        public const int Reboot = 2;
        /// <summary>
        /// 关机常量
        /// </summary>
        public const int ShutDown = 1;
        /// <summary>
        /// 关机，重启等命令
        /// </summary>
        /// <param name="uFlags"></param>
        /// <param name="dwReserved"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern long ExitWindowsEx(long uFlags, Int32 dwReserved);

        /// <summary>
        /// 将数据写入配置文件
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string lpAppName, string lpKeyName, string val, string lpFileName);
        /// <summary>
        /// 从配置文件读取数据
        /// </summary>
        /// <param name="lpAppName"></param>
        /// <param name="lpKeyName"></param>
        /// <param name="lpDefault"></param>
        /// <param name="lpReturnedString"></param>
        /// <param name="nSize"></param>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        #region//绘画开始与结束
        /// <summary>
        /// 绘画结构体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public int fErase;
            public RECT rcPaint;
            public int fRestore;
            public int fIncUpdate;
            public int Reserved1;
            public int Reserved2;
            public int Reserved3;
            public int Reserved4;
            public int Reserved5;
            public int Reserved6;
            public int Reserved7;
            public int Reserved8;
        }
        /// <summary>
        /// Win32中的矩形结构，及与.net中的Rectangle的处理
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public RECT(Rectangle rect)
            {
                this.left = rect.Left;
                this.right = rect.Right;
                this.top = rect.Top;
                this.bottom = rect.Bottom;
            }

            public Rectangle Rect
            {
                get
                {
                    return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top);
                }
            }

            public static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x, y, x + width, y + height);
            }

            public static RECT FromRectangle(Rectangle rect)
            {
                return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }
        }
        /// <summary>
        /// 开始绘画
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);
        /// <summary>
        /// 结束绘画
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);
        #endregion
    }
}
