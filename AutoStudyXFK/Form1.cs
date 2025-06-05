using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HttpCodeLib;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace AutoStudyXFK
{
    public partial class Form1 : Form
    {
        public static Form1 MainForm;
        public Form1()
        {
            InitializeComponent();
            MainForm = this;
          
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            NutDebug("此软件仅用于交流学习，请勿违规使用此软件。");
            NutDebug("选择只答题模式每天可以多得一分（够学到100分了）");
            ReadUserFormJson();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Global.LoginCookie == null)
            {
                NutDebug("请先获取验证码");
                return;
            }
            var id = UserName_txtbox.Text;
            var psw = PassWord_txtbox.Text;
            var captcha = Captcha_txtbox.Text;
            
            var result = WebAPI.Login(id, psw, captcha);

            if (result.Contains("登录成功"))
            {
                NutDebug(Global.LoginCookie);
                var user = new User();
                user.Cookie = Global.LoginCookie;
                user.PassWord = psw;
                user.PhoneNum = id;
                user = WebAPI.GetInfo(user);
                AddUserToListView(user);
                RefreshCaptcha();
                return;
            }
            NutDebug("登录失败，请查看上方日志");
            RefreshCaptcha();
        }


        public delegate void DebugWindowDelegate(String Text);
        public void NutDebug(String Text)
        {

            if (InvokeRequired)
            {
                DebugWindowDelegate _NutDebug = new DebugWindowDelegate(NutDebug);
                label1.Invoke(_NutDebug, new object[] { Text });
            }
            else
            {
                var time = DateTime.Now.ToString().Replace(@"/", "-");
                var text = "[" + time + "]--->" + Text;

                Debug.WriteLine(text);

                DebugWindow.Text += "\r\n" + text + "\r\n";

                if (DebugWindow.Text.Length > 10000)
                {
                    DebugWindow.Text = DebugWindow.Text.Remove(0, 3000);
                }

                DebugWindow.Focus();
                DebugWindow.Select(DebugWindow.Text.Length, 0);
                DebugWindow.ScrollToCaret();
            }
        }

        private void Refresh_Btn_Click(object sender, EventArgs e)
        {
            RefreshCaptcha();
        }

        public void RefreshCaptcha()
        {
            Global.LoginCookie = NutWeb.Nut_Get("http://www.scxfks.com/study/login", null).Cookie;
            var img = NutWeb.Nut_GetImage("http://www.scxfks.com/study/captcha", Global.LoginCookie);

            //方法1
            pictureBox1.Image = new HttpHelpers().GetImg(img);

            //打码狗
            //var Result = Global.DaMaGou(Global.PicCovert(pictureBox1.Image));

            //StupidOCR
            var Result = Global.StupidOCR(Global.PicCovert(pictureBox1.Image));

            if (Result.Contains("失败"))
            {
                NutDebug("StupidOCR链接失败，请检测服务是否开启！或是否在6688端口上运行。");
                return;
            }
      
            NutDebug("已经获取最新验证码，验证码识别结果：" + Result+"  如有错误请手动输入");
            Captcha_txtbox.Text = Result;
        }


        //public void SaveUserInfo(User user)
        //{
        //    if (user.Name == null) return;
        //    bool IsIn = false;
        //    //先判断账号是否在list中
        //    for (int i = 0; i < Global.UserList.Count; i++)
        //    {
        //        if (Global.UserList[i].Name == user.Name)
        //        {
        //            Global.UserList[i] = user;
        //            IsIn = true;      
        //            break;
        //        }
        //    }
        //    if (!IsIn) Global.UserList.Add(user);
        //    SaveUserToJson(user);
        //}

        public void AddUserToListView(User user)
        {
            var AccountNum = Form1.MainForm.listView1.Items.Count;
            if (AccountNum != 0)
            {
                for (int i = 0; i <= AccountNum - 1; i++)
                {
                    String name = "";
                    Invoke(new Action(() =>
                    {
                        name = Form1.MainForm.listView1.Items[i].SubItems[0].Text;
                    }));

                    if (name == user.Name)
                    {
                        Form1.MainForm.NutDebug("已经存在于列表的第 " + i.ToString() + " 行中");

                        Invoke(new Action(() =>
                        {
                            Form1.MainForm.listView1.Items[i].SubItems[0].Text = user.Name; 
                            Form1.MainForm.listView1.Items[i].SubItems[1].Text = user.ID; 
                            Form1.MainForm.listView1.Items[i].SubItems[2].Text = user.Score; 
                            Form1.MainForm.listView1.Items[i].SubItems[3].Text = user.TodayScore.ToString();
                            Form1.MainForm.listView1.Items[i].SubItems[4].Text = user.TodayAnswer.ToString();
                        }));
                        //更新List
                        bool InTheList = false;
                        foreach (var account in Global.UserList)
                        {
                            if (account.Name == user.Name)
                            {
                                int index = Global.UserList.IndexOf(account);
                                Global.UserList[index] = user;
                                InTheList = true;
                                break;
                            }
                        }
                        if (!InTheList)
                        {
                            Global.UserList.Add(user);
                        }
                        //更新json
                        SaveUserToJson(user);
                        Form1.MainForm.NutDebug("账号信息已经刷新");
                        return;
                    }

                }
            }


            //如果列表中没有对应账号
            var item = new ListViewItem(user.Name);
            item.SubItems.Add(user.ID);
            item.SubItems.Add(user.Score);
            item.SubItems.Add(user.TodayScore.ToString());
            item.SubItems.Add(user.TodayAnswer.ToString());


            //更新List
            bool InTheList2 = false;
            foreach (var account in Global.UserList)
            {
                if (account.Name == user.Name)
                {
                    int index = Global.UserList.IndexOf(account);
                    Global.UserList[index] = user;
                    InTheList2 = true;
                    break;
                }
            }
            if (!InTheList2)
            {
                Global.UserList.Add(user);
            }
            //更新json
            SaveUserToJson(user);
            Invoke(new Action(() => { Form1.MainForm.listView1.Items.Add(item); }));
        }

        public void SaveUserToJson(User user)
        {
            
            var path = Directory.GetCurrentDirectory() + "\\AccountInfo.json";
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            var Text = File.ReadAllText(path);

            JObject JsonObj = (JObject)JsonConvert.DeserializeObject(Text);

            JObject NewAccount = new JObject
            {
                { "phonenum", user.PhoneNum },
                { "password", user.PassWord },
                { "cookie", user.Cookie },
                { "name", user.Name },
                { "id", user.ID },
                { "score", user.Score },
                { "todayscore", user.TodayScore.ToString() },
                { "todayanswer", user.TodayAnswer.ToString() }
            };

            if (JsonObj == null)
            {
                JsonObj = new JObject(new JProperty("account", new JArray(NewAccount)));
            }

            //获取账号数量
            var count = ((JContainer)JsonObj["account"]).Count;

            if (count > 0)
            {
                var AccountList = JsonObj["account"];
                for (int i = 0; i < count; i++)
                {
                    var account = AccountList[i];
                    if (account["name"].ToString() == user.Name)
                    {
                        //如果账号存在于json中
                        if (user.Cookie.Replace(" ", "").Length > 10)
                        {
                            account["cookie"] = user.Cookie;
                        }
                        if (user.PassWord.Replace(" ", "").Length > 3)
                        {
                            account["password"] = user.PassWord;
                        }
                        account["score"] = user.Score;
                        account["todayscore"] = user.TodayScore.ToString();
                        account["todayanswer"] = user.TodayAnswer.ToString();



                        AccountList[i] = account;
                        JsonObj["account"] = AccountList;
                        string out_put = Newtonsoft.Json.JsonConvert.SerializeObject(JsonObj, Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(path, out_put, UTF8Encoding.UTF8);
                        return;
                    }
                }
            }

            if (count == 0)
            {
                JsonObj["account"] = new JArray(NewAccount);
                string out_put = Newtonsoft.Json.JsonConvert.SerializeObject(JsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(path, out_put, UTF8Encoding.UTF8);
                return;
            }

            JsonObj["account"].Last.AddAfterSelf(NewAccount);

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(JsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(path, output, UTF8Encoding.UTF8);
            return;
        }

        public void ReadUserFormJson()
        {
            var path = Directory.GetCurrentDirectory() + "\\AccountInfo.json";
            if (!File.Exists(path))
            {
                Form1.MainForm.NutDebug("本地没有账号");
                return;
            }
            var Text = File.ReadAllText(path);

            JObject JsonObj = (JObject)JsonConvert.DeserializeObject(Text);
            if (JsonObj == null)
            {
                JsonObj = new JObject(new JProperty("account", new JArray()));
            }
            //获取账号数量
            var count = ((JContainer)JsonObj["account"]).Count;


            if (count > 0)
            {
                Global.UserList.Clear();//清空List
                Form1.MainForm.NutDebug("本地读取到 " + count.ToString() + " 个账号");
                var AccountList = JsonObj["account"];
                for (int i = Global.ReadJsonCurrtenNum; i < count; i++)
                {
                    Global.ReadJsonCurrtenNum = i;//储存读取进度
                    var account = AccountList[i];
                    var AddAccount = new User();
                    AddAccount.PhoneNum =  account["phonenum"].ToString();
                    AddAccount.PassWord = account["password"].ToString();
                    AddAccount.Cookie = account["cookie"].ToString();
                    AddAccount.Name = account["name"].ToString();
                    AddAccount.ID = account["id"].ToString();
                    AddAccount.Score = account["score"].ToString();
                    AddAccount.TodayScore =float.Parse(account["todayscore"].ToString());
 
                    if (account["todayanswer"] == null)
                    {
                        AddAccount.TodayAnswer = -1;
                    }
                    else
                    {
                        AddAccount.TodayAnswer =int.Parse(account["todayanswer"].ToString());
                    }

                    Form1.MainForm.AddUserToListView(AddAccount);
                }
            }
            else
            {
                Form1.MainForm.NutDebug("本地没有账号");
            }
            Form1.MainForm.NutDebug("本地账号导入完毕");
            Global.ReadJsonCurrtenNum = 0;//导入完毕，清空读取进度

        }
        Thread AutoRunThread;
        private void AutoRun_Tick(object sender, EventArgs e)
        {
            AutoRun.Interval = 1800000;
            if (AutoRunThread == null || !AutoRunThread.IsAlive)
            {
                AutoRunThread = new Thread(RunTimerInvoke);
                AutoRunThread.Start();
            }
            NutDebug("下次将在30分钟后再次检测");
        }

        public void RunTimerInvoke()
        {
            TryRestTaskInfo();

            for (int i=0;i< Global.UserList.Count;i++)
            {
                var user = Global.UserList[i];

                if (user.TodayScore < 5|| user.TodayAnswer<0)
                {
                    var t_user = WebAPI.GetInfo(user);
                    if (t_user.Cookie == null)
                    {
                        NutDebug("Cookie失效需要重新登录");
                        //开始尝试重新登录
                        var loginr = WebAPI.AutoLogin(user.PhoneNum, user.PassWord);
                        if (loginr == null)
                        {
                            NutDebug("登录失败，请查看上方日志，暂时跳过此账号。");
                            continue;
                        }
                        user.Cookie = loginr;
                        user = WebAPI.GetInfo(user);
                    }

                    WebAPI.StartStudy(user.Cookie);
                    WebAPI.StartAnswer(user.Cookie);
                    user = WebAPI.GetInfo(user);
                    AddUserToListView(user);
                }
            }
            NutDebug("所有账号检测完毕");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (AutoRun.Enabled == false)
            {
                AutoRun.Interval = 1;
                AutoRun.Enabled = true;
                button3.Text = "挂机检测中";
 
            }
            else
            {
                //结束挂机
                AutoRun.Enabled = false;
                button3.Text = "开始挂机";
                NutDebug("挂机已经停止");
            }
        }



        public void TryRestTaskInfo()
        {
            //判断当前时间是否应该刷新任务信息 - 每天刷新一次
            var path = Directory.GetCurrentDirectory() + "\\RestTime.txt";

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            var Text = File.ReadAllText(path);
            var Today = DateTime.Parse(DateTime.Now.ToShortDateString());
            var LastRestDayInfo = Text;

            if (LastRestDayInfo != "")
            {
                var LastRestDay = DateTime.Parse(LastRestDayInfo);
                if (LastRestDay >= Today)
                {
                    return; //如果上次 刷新的时间 大于等于 今天 就不用刷新
                }
            }
            //刷新操作
            for (int i = 0; i < Global.UserList.Count; i++)
            {
                var CacheAccount = Global.UserList[i];
                CacheAccount.TodayScore = 0;
                AddUserToListView(CacheAccount);
            }

            File.WriteAllText(path, Today.ToString(), UTF8Encoding.UTF8);
            Form1.MainForm.NutDebug("账号信息已经刷新！");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Global.OnlyQA = checkBox2.Checked;
        }


    }
}
