using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsRegister
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //int res = TimeClass.InitRegedit();
            //if (res == 0)
            //{
            //    Application.Run(new Form1());
            //}
            //else if (res == 1)
            //{
            //    MessageBox.Show("软件尚未注册，请注册软件！");
            //}
            //else if (res == 2)
            //{
            //    MessageBox.Show("注册机器与本机不一致,请联系管理员！");
            //}
            //else if (res == 3)
            //{
            //    MessageBox.Show("软件试用已到期！");
            //}
            //else
            //{
            //    MessageBox.Show("软件运行出错，请重新启动！");
            //}
           
        }
    }
}
