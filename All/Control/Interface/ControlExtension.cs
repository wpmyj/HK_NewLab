using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public static class ControlExtension
    {
        /// <summary>
        /// 跨线程调用
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="t"></param>
        public static void CrossThreadDo(this System.Windows.Forms.Control sender, Action t)
        {
            if (t == null)
            {
                return;
            }
            if (sender.InvokeRequired)
            {
                sender.Invoke(new Action(() => t()));
            }
            else
            {
                t();
            }
        }
        /// <summary>
        /// 跨线程设置控件文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        public static void SetText(this System.Windows.Forms.Control sender, string value)
        {
            if (sender.InvokeRequired)
            {
                sender.Invoke(new Action<System.Windows.Forms.Control, string>(SetText), sender, value);
            }
            else
            {
                sender.Text = value;
            }
        }
    }
}
