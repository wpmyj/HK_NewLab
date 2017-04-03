using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Communicate.Base
{
    public class Api232
    {
        /// <summary>
        /// 缓冲区数据
        /// </summary>
        public int DataRecive
        {
            get
            {
                return 0;
            }
        }

        bool isOpen = false;
        /// <summary>
        /// 端口是否已经打开
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
        }

    }
}
