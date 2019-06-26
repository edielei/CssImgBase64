using System;
using System.Windows.Forms;

namespace CssImgBase64
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                string a = Inc.IsCssOrImg(args[0].ToString()).ToString();
                switch (a)
                {
                    case "img":
                        Inc.ImgToBase64(args[0].ToString());
                        break;
                    case "css":
                        Inc.Base64ToImg(args[0].ToString(), Inc.GenerateTimeStamp());
                        break;
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}
