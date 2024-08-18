using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoStudyXFK
{

    public struct User
    {
        public String Name;
        public String ID;
        public String PhoneNum;
        public String PassWord;
        public String Cookie;
        public String Score;
        public float TodayScore;
        public int TodayAnswer;
    }

 

    public static class Global
    {
        public static bool OnlyQA = true;
        public static String LoginCookie = null;
        public static int ReadJsonCurrtenNum = 0;
        public static List<User> UserList = new List<User>();

        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>加密后字符串</returns>
        public static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 平台打码
        /// </summary>
        /// <param name="ImgBase64">图片的base64编码</param>
        /// <returns></returns>
        public static String DaMaGou(String ImgBase64)
        {
            //这里填入你自己的UserKey  
            String UserKey = "";
            var url = @"http://api.damagou.top/apiv1/recognize.html";
            ImgBase64 = "data:image/jpg;base64," + ImgBase64;
            ImgBase64 = NutWeb.UrlEncode(ImgBase64);
            var data = "image=" + ImgBase64 + "&userkey="+ UserKey + "&type=1002";
            var postResult = NutWeb.Nut_Post(url, data, null, null);
            return postResult.Html;
        }

        /// <summary>
        /// 本地验证码识别
        /// </summary>
        /// <param name="ImgBase64"></param>
        /// <returns></returns>
        public static string StupidOCR(String ImgBase64)
        {
            var url = @"http://127.0.0.1:6688/identify_GeneralCAPTCHA";
            //ImgBase64 = NutWeb.UrlEncode(ImgBase64);
            var data = "{\"ImageBase64\": \"" + ImgBase64 + "\"}";
            var postResult = NutWeb.Nut_Post(url, data, null, null, "application/json",null,1000); 
            if (postResult.StatusDescription == "无法连接到远程服务器"|| postResult.StatusDescription == "操作超时")
            {
                return "失败";
            }
            var Result = Global.TextGainCenter("result\":\"", "\",\"BiliBili", postResult.Html);
            return Result;
        }

        public static string PicCovert(Image img)
        {


            using (var b = new Bitmap(img.Width, img.Height))
            {
                b.SetResolution(img.HorizontalResolution, img.VerticalResolution);

                using (var g = Graphics.FromImage(b))
                {
                    g.Clear(Color.White);
                    g.DrawImageUnscaled(img, 0, 0);
                }

                // Now save b as a JPEG like you normally would
                b.Save("captcha.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                return ToBase64(b);
            }
        }

        public static string ToBase64(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);
                return strbaser64;
            }
            catch (Exception ex)
            {

                return "";
            }
        }


        /// <summary>
        /// 取文本中间字符串
        /// </summary>
        /// <param name="left">左边的字符串</param>
        /// <param name="right">右边的字符串</param>
        /// <param name="text">字符串整体</param>
        /// <returns></returns>
        public static string TextGainCenter(string left, string right, string text)
        {
            //判断是否为null或者是empty
            if (string.IsNullOrEmpty(left))
                return "";
            if (string.IsNullOrEmpty(right))
                return "";
            if (string.IsNullOrEmpty(text))
                return "";
            //判断是否为null或者是empty

            int Lindex = text.IndexOf(left); //搜索left的位置

            if (Lindex == -1)
            { //判断是否找到left
                return "";
            }
            Lindex = Lindex + left.Length; //取出left右边文本起始位置

            int Rindex = text.IndexOf(right, Lindex);//从left的右边开始寻找right

            if (Rindex == -1)
            {//判断是否找到right
                return "";
            }
            return text.Substring(Lindex, Rindex - Lindex);//返回查找到的文本
        }

        public static string SubStringToEnd(string origin, string start)
        {
            origin = origin.Substring(origin.IndexOf(start) + start.Length, origin.Length - origin.IndexOf(start) - start.Length);
            return origin;
        }
 
 
    }
}
