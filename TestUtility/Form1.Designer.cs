
namespace TestUtility
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LoadOrdersOld = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.MustHaveButton = new System.Windows.Forms.Button();
            this.AddPharm = new System.Windows.Forms.Button();
            this.LoadOrders = new System.Windows.Forms.Button();
            this.LoadAss = new System.Windows.Forms.Button();
            this.AttachPharm = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.PharmAuthkey = new System.Windows.Forms.TextBox();
            this.auLP = new System.Windows.Forms.Label();
            this.PharmId = new System.Windows.Forms.TextBox();
            this.LicCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.SlaveAuthkey = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.SlavePharmId = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SlaveEmail = new System.Windows.Forms.TextBox();
            this.CreateWNPass = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.AuthEmail = new System.Windows.Forms.TextBox();
            this.Service = new System.Windows.Forms.TextBox();
            this.AuthPass = new System.Windows.Forms.TextBox();
            this.Register = new System.Windows.Forms.Button();
            this.Authorize = new System.Windows.Forms.Button();
            this.RegName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.RegSurname = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.AddUser = new System.Windows.Forms.Button();
            this.PharmsResp = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UserFromGuest = new System.Windows.Forms.Button();
            this.CreateGuestButton = new System.Windows.Forms.Button();
            this.UidLabel = new System.Windows.Forms.Label();
            this.UserId = new System.Windows.Forms.TextBox();
            this.FillMedkit = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.PharmIdForOrder = new System.Windows.Forms.TextBox();
            this.RecFrame = new System.Windows.Forms.WebBrowser();
            this.authkeyUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.MakeOrder = new System.Windows.Forms.Button();
            this.GetReceiptUrl = new System.Windows.Forms.Button();
            this.GetReceipJson = new System.Windows.Forms.Button();
            this.recCLabel = new System.Windows.Forms.Label();
            this.receiptcode = new System.Windows.Forms.TextBox();
            this.DOC = new System.Windows.Forms.Label();
            this.DateOfRec = new System.Windows.Forms.TextBox();
            this.SnilsLabel = new System.Windows.Forms.Label();
            this.snils = new System.Windows.Forms.TextBox();
            this.FIO = new System.Windows.Forms.TextBox();
            this.fioLabel = new System.Windows.Forms.Label();
            this.response = new System.Windows.Forms.TextBox();
            this.barcode = new System.Windows.Forms.TextBox();
            this.check = new System.Windows.Forms.Button();
            this.GetDrugstoresByUserId = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.groupBox2.Controls.Add(this.GetDrugstoresByUserId);
            this.groupBox2.Controls.Add(this.LoadOrdersOld);
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.AddUser);
            this.groupBox2.Controls.Add(this.PharmsResp);
            this.groupBox2.Location = new System.Drawing.Point(466, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(445, 468);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PhTForPharms";
            // 
            // LoadOrdersOld
            // 
            this.LoadOrdersOld.Location = new System.Drawing.Point(224, 354);
            this.LoadOrdersOld.Name = "LoadOrdersOld";
            this.LoadOrdersOld.Size = new System.Drawing.Size(100, 23);
            this.LoadOrdersOld.TabIndex = 27;
            this.LoadOrdersOld.Text = "LoadOrdersOld";
            this.LoadOrdersOld.UseVisualStyleBackColor = true;
            this.LoadOrdersOld.Click += new System.EventHandler(this.LoadOrdersOld_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.MustHaveButton);
            this.groupBox6.Controls.Add(this.AddPharm);
            this.groupBox6.Controls.Add(this.LoadOrders);
            this.groupBox6.Controls.Add(this.LoadAss);
            this.groupBox6.Controls.Add(this.AttachPharm);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.PharmAuthkey);
            this.groupBox6.Controls.Add(this.auLP);
            this.groupBox6.Controls.Add(this.PharmId);
            this.groupBox6.Controls.Add(this.LicCode);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Location = new System.Drawing.Point(6, 266);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(433, 89);
            this.groupBox6.TabIndex = 26;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Аптеки";
            // 
            // MustHaveButton
            // 
            this.MustHaveButton.Location = new System.Drawing.Point(324, 31);
            this.MustHaveButton.Name = "MustHaveButton";
            this.MustHaveButton.Size = new System.Drawing.Size(100, 23);
            this.MustHaveButton.TabIndex = 21;
            this.MustHaveButton.Text = "ЖНВЛП";
            this.MustHaveButton.UseVisualStyleBackColor = true;
            this.MustHaveButton.Click += new System.EventHandler(this.MustHaveButton_Click);
            // 
            // AddPharm
            // 
            this.AddPharm.Location = new System.Drawing.Point(6, 59);
            this.AddPharm.Name = "AddPharm";
            this.AddPharm.Size = new System.Drawing.Size(100, 23);
            this.AddPharm.TabIndex = 20;
            this.AddPharm.Text = "Add Pharm";
            this.AddPharm.UseVisualStyleBackColor = true;
            this.AddPharm.Click += new System.EventHandler(this.AddPharm_Click);
            // 
            // LoadOrders
            // 
            this.LoadOrders.Location = new System.Drawing.Point(218, 59);
            this.LoadOrders.Name = "LoadOrders";
            this.LoadOrders.Size = new System.Drawing.Size(100, 23);
            this.LoadOrders.TabIndex = 19;
            this.LoadOrders.Text = "LoadOrders";
            this.LoadOrders.UseVisualStyleBackColor = true;
            this.LoadOrders.Click += new System.EventHandler(this.LoadOrders_Click);
            // 
            // LoadAss
            // 
            this.LoadAss.Location = new System.Drawing.Point(324, 59);
            this.LoadAss.Name = "LoadAss";
            this.LoadAss.Size = new System.Drawing.Size(100, 23);
            this.LoadAss.TabIndex = 11;
            this.LoadAss.Text = "LoadAssortiment";
            this.LoadAss.UseVisualStyleBackColor = true;
            this.LoadAss.Click += new System.EventHandler(this.LoadAss_Click);
            // 
            // AttachPharm
            // 
            this.AttachPharm.Location = new System.Drawing.Point(112, 59);
            this.AttachPharm.Name = "AttachPharm";
            this.AttachPharm.Size = new System.Drawing.Size(100, 23);
            this.AttachPharm.TabIndex = 17;
            this.AttachPharm.Text = "AttachPharm";
            this.AttachPharm.UseVisualStyleBackColor = true;
            this.AttachPharm.Click += new System.EventHandler(this.AttachPharm_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(3, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "PharmId";
            // 
            // PharmAuthkey
            // 
            this.PharmAuthkey.Location = new System.Drawing.Point(218, 33);
            this.PharmAuthkey.Name = "PharmAuthkey";
            this.PharmAuthkey.Size = new System.Drawing.Size(100, 20);
            this.PharmAuthkey.TabIndex = 8;
            // 
            // auLP
            // 
            this.auLP.AutoSize = true;
            this.auLP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.auLP.Location = new System.Drawing.Point(215, 17);
            this.auLP.Name = "auLP";
            this.auLP.Size = new System.Drawing.Size(53, 13);
            this.auLP.TabIndex = 7;
            this.auLP.Text = "Authkey";
            // 
            // PharmId
            // 
            this.PharmId.Location = new System.Drawing.Point(6, 33);
            this.PharmId.Name = "PharmId";
            this.PharmId.Size = new System.Drawing.Size(100, 20);
            this.PharmId.TabIndex = 10;
            // 
            // LicCode
            // 
            this.LicCode.Location = new System.Drawing.Point(112, 33);
            this.LicCode.Name = "LicCode";
            this.LicCode.Size = new System.Drawing.Size(100, 20);
            this.LicCode.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(109, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "LicCode";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.SlaveAuthkey);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.textBox2);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.SlavePharmId);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.SlaveEmail);
            this.groupBox4.Controls.Add(this.CreateWNPass);
            this.groupBox4.Location = new System.Drawing.Point(6, 166);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(433, 94);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Приглашение пользователя";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(218, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(46, 13);
            this.label13.TabIndex = 29;
            this.label13.Text = "Authkey";
            // 
            // SlaveAuthkey
            // 
            this.SlaveAuthkey.Location = new System.Drawing.Point(218, 39);
            this.SlaveAuthkey.Name = "SlaveAuthkey";
            this.SlaveAuthkey.Size = new System.Drawing.Size(100, 20);
            this.SlaveAuthkey.TabIndex = 28;
            this.SlaveAuthkey.Text = "37551e9be0c9611e";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(321, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Приложение";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(365, 39);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(15, 20);
            this.textBox2.TabIndex = 26;
            this.textBox2.Text = "6";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(109, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "ID Аптеки";
            // 
            // SlavePharmId
            // 
            this.SlavePharmId.Location = new System.Drawing.Point(112, 39);
            this.SlavePharmId.Name = "SlavePharmId";
            this.SlavePharmId.Size = new System.Drawing.Size(100, 20);
            this.SlavePharmId.TabIndex = 24;
            this.SlavePharmId.Text = "_2381X5KSZ53MC";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Email Раба";
            // 
            // SlaveEmail
            // 
            this.SlaveEmail.Location = new System.Drawing.Point(6, 39);
            this.SlaveEmail.Name = "SlaveEmail";
            this.SlaveEmail.Size = new System.Drawing.Size(100, 20);
            this.SlaveEmail.TabIndex = 22;
            // 
            // CreateWNPass
            // 
            this.CreateWNPass.Location = new System.Drawing.Point(6, 65);
            this.CreateWNPass.Name = "CreateWNPass";
            this.CreateWNPass.Size = new System.Drawing.Size(100, 23);
            this.CreateWNPass.TabIndex = 21;
            this.CreateWNPass.Text = "New Slave User";
            this.CreateWNPass.UseVisualStyleBackColor = true;
            this.CreateWNPass.Click += new System.EventHandler(this.CreateWNPass_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(183, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "LoggedOut";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.AuthEmail);
            this.groupBox3.Controls.Add(this.Service);
            this.groupBox3.Controls.Add(this.AuthPass);
            this.groupBox3.Controls.Add(this.Register);
            this.groupBox3.Controls.Add(this.Authorize);
            this.groupBox3.Controls.Add(this.RegName);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.RegSurname);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(6, 29);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(433, 131);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Авторизация";
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(233, 131);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 100);
            this.groupBox5.TabIndex = 26;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "groupBox5";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Password";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Email";
            // 
            // AuthEmail
            // 
            this.AuthEmail.Location = new System.Drawing.Point(6, 32);
            this.AuthEmail.Name = "AuthEmail";
            this.AuthEmail.Size = new System.Drawing.Size(200, 20);
            this.AuthEmail.TabIndex = 0;
            // 
            // Service
            // 
            this.Service.Location = new System.Drawing.Point(6, 94);
            this.Service.Name = "Service";
            this.Service.Size = new System.Drawing.Size(26, 20);
            this.Service.TabIndex = 22;
            this.Service.Text = "7";
            // 
            // AuthPass
            // 
            this.AuthPass.Location = new System.Drawing.Point(6, 68);
            this.AuthPass.Name = "AuthPass";
            this.AuthPass.Size = new System.Drawing.Size(200, 20);
            this.AuthPass.TabIndex = 1;
            // 
            // Register
            // 
            this.Register.Location = new System.Drawing.Point(227, 96);
            this.Register.Name = "Register";
            this.Register.Size = new System.Drawing.Size(75, 23);
            this.Register.TabIndex = 2;
            this.Register.Text = "Register";
            this.Register.UseVisualStyleBackColor = true;
            this.Register.Click += new System.EventHandler(this.Register_Click);
            // 
            // Authorize
            // 
            this.Authorize.Location = new System.Drawing.Point(131, 96);
            this.Authorize.Name = "Authorize";
            this.Authorize.Size = new System.Drawing.Size(75, 23);
            this.Authorize.TabIndex = 3;
            this.Authorize.Text = "Authorize";
            this.Authorize.UseVisualStyleBackColor = true;
            this.Authorize.Click += new System.EventHandler(this.Authorize_Click);
            // 
            // RegName
            // 
            this.RegName.Location = new System.Drawing.Point(227, 70);
            this.RegName.Name = "RegName";
            this.RegName.Size = new System.Drawing.Size(200, 20);
            this.RegName.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(224, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Name";
            // 
            // RegSurname
            // 
            this.RegSurname.Location = new System.Drawing.Point(227, 32);
            this.RegSurname.Name = "RegSurname";
            this.RegSurname.Size = new System.Drawing.Size(200, 20);
            this.RegSurname.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(224, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Familia";
            // 
            // AddUser
            // 
            this.AddUser.Location = new System.Drawing.Point(6, 351);
            this.AddUser.Name = "AddUser";
            this.AddUser.Size = new System.Drawing.Size(90, 23);
            this.AddUser.TabIndex = 16;
            this.AddUser.Text = "AddUser";
            this.AddUser.UseVisualStyleBackColor = true;
            this.AddUser.Click += new System.EventHandler(this.AddUser_Click);
            // 
            // PharmsResp
            // 
            this.PharmsResp.Location = new System.Drawing.Point(6, 380);
            this.PharmsResp.Multiline = true;
            this.PharmsResp.Name = "PharmsResp";
            this.PharmsResp.Size = new System.Drawing.Size(433, 82);
            this.PharmsResp.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox1.Controls.Add(this.UserFromGuest);
            this.groupBox1.Controls.Add(this.CreateGuestButton);
            this.groupBox1.Controls.Add(this.UidLabel);
            this.groupBox1.Controls.Add(this.UserId);
            this.groupBox1.Controls.Add(this.FillMedkit);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.PharmIdForOrder);
            this.groupBox1.Controls.Add(this.RecFrame);
            this.groupBox1.Controls.Add(this.authkeyUser);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.MakeOrder);
            this.groupBox1.Controls.Add(this.GetReceiptUrl);
            this.groupBox1.Controls.Add(this.GetReceipJson);
            this.groupBox1.Controls.Add(this.recCLabel);
            this.groupBox1.Controls.Add(this.receiptcode);
            this.groupBox1.Controls.Add(this.DOC);
            this.groupBox1.Controls.Add(this.DateOfRec);
            this.groupBox1.Controls.Add(this.SnilsLabel);
            this.groupBox1.Controls.Add(this.snils);
            this.groupBox1.Controls.Add(this.FIO);
            this.groupBox1.Controls.Add(this.fioLabel);
            this.groupBox1.Controls.Add(this.response);
            this.groupBox1.Controls.Add(this.barcode);
            this.groupBox1.Controls.Add(this.check);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(445, 468);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PhTForUsers";
            // 
            // UserFromGuest
            // 
            this.UserFromGuest.Location = new System.Drawing.Point(120, 308);
            this.UserFromGuest.Name = "UserFromGuest";
            this.UserFromGuest.Size = new System.Drawing.Size(96, 23);
            this.UserFromGuest.TabIndex = 23;
            this.UserFromGuest.Text = "User from Guest";
            this.UserFromGuest.UseVisualStyleBackColor = true;
            this.UserFromGuest.Click += new System.EventHandler(this.UserFromGuest_Click);
            // 
            // CreateGuestButton
            // 
            this.CreateGuestButton.Location = new System.Drawing.Point(6, 308);
            this.CreateGuestButton.Name = "CreateGuestButton";
            this.CreateGuestButton.Size = new System.Drawing.Size(75, 23);
            this.CreateGuestButton.TabIndex = 22;
            this.CreateGuestButton.Text = "Guest";
            this.CreateGuestButton.UseVisualStyleBackColor = true;
            this.CreateGuestButton.Click += new System.EventHandler(this.CreateGuestButton_Click);
            // 
            // UidLabel
            // 
            this.UidLabel.AutoSize = true;
            this.UidLabel.Location = new System.Drawing.Point(145, 178);
            this.UidLabel.Name = "UidLabel";
            this.UidLabel.Size = new System.Drawing.Size(38, 13);
            this.UidLabel.TabIndex = 21;
            this.UidLabel.Text = "UserId";
            // 
            // UserId
            // 
            this.UserId.Location = new System.Drawing.Point(145, 195);
            this.UserId.Name = "UserId";
            this.UserId.Size = new System.Drawing.Size(100, 20);
            this.UserId.TabIndex = 20;
            // 
            // FillMedkit
            // 
            this.FillMedkit.Location = new System.Drawing.Point(358, 153);
            this.FillMedkit.Name = "FillMedkit";
            this.FillMedkit.Size = new System.Drawing.Size(81, 39);
            this.FillMedkit.TabIndex = 19;
            this.FillMedkit.Text = "Пополнить аптечку";
            this.FillMedkit.UseVisualStyleBackColor = true;
            this.FillMedkit.Click += new System.EventHandler(this.FillMedkit_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(257, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "PharmId";
            // 
            // PharmIdForOrder
            // 
            this.PharmIdForOrder.Location = new System.Drawing.Point(260, 195);
            this.PharmIdForOrder.Name = "PharmIdForOrder";
            this.PharmIdForOrder.Size = new System.Drawing.Size(100, 20);
            this.PharmIdForOrder.TabIndex = 17;
            // 
            // RecFrame
            // 
            this.RecFrame.Location = new System.Drawing.Point(260, 221);
            this.RecFrame.MinimumSize = new System.Drawing.Size(20, 20);
            this.RecFrame.Name = "RecFrame";
            this.RecFrame.Size = new System.Drawing.Size(167, 153);
            this.RecFrame.TabIndex = 16;
            // 
            // authkeyUser
            // 
            this.authkeyUser.Location = new System.Drawing.Point(6, 282);
            this.authkeyUser.Name = "authkeyUser";
            this.authkeyUser.Size = new System.Drawing.Size(210, 20);
            this.authkeyUser.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(6, 266);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "authkey";
            // 
            // MakeOrder
            // 
            this.MakeOrder.Location = new System.Drawing.Point(260, 153);
            this.MakeOrder.Name = "MakeOrder";
            this.MakeOrder.Size = new System.Drawing.Size(97, 23);
            this.MakeOrder.TabIndex = 13;
            this.MakeOrder.Text = "Сделать заказ";
            this.MakeOrder.UseVisualStyleBackColor = true;
            this.MakeOrder.Click += new System.EventHandler(this.MakeOrder_Click);
            // 
            // GetReceiptUrl
            // 
            this.GetReceiptUrl.Location = new System.Drawing.Point(260, 111);
            this.GetReceiptUrl.Name = "GetReceiptUrl";
            this.GetReceiptUrl.Size = new System.Drawing.Size(136, 36);
            this.GetReceiptUrl.TabIndex = 12;
            this.GetReceiptUrl.Text = "Получить страницу рецепта";
            this.GetReceiptUrl.UseVisualStyleBackColor = true;
            this.GetReceiptUrl.Click += new System.EventHandler(this.GetReceiptUrl_Click);
            // 
            // GetReceipJson
            // 
            this.GetReceipJson.Location = new System.Drawing.Point(260, 82);
            this.GetReceipJson.Name = "GetReceipJson";
            this.GetReceipJson.Size = new System.Drawing.Size(136, 23);
            this.GetReceipJson.TabIndex = 11;
            this.GetReceipJson.Text = "Получить рецепт JSON";
            this.GetReceipJson.UseVisualStyleBackColor = true;
            this.GetReceipJson.Click += new System.EventHandler(this.GetReceipJson_Click);
            // 
            // recCLabel
            // 
            this.recCLabel.AutoSize = true;
            this.recCLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.recCLabel.Location = new System.Drawing.Point(6, 221);
            this.recCLabel.Name = "recCLabel";
            this.recCLabel.Size = new System.Drawing.Size(170, 13);
            this.recCLabel.TabIndex = 10;
            this.recCLabel.Text = "Код заказа/номер рецепта";
            // 
            // receiptcode
            // 
            this.receiptcode.Location = new System.Drawing.Point(6, 240);
            this.receiptcode.Name = "receiptcode";
            this.receiptcode.Size = new System.Drawing.Size(100, 20);
            this.receiptcode.TabIndex = 9;
            // 
            // DOC
            // 
            this.DOC.AutoSize = true;
            this.DOC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DOC.Location = new System.Drawing.Point(6, 166);
            this.DOC.Name = "DOC";
            this.DOC.Size = new System.Drawing.Size(118, 13);
            this.DOC.TabIndex = 8;
            this.DOC.Text = "МесяцГод рецепта";
            // 
            // DateOfRec
            // 
            this.DateOfRec.Location = new System.Drawing.Point(6, 185);
            this.DateOfRec.Name = "DateOfRec";
            this.DateOfRec.Size = new System.Drawing.Size(100, 20);
            this.DateOfRec.TabIndex = 7;
            // 
            // SnilsLabel
            // 
            this.SnilsLabel.AutoSize = true;
            this.SnilsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SnilsLabel.Location = new System.Drawing.Point(6, 111);
            this.SnilsLabel.Name = "SnilsLabel";
            this.SnilsLabel.Size = new System.Drawing.Size(50, 13);
            this.SnilsLabel.TabIndex = 6;
            this.SnilsLabel.Text = "СНИЛС";
            // 
            // snils
            // 
            this.snils.Location = new System.Drawing.Point(6, 127);
            this.snils.Name = "snils";
            this.snils.Size = new System.Drawing.Size(210, 20);
            this.snils.TabIndex = 5;
            // 
            // FIO
            // 
            this.FIO.Location = new System.Drawing.Point(6, 82);
            this.FIO.Name = "FIO";
            this.FIO.Size = new System.Drawing.Size(210, 20);
            this.FIO.TabIndex = 4;
            // 
            // fioLabel
            // 
            this.fioLabel.AutoSize = true;
            this.fioLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fioLabel.Location = new System.Drawing.Point(6, 65);
            this.fioLabel.Name = "fioLabel";
            this.fioLabel.Size = new System.Drawing.Size(37, 13);
            this.fioLabel.TabIndex = 3;
            this.fioLabel.Text = "ФИО";
            // 
            // response
            // 
            this.response.Location = new System.Drawing.Point(6, 380);
            this.response.Multiline = true;
            this.response.Name = "response";
            this.response.Size = new System.Drawing.Size(433, 82);
            this.response.TabIndex = 2;
            // 
            // barcode
            // 
            this.barcode.Location = new System.Drawing.Point(6, 19);
            this.barcode.Name = "barcode";
            this.barcode.Size = new System.Drawing.Size(309, 20);
            this.barcode.TabIndex = 1;
            this.barcode.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // check
            // 
            this.check.Location = new System.Drawing.Point(321, 17);
            this.check.Name = "check";
            this.check.Size = new System.Drawing.Size(75, 23);
            this.check.TabIndex = 0;
            this.check.Text = "<--- Check";
            this.check.UseVisualStyleBackColor = true;
            this.check.Click += new System.EventHandler(this.check_Click);
            // 
            // GetDrugstoresByUserId
            // 
            this.GetDrugstoresByUserId.Location = new System.Drawing.Point(118, 354);
            this.GetDrugstoresByUserId.Name = "GetDrugstoresByUserId";
            this.GetDrugstoresByUserId.Size = new System.Drawing.Size(100, 23);
            this.GetDrugstoresByUserId.TabIndex = 28;
            this.GetDrugstoresByUserId.Text = "GetDrugstoresByUserId";
            this.GetDrugstoresByUserId.UseVisualStyleBackColor = true;
            this.GetDrugstoresByUserId.Click += new System.EventHandler(this.GetDrugstoresByUserId_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(923, 496);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox response;
        private System.Windows.Forms.TextBox barcode;
        private System.Windows.Forms.Button check;
        private System.Windows.Forms.Button GetReceipJson;
        private System.Windows.Forms.Label recCLabel;
        private System.Windows.Forms.TextBox receiptcode;
        private System.Windows.Forms.Label DOC;
        private System.Windows.Forms.TextBox DateOfRec;
        private System.Windows.Forms.Label SnilsLabel;
        private System.Windows.Forms.TextBox snils;
        private System.Windows.Forms.TextBox FIO;
        private System.Windows.Forms.Label fioLabel;
        private System.Windows.Forms.Button MakeOrder;
        private System.Windows.Forms.Button GetReceiptUrl;
        private System.Windows.Forms.Button Authorize;
        private System.Windows.Forms.Button Register;
        private System.Windows.Forms.TextBox AuthPass;
        private System.Windows.Forms.TextBox AuthEmail;
        private System.Windows.Forms.TextBox PharmsResp;
        private System.Windows.Forms.TextBox LicCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PharmAuthkey;
        private System.Windows.Forms.Label auLP;
        private System.Windows.Forms.TextBox authkeyUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PharmId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.WebBrowser RecFrame;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PharmIdForOrder;
        private System.Windows.Forms.Button LoadAss;
        private System.Windows.Forms.TextBox RegName;
        private System.Windows.Forms.TextBox RegSurname;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button AttachPharm;
        private System.Windows.Forms.Button AddUser;
        private System.Windows.Forms.Button LoadOrders;
        private System.Windows.Forms.Button FillMedkit;
        private System.Windows.Forms.Label UidLabel;
        private System.Windows.Forms.TextBox UserId;
        private System.Windows.Forms.Button AddPharm;
        private System.Windows.Forms.Button CreateWNPass;
        private System.Windows.Forms.TextBox Service;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox SlavePharmId;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox SlaveEmail;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox SlaveAuthkey;
        private System.Windows.Forms.Button CreateGuestButton;
        private System.Windows.Forms.Button UserFromGuest;
        private System.Windows.Forms.Button LoadOrdersOld;
        private System.Windows.Forms.Button MustHaveButton;
        private System.Windows.Forms.Button GetDrugstoresByUserId;
    }
}

