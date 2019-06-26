using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace CssImgBase64
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            this.TopMost = this.checkBox1.Checked;
        }

        private void label1_DragDrop(object sender, DragEventArgs e)
        {
            this.Activate();
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            ArrayList imgpaths = new ArrayList();
            foreach (string fpath in s)
            {
                if (Inc.IsWebImg(fpath))
                {
                    imgpaths.Add(fpath);
                }
            }
            if (imgpaths.Count != 0)
            {
                foreach (string item in imgpaths)
                {
                    Inc.ImgToBase64(item);
                }
                MessageBox.Show("转换完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None;
        }

        private void label2_DragDrop(object sender, DragEventArgs e)
        {
            this.Activate();
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            ArrayList csspaths = new ArrayList();
            foreach (string csslj in s)
            {
                if (Path.GetExtension(csslj) == ".css" || Path.GetExtension(csslj) == ".txt")
                {
                    csspaths.Add(csslj);
                }
            }
            if (csspaths.Count != 0)
            {
                int j = 0;
                foreach (string item in csspaths)
                {
                    j++;
                    Inc.Base64ToImg(item, j.ToString());
                }
                MessageBox.Show("转换完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("暂支持.png|.jpg|.gif|.bmp格式。文件大小限制为小于或等于100KB\n1.Base64数据保存到对应的图片路径中。【原图片名称-Base64.txt】\n2.还原的图片保存在对应的CSS同级目录下的【CssBase64ToImg】文件夹里\n\n命令行使用方法（请使用绝对路径）：\nCssImgBase64 E:\\1.png 或 CssImgBase64 \"E:\\1.png\"\nCssImgBase64 E:\\1.css 或 CssImgBase64 \"E:\\1.css\"\n[注意]命令行只支持单个文件，路径建议使用双引号(空格、特殊字符...)\n[提示]可开发插件调用此工具，也可为css或图片文件设置右键菜单执行", "使用说明", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
