using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Diagnostics;
using System.Net;
using HttpCodeLib;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web;

namespace AutoStudyXFK
{
    class WebAPI
    {
        public static User GetInfo(User user)
        {
            var url = "http://www.scxfks.com/study/index";
            var getResult = NutWeb.Nut_Get(url, null, user.Cookie);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(getResult.Html);
            var infoNode = doc.DocumentNode.SelectSingleNode("//div[@class='card userbox']");

            if (infoNode == null)
            {
                Form1.MainForm.NutDebug("GetInfo() Cookie失效");
                user.Cookie = null;
                return user;
            }

            user.Name = Global.TextGainCenter("姓名：", "\n                    学员ID", infoNode.InnerText);
            user.ID = Global.TextGainCenter("ID：", "\n                    学分", infoNode.InnerText);
            //var scoreNode = doc.DocumentNode.SelectSingleNode("//div[@class='card schedule pull-left ksico']/div[1]");
            user.Score = Global.TextGainCenter("学分累计：", "学分\n                                            测评", infoNode.InnerText);
            user.TodayScore = GetTodayScore(user.Cookie);
            user.TodayAnswer = GetTodayAnswer(user.Cookie);
            return user;
        }

        public static String AutoLogin(String ID, String PassWord)
        {
            HttpResults r;
            while (true)
            {
                Form1.MainForm.NutDebug("开始尝试登录账号:" + ID + "  密码：" + PassWord);

                Global.LoginCookie = NutWeb.Nut_Get("http://www.scxfks.com/study/login", null).Cookie;
                var img = NutWeb.Nut_GetImage("http://www.scxfks.com/study/captcha", Global.LoginCookie);

                //方法1
                var img_t = new HttpHelpers().GetImg(img);
                var img_str = Global.PicCovert(img_t);

                //打码狗
                //var Result = Global.DaMaGou(img_str);


                //StupidOCR
                var Result = Global.StupidOCR(img_str);
                if (Result.Contains("失败"))
                {
                    Form1.MainForm.NutDebug("StupidOCR链接失败，请检测服务是否开启！或是否在6688端口上运行。");
                    return null;
                }

                Form1.MainForm.NutDebug("验证码识别结果：" + Result);


                var t_PassWord = Global.GenerateMD5(PassWord + "gw-gd-exam").ToUpper();
                var url = @"http://www.scxfks.com/study/session";
                var data = "{\"mobile\":\"" + ID + "\",\"password\":\"" + t_PassWord + "\",\"captcha\":\"" + Result + "\"}";
                r = NutWeb.Nut_Post(url, data, Global.LoginCookie, null);
                Form1.MainForm.NutDebug(r.Html);

                if (!r.Html.Contains("验证码错误"))
                {
                    break;
                }
                Form1.MainForm.NutDebug("验证码错误,再次尝试");
            }
            if (r.Html.Contains("登录成功"))
            {
                Form1.MainForm.NutDebug("登录成功");
                return r.Cookie;
            }
            return null;
        }

        public static float GetTodayScore(String Cookie)
        {
            var url = "http://www.scxfks.com/study/profile/learnLogs";
            var getResult = NutWeb.Nut_Get(url, null, Cookie);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(getResult.Html);
            var infoNodes = doc.DocumentNode.SelectNodes("//div[@class='card']//li");
            if (infoNodes == null)
            {
                Form1.MainForm.NutDebug("GetTodayScore() Cookie失效");
                return 0;
            }
            float val = 0.0f;
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            foreach (var node in infoNodes)
            {

                var data = Global.TextGainCenter(" • ", "\n", node.InnerText);
                if (data == today)
                {

                    var score = Global.TextGainCenter("[", "分]", node.InnerText);
                    val += float.Parse(score);

                }

            }
            Form1.MainForm.NutDebug("今日得分:" + val.ToString());
            return val;

        }

