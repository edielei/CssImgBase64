using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CssImgBase64
{
    class Inc
    {
        #region 获取真实文件类型，即使修改文件后缀名依然可以正确获取
        /// <summary>
        /// 获取真实文件类型，即使修改文件后缀名依然可以正确获取
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>返回文件类型,数字形式</returns>
        public static string CheckTrueFileName(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            string bx = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                bx = buffer.ToString();
                buffer = r.ReadByte();
                bx += buffer.ToString();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            r.Close();
            fs.Close();
            return bx;
        }
        #endregion

        //判断是否符合要求的图片
        public static bool IsWebImg(string path)
        {
            bool yn;
            FileInfo fi = new FileInfo(path);
            yn = (CheckTrueFileName(path) == "255216"   //jpg
                || CheckTrueFileName(path) == "13780"   //png
                || CheckTrueFileName(path) == "7173"    //gif
                || CheckTrueFileName(path) == "6677")   //bmp
                && fi.Length <= 1024 * 100;  //少于或等于100KB
            return yn;
        }

        // 获取图像类型Base64编码使用
        public static string GetMimeType(Image i)
        {
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders())
            {
                if (codec.FormatID == i.RawFormat.Guid)
                    return codec.MimeType;
            }
            return "image/unknown";
        }

        // 把图像转换成CSS的Base64数据
        public static string ImageToBase64(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return "data:" + GetMimeType(image) + ";base64," + base64String;
            }
        }

        /// <summary>
        /// 图片转Base64，生成txt文件
        /// </summary>
        /// <param name="path">图片路径</param>
        public static void ImgToBase64(string path)
        {
            Image img = Image.FromFile(path);
            string lj = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + "-Base64.txt";
            File.WriteAllText(lj, Inc.ImageToBase64(img, img.RawFormat));
        }

        /// <summary>
        /// 获取Image对象的文件后缀名
        /// </summary>
        /// <param name="image">Image对象</param>
        /// <returns>返回如：png,jpg,gif</returns>
        public static string GetImageExtension(Image image)
        {
            if (image != null)
            {
                Type Type = typeof(ImageFormat);
                PropertyInfo[] imageFormatList = Type.GetProperties(BindingFlags.Static | BindingFlags.Public);
                for (int i = 0; i != imageFormatList.Length; i++)
                {
                    ImageFormat formatClass = (ImageFormat)imageFormatList[i].GetValue(null, null);
                    if (formatClass.Guid.Equals(image.RawFormat.Guid))
                    {
                        return imageFormatList[i].Name.ToLower().Replace("jpeg", "jpg");
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 把Base64数据还原成图片
        /// </summary>
        /// <param name="base64String">Base64编码</param>
        /// <returns></returns>
        public static Image Base64ToImage(string base64String)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                Image image = Image.FromStream(ms, true);
                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Base64转图片
        public static void Base64ToImg(string path, string j)
        {
            string str = File.ReadAllText(path).Trim();
            MatchCollection matches = Regex.Matches(str, @"data:(image\/.+)\;base64,(.+)[^\)'""]", RegexOptions.IgnoreCase);
            int i = 0;
            foreach (Match m in matches)
            {
                i++;
                m.ToString();
                string nr = Regex.Replace(m.Value, @"data:(image\/.+)\;base64,", string.Empty);
                Image img = Base64ToImage(nr);
                if (img != null)
                {
                    string lj = Path.GetDirectoryName(path) + "\\CssBase64ToImg";
                    Directory.CreateDirectory(lj);
                    img.Save(lj + "\\" + j + i.ToString() + "." + GetImageExtension(img), img.RawFormat);
                }
            }

        }

        //判断文件是CSS还是Image[命令行使用]
        public static string IsCssOrImg(string str)
        {
            //如果是CSS文件
            if ((Path.GetExtension(str) == ".css" || Path.GetExtension(str) == ".txt") && File.Exists(str))
            {
                return "css";
            }
            else if (IsWebImg(str))
            {
                return "img";
            }
            else
            {
                return null;
            }
        }

        //时间戳
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
