using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace System
{
    public static class WindowExtension
    {

        #region//Form
        /// <summary>
        /// 跨线程调用
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="t"></param>
        public static void CrossThreadDo(this Form frm, Action t)
        {
            if (t == null)
            {
                return;
            }
            if (frm.InvokeRequired)
            {
                frm.Invoke(new Action(() => t()));
            }
            else
            {
                t();
            }
        }
        #endregion
        #region//BestWindow
        /// <summary>
        /// 显示具有指定文本的消息框
        /// </summary>
        /// <param name="window"></param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="title">要在消息框的标题栏中显示的文本</param>
        /// <param name="button">System.Windows.Forms.MessageBoxButtons 值之一，可指定在消息框中显示哪些按钮。</param>
        /// <param name="icon">System.Windows.Forms.MessageBoxIcon 值之一，它指定在消息框中显示哪个图标。</param>
        /// <returns>System.Windows.Forms.DialogResult 值之一。</returns>
        public static DialogResult Show(this All.Window.BestWindow window, string text, string title, MessageBoxButtons button, MessageBoxIcon icon)
        {
            return All.Window.MessageWindow.Show(text, title, button, icon);
        }
        /// <summary>
        /// 显示具有指定文本的消息框
        /// </summary>
        /// <param name="window"></param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="title">要在消息框的标题栏中显示的文本</param>
        /// <param name="button">System.Windows.Forms.MessageBoxButtons 值之一，可指定在消息框中显示哪些按钮。</param>
        /// <param name="icon">System.Windows.Forms.MessageBoxIcon 值之一，它指定在消息框中显示哪个图标。</param>
        /// <returns>System.Windows.Forms.DialogResult 值之一。</returns>
        public static DialogResult Show(this All.Window.BestWindow window, string text, string title, MessageBoxButtons button)
        {
            return All.Window.MessageWindow.Show(text, title, button);
        }
        /// <summary>
        /// 显示具有指定文本的消息框
        /// </summary>
        /// <param name="window"></param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="title">要在消息框的标题栏中显示的文本</param>
        /// <param name="button">System.Windows.Forms.MessageBoxButtons 值之一，可指定在消息框中显示哪些按钮。</param>
        /// <param name="icon">System.Windows.Forms.MessageBoxIcon 值之一，它指定在消息框中显示哪个图标。</param>
        /// <returns>System.Windows.Forms.DialogResult 值之一。</returns>
        public static DialogResult Show(this All.Window.BestWindow window, string text, string title)
        {
            return All.Window.MessageWindow.Show(text, title);
        }
        /// <summary>
        /// 显示具有指定文本的消息框
        /// </summary>
        /// <param name="window"></param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="title">要在消息框的标题栏中显示的文本</param>
        /// <param name="button">System.Windows.Forms.MessageBoxButtons 值之一，可指定在消息框中显示哪些按钮。</param>
        /// <param name="icon">System.Windows.Forms.MessageBoxIcon 值之一，它指定在消息框中显示哪个图标。</param>
        /// <returns>System.Windows.Forms.DialogResult 值之一。</returns>
        public static DialogResult Show(this All.Window.BestWindow window, string text)
        {
            return All.Window.MessageWindow.Show(text);
        }
        #endregion
        #region//BaseWindow
        /// <summary>
        /// 显示具有指定文本的消息框
        /// </summary>
        /// <param name="window"></param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="title">要在消息框的标题栏中显示的文本</param>
        /// <param name="button">System.Windows.Forms.MessageBoxButtons 值之一，可指定在消息框中显示哪些按钮。</param>
        /// <param name="icon">System.Windows.Forms.MessageBoxIcon 值之一，它指定在消息框中显示哪个图标。</param>
        /// <returns>System.Windows.Forms.DialogResult 值之一。</returns>
        public static DialogResult Show(this All.Window.BaseWindow window, string text, string title, MessageBoxButtons button, MessageBoxIcon icon)
        {
            return All.Window.MessageWindow.Show(text, title, button, icon);
        }
        /// <summary>
        /// 显示具有指定文本的消息框
        /// </summary>
        /// <param name="window"></param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="title">要在消息框的标题栏中显示的文本</param>
        /// <param name="button">System.Windows.Forms.MessageBoxButtons 值之一，可指定在消息框中显示哪些按钮。</param>
        /// <param name="icon">System.Windows.Forms.MessageBoxIcon 值之一，它指定在消息框中显示哪个图标。</param>
        /// <returns>System.Windows.Forms.DialogResult 值之一。</returns>
        public static DialogResult Show(this All.Window.BaseWindow window, string text, string title, MessageBoxButtons button)
        {
            return All.Window.MessageWindow.Show(text, title, button);
        }
        /// <summary>
        /// 显示具有指定文本的消息框
        /// </summary>
        /// <param name="window"></param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="title">要在消息框的标题栏中显示的文本</param>
        /// <param name="button">System.Windows.Forms.MessageBoxButtons 值之一，可指定在消息框中显示哪些按钮。</param>
        /// <param name="icon">System.Windows.Forms.MessageBoxIcon 值之一，它指定在消息框中显示哪个图标。</param>
        /// <returns>System.Windows.Forms.DialogResult 值之一。</returns>
        public static DialogResult Show(this All.Window.BaseWindow window, string text, string title)
        {
            return All.Window.MessageWindow.Show(text, title);
        }
        /// <summary>
        /// 显示具有指定文本的消息框
        /// </summary>
        /// <param name="window"></param>
        /// <param name="text">要在消息框中显示的文本</param>
        /// <param name="title">要在消息框的标题栏中显示的文本</param>
        /// <param name="button">System.Windows.Forms.MessageBoxButtons 值之一，可指定在消息框中显示哪些按钮。</param>
        /// <param name="icon">System.Windows.Forms.MessageBoxIcon 值之一，它指定在消息框中显示哪个图标。</param>
        /// <returns>System.Windows.Forms.DialogResult 值之一。</returns>
        public static DialogResult Show(this All.Window.BaseWindow window, string text)
        {
            return All.Window.MessageWindow.Show(text);
        }
        #endregion
    }
}