        public static int GetTodayAnswer(String Cookie)
        {
            var url = "http://www.scxfks.com/study/profile/activityScores";
            var getResult = NutWeb.Nut_Get(url, null, Cookie);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(getResult.Html);
            var infoNodes = doc.DocumentNode.SelectNodes("//div[@class='card']//li");

            if (infoNodes == null)
            {
                Form1.MainForm.NutDebug("GetTodayAnswer() Cookie失效?或者获取记录失败？如正常运行则不用搭理");
                return -1;
            }
            int val = 0;
            bool IsDone = false;
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            foreach (var node in infoNodes)
            {

                var data = Global.TextGainCenter(" • ", "\r\n", node.InnerText);
                if (data == today)
                {
                    IsDone = true;
                    var score = Global.TextGainCenter("[", "分]", node.InnerText);
                    val += int.Parse(score);

                }

            }
            Form1.MainForm.NutDebug("今日答题竞赛完成情况:" + IsDone);
            if (!IsDone) return -1;
            Form1.MainForm.NutDebug("今日竞赛得分:" + val.ToString());
            return val;
        }

        public static string Login(String ID, String PassWord, string captcha)
        {
            PassWord = Global.GenerateMD5(PassWord + "gw-gd-exam").ToUpper();
            var url = @"http://www.scxfks.com/study/session";
            var data = "{\"mobile\":\"" + ID + "\",\"password\":\"" + PassWord + "\",\"captcha\":\"" + captcha + "\"}";
            var postresulet = NutWeb.Nut_Post(url, data, Global.LoginCookie, null);
            Form1.MainForm.NutDebug(postresulet.Html);
            return postresulet.Html;
        }

