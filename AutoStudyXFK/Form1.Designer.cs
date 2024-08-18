
namespace AutoStudyXFK
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.UserName_txtbox = new System.Windows.Forms.TextBox();
            this.PassWord_txtbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Refresh_Btn = new System.Windows.Forms.Button();
            this.Captcha_txtbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.DebugWindow = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.总学分 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.今日学习 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.答题竞赛 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AutoRun = new System.Windows.Forms.Timer(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // UserName_txtbox
            // 
            this.UserName_txtbox.Location = new System.Drawing.Point(60, 20);
            this.UserName_txtbox.Name = "UserName_txtbox";
            this.UserName_txtbox.Size = new System.Drawing.Size(114, 21);
            this.UserName_txtbox.TabIndex = 1;
            // 
            // PassWord_txtbox
            // 
            this.PassWord_txtbox.Location = new System.Drawing.Point(60, 47);
            this.PassWord_txtbox.Name = "PassWord_txtbox";
            this.PassWord_txtbox.Size = new System.Drawing.Size(114, 21);
            this.PassWord_txtbox.TabIndex = 3;
            this.PassWord_txtbox.Text = "xfks1234";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "密码：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Refresh_Btn);
            this.groupBox1.Controls.Add(this.Captcha_txtbox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.PassWord_txtbox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.UserName_txtbox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(311, 147);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "添加账号";
            // 
            // Refresh_Btn
            // 
            this.Refresh_Btn.Location = new System.Drawing.Point(180, 74);
            this.Refresh_Btn.Name = "Refresh_Btn";
            this.Refresh_Btn.Size = new System.Drawing.Size(117, 25);
            this.Refresh_Btn.TabIndex = 9;
            this.Refresh_Btn.Text = "刷新验证码";
            this.Refresh_Btn.UseVisualStyleBackColor = true;
            this.Refresh_Btn.Click += new System.EventHandler(this.Refresh_Btn_Click);
            // 
            // Captcha_txtbox
            // 
            this.Captcha_txtbox.Location = new System.Drawing.Point(60, 75);
            this.Captcha_txtbox.Name = "Captcha_txtbox";
            this.Captcha_txtbox.Size = new System.Drawing.Size(114, 21);
            this.Captcha_txtbox.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "验证码：";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(180, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(117, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(99, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 36);
            this.button1.TabIndex = 4;
            this.button1.Text = "登录";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DebugWindow
            // 
            this.DebugWindow.BackColor = System.Drawing.SystemColors.MenuText;
            this.DebugWindow.ForeColor = System.Drawing.Color.Aqua;
            this.DebugWindow.Location = new System.Drawing.Point(329, 12);
            this.DebugWindow.Multiline = true;
            this.DebugWindow.Name = "DebugWindow";
            this.DebugWindow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DebugWindow.Size = new System.Drawing.Size(459, 426);
            this.DebugWindow.TabIndex = 5;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.name,
            this.id,
            this.总学分,
            this.今日学习,
            this.答题竞赛});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 172);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(308, 233);
            this.listView1.TabIndex = 11;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // name
            // 
            this.name.Text = "姓名";
            this.name.Width = 47;
            // 
            // id
            // 
            this.id.Text = "ID";
            this.id.Width = 50;
            // 
            // 总学分
            // 
            this.总学分.Text = "总学分";
            this.总学分.Width = 52;
            // 
            // 今日学习
            // 
            this.今日学习.Text = "今日学习分数";
            this.今日学习.Width = 86;
            // 
            // 答题竞赛
            // 
            this.答题竞赛.Text = "答题竞赛";
            this.答题竞赛.Width = 68;
            // 
            // AutoRun
            // 
            this.AutoRun.Tick += new System.EventHandler(this.AutoRun_Tick);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 409);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(86, 31);
            this.button3.TabIndex = 12;
            this.button3.Text = "开始挂机";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(104, 416);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(60, 16);
            this.checkBox2.TabIndex = 14;
            this.checkBox2.Text = "只答题";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 444);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.DebugWindow);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "四川自动学法 Build By CrazyNut";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox UserName_txtbox;
        private System.Windows.Forms.TextBox PassWord_txtbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox DebugWindow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Captcha_txtbox;
        private System.Windows.Forms.Button Refresh_Btn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader 总学分;
        private System.Windows.Forms.ColumnHeader 今日学习;
        private System.Windows.Forms.Timer AutoRun;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ColumnHeader 答题竞赛;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
    }
}