        public static void StartStudy(String Cookie)
        {
            //List<string> courseId = new List<string>();
            var url = "http://www.scxfks.com/study/courses/year";
            var getResult = NutWeb.Nut_Get(url, null, Cookie);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(getResult.Html);

            var AllNode = doc.DocumentNode.SelectNodes("//ul[@class='film_focus_nav']//li");
            if (AllNode == null)
            {
                Form1.MainForm.NutDebug("StartStudy() Cookie失效");
                return;
            }

            foreach (var nav in AllNode)
            {
                var cl = nav.GetAttributeValue("cl", "");
                var atb = nav.GetAttributeValue("atb", "");
                //Form1.MainForm.NutDebug(cl+"  "+atb);

                var t_url = @"http://www.scxfks.com/study/selectCourse/" + cl + "/" + atb + "/51";
                var Result = NutWeb.Nut_Get(t_url, null, Cookie);
                var JsonObj = (JArray)JsonConvert.DeserializeObject(Result.Html);


                if (Global.OnlyQA && cl != "x_z_lxtk") continue;

                foreach (var j in JsonObj)
                {
                    JObject T_J = (JObject)JsonConvert.DeserializeObject(j.ToString());
                    if (T_J["learnRate"] == null || T_J["learnRate"].ToString() != "100")
                    {
                        //进入课程
                        var courseId = T_J["courseId"].ToString();
                        var c_url = @"http://www.scxfks.com/study/course/" + courseId;
                        var c_getResult = NutWeb.Nut_Get(c_url, null, Cookie);
                        HtmlDocument c_doc = new HtmlDocument();
                        c_doc.LoadHtml(c_getResult.Html);

                        var ChapterNodes = c_doc.DocumentNode.SelectNodes("//td[@class='title']");
                        foreach (var c_node in ChapterNodes)
                        {
                            if (!c_node.InnerText.Contains("学分") && !c_node.InnerText.Contains("待后续开放"))
                            {
                                var t_doc = new HtmlDocument();
                                t_doc.LoadHtml(c_node.InnerHtml);
                                var herf = t_doc.DocumentNode.SelectSingleNode("//a");
                                var Link = herf.GetAttributeValue("href", null);
                                //Form1.MainForm.NutDebug(Link);

                                String s_url = "";
                                if (Link == "javascript:void(0);")//练习题
                                {

                                    var id_att = herf.GetAttributeValue("onclick", "");
                                    var id = Global.TextGainCenter(",", ");", id_att);
                                    s_url = "http://www.scxfks.com/study/exercise/" + id;
                                    //生成题库
                                    NutWeb.Nut_Post("http://www.scxfks.com/study/exerciseBuild/" + courseId + "/chapter/" + id, "", Cookie, null);

                                }
                                else
                                {
                                    s_url = "http://www.scxfks.com" + Link;
                                }

                                Form1.MainForm.NutDebug(s_url);

                                //两种情况 阅读 和 答题
                                if (s_url.Contains("chapter"))
                                {
                                    //阅读
                                    var c_id = Global.SubStringToEnd(s_url, "chapter/");
                                    var getr = NutWeb.Nut_Get(s_url, null, Cookie);
                                    if (getr.Html.Contains("您已到达今日上限"))
                                    {
                                        Form1.MainForm.NutDebug("学习完毕");
                                        return;
                                    }
                                    var p_url = "http://www.scxfks.com/study/learnlog/" + c_id + "/" + cl;
                                    var p_r = NutWeb.Nut_Post(p_url, "", Cookie, null);
                                    Form1.MainForm.NutDebug(p_r.Html);
                                }
                                else if (s_url.Contains("exercise"))
                                {

                                    var c_id = Global.SubStringToEnd(s_url, "exercise/");
                                    var getr = NutWeb.Nut_Get(s_url, null, Cookie);
                                    var e_doc = new HtmlDocument();
                                    e_doc.LoadHtml(getr.Html);

                                    var Answer_node = e_doc.DocumentNode.SelectNodes("//div[@class='item']");
                                    if (Answer_node == null)
                                    {
                                        continue;
                                    }

                                    String P_Data = "";
                                    foreach (var Answer in Answer_node)
                                    {
                                        var qid = Answer.GetAttributeValue("qid", "");
                                        var ans = Global.TextGainCenter("正确答案：", "\n            \n", Answer.InnerText);

                                        if (ans == "对") ans = "V";
                                        if (ans == "错") ans = "X";

                                        P_Data = P_Data + qid + "=" + ans + "&";
                                    }
                                    P_Data = P_Data.Substring(0, P_Data.Length - 1);
                                    Form1.MainForm.NutDebug(P_Data);

                                    var p_url = "http://www.scxfks.com/study/submitExercise/" + c_id + "/" + cl;
                                    var post_result = NutWeb.Nut_Post(p_url, P_Data, Cookie, null);
                                    if (post_result.Html.Contains("枩鑾峰緱婊"))
                                    {
                                        Form1.MainForm.NutDebug("答题成功");
                                    }
                                    else if (post_result.Html.Contains("凡杈惧埌浠婃棩瀛"))
                                    {
                                        Form1.MainForm.NutDebug("学习完毕");
                                        return;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 答题竞赛
        /// </summary>
        public static bool StartAnswer(String Cookie)
        {

            string Url = "http://www.scxfks.com/study/activity/score";
            string PostData = "id=20&correctAnswerNum=5";
            var PostResult = NutWeb.Nut_Post(Url, PostData, Cookie, null);
            if (PostResult != null)
            {
                return true;
            }
            return false;

        }

        //爬题库
        public static void GetPracticeAnswer(String Cookie)
        {
            //List<string> courseId = new List<string>();
            var url = "http://www.scxfks.com/study/index";
            var getResult = NutWeb.Nut_Get(url, null, Cookie);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(getResult.Html);

            var AllNode = doc.DocumentNode.SelectNodes("//ul[@class='film_focus_nav']//li");
            if (AllNode == null)
            {
                Form1.MainForm.NutDebug("StartStudy() Cookie失效");
                return;
            }

            foreach (var nav in AllNode)
            {
                var cl = nav.GetAttributeValue("cl", "");
                var atb = nav.GetAttributeValue("atb", "");
                //Form1.MainForm.NutDebug(cl+"  "+atb);

                var t_url = @"http://www.scxfks.com/study/selectCourse/" + cl + "/" + atb + "/51";
                var Result = NutWeb.Nut_Get(t_url, null, Cookie);
                var JsonObj = (JArray)JsonConvert.DeserializeObject(Result.Html);


                if (cl != "x_z_lxtk") continue;

                foreach (var j in JsonObj)
                {
                    JObject T_J = (JObject)JsonConvert.DeserializeObject(j.ToString());

                    //进入课程
                    var courseId = T_J["courseId"].ToString();
                    var c_url = @"http://www.scxfks.com/study/course/" + courseId;
                    var c_getResult = NutWeb.Nut_Get(c_url, null, Cookie);
                    HtmlDocument c_doc = new HtmlDocument();
                    c_doc.LoadHtml(c_getResult.Html);

                    var ChapterNodes = c_doc.DocumentNode.SelectNodes("//td[@class='title']");
                    foreach (var c_node in ChapterNodes)
                    {
                        if (!c_node.InnerText.Contains("待后续开放"))
                        {
                            var t_doc = new HtmlDocument();
                            t_doc.LoadHtml(c_node.InnerHtml);
                            var herf = t_doc.DocumentNode.SelectSingleNode("//a");
                            var Link = herf.GetAttributeValue("href", null);
                            //Form1.MainForm.NutDebug(Link);

                            String s_url = "";
                            if (Link == "javascript:void(0);")//练习题
                            {

                                var id_att = herf.GetAttributeValue("onclick", "");
                                var id = Global.TextGainCenter(",", ");", id_att);
                                s_url = "http://www.scxfks.com/study/exercise/" + id;
                                //生成题库
                                NutWeb.Nut_Post("http://www.scxfks.com/study/exerciseBuild/" + courseId + "/chapter/" + id, "", Cookie, null);

                            }
                            else
                            {
                                s_url = "http://www.scxfks.com" + Link;
                            }

                            Form1.MainForm.NutDebug(s_url);

                            if (s_url.Contains("exercise"))
                            {

                                var c_id = Global.SubStringToEnd(s_url, "exercise/");
                                var getr = NutWeb.Nut_Get(s_url, null, Cookie);
                                var e_doc = new HtmlDocument();
                                e_doc.LoadHtml(getr.Html);

                                var Answer_node = e_doc.DocumentNode.SelectNodes("//div[@class='item']");
                                if (Answer_node == null)
                                {
                                    continue;
                                }

                         
                                foreach (var Answer in Answer_node)
                                {
                                 

                                    var d_doc = new HtmlDocument();
                                    d_doc.LoadHtml(Answer.InnerHtml);
                                    var Qusetion = d_doc.DocumentNode.SelectSingleNode("//h3[@class='question-title']").InnerText;

                                    //var ans = Global.TextGainCenter("正确答案：", "\n            \n", Answer.InnerText);
                                    var RightAns = d_doc.DocumentNode.SelectSingleNode("//label[@class='answer']").GetAttributeValue("val","");
                                    //var Tpath = "//input[@value='"+ RightAns + "']";
                                    var Tpath = "//li[@class='question-option']";
                                    var Ans = d_doc.DocumentNode.SelectNodes(Tpath);
                                    String Ra = "";
                                    foreach (var a in Ans)
                                    {
                                        if (a.InnerText.Contains(RightAns))
                                        {
                                            Ra = a.InnerText;
                                            break;
                                        }
                                    }


                                    Qusetion = Qusetion.Replace(" ", "").Replace("\n", "");
                                    Form1.MainForm.NutDebug(Qusetion);
                                    Form1.MainForm.NutDebug(Ra);

                                    string filePath = "C:\\Users\\CrazyNut\\Desktop\\题库.txt";  // 你的文本文件路径
                                    string[] lines = { Qusetion, Ra+"\n" };  // 你要写入的两行字符串

                                    try
                                    {
                                        using (StreamWriter sw = new StreamWriter(filePath, true))  // 第二个参数为true表示追加模式
                                        {
                                            foreach (string line in lines)
                                            {
                                                sw.WriteLine(line);  // 写入一行字符串
                                            }
                                        }
                                        Console.WriteLine("写入成功！");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"发生错误：{ex.Message}");
                                    }

                                }
                      
               

                            }
                        }


                    }
                }
            }

        }
    }
}
