namespace parser_OZON_webview
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label1 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            linkLabel1 = new LinkLabel();
            txtHistoryText = new TextBox();
            dataGridView1 = new DataGridView();
            Номер = new DataGridViewTextBoxColumn();
            Артикул = new DataGridViewTextBoxColumn();
            Цена_с_картой = new DataGridViewTextBoxColumn();
            Цена = new DataGridViewTextBoxColumn();
            Цена_старая = new DataGridViewTextBoxColumn();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            btnStartParse = new Button();
            notifyIcon1 = new NotifyIcon(components);
            timer1 = new System.Windows.Forms.Timer(components);
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 18);
            label1.Name = "label1";
            label1.Size = new Size(393, 17);
            label1.TabIndex = 0;
            label1.Text = "Всё готово. Расписание будет запущено после запуска парсера.";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(91, 50);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(321, 25);
            textBox1.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 53);
            label2.Name = "label2";
            label2.Size = new Size(67, 17);
            label2.TabIndex = 2;
            label2.Text = "Источник:";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(785, 9);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(79, 17);
            linkLabel1.TabIndex = 3;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "cmacuk.t.me";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // txtHistoryText
            // 
            txtHistoryText.BackColor = Color.White;
            txtHistoryText.Location = new Point(6, 24);
            txtHistoryText.Multiline = true;
            txtHistoryText.Name = "txtHistoryText";
            txtHistoryText.ScrollBars = ScrollBars.Vertical;
            txtHistoryText.Size = new Size(282, 476);
            txtHistoryText.TabIndex = 4;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Номер, Артикул, Цена_с_картой, Цена, Цена_старая });
            dataGridView1.Location = new Point(6, 24);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Size = new Size(546, 476);
            dataGridView1.TabIndex = 5;
            // 
            // Номер
            // 
            Номер.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Номер.HeaderText = "#";
            Номер.Name = "Номер";
            Номер.ReadOnly = true;
            // 
            // Артикул
            // 
            Артикул.HeaderText = "Артикул";
            Артикул.Name = "Артикул";
            Артикул.ReadOnly = true;
            // 
            // Цена_с_картой
            // 
            Цена_с_картой.HeaderText = "Цена (с картой)";
            Цена_с_картой.Name = "Цена_с_картой";
            Цена_с_картой.ReadOnly = true;
            Цена_с_картой.Width = 130;
            // 
            // Цена
            // 
            Цена.HeaderText = "Цена";
            Цена.Name = "Цена";
            Цена.ReadOnly = true;
            Цена.Width = 130;
            // 
            // Цена_старая
            // 
            Цена_старая.HeaderText = "Цена (старая)";
            Цена_старая.Name = "Цена_старая";
            Цена_старая.ReadOnly = true;
            Цена_старая.Width = 130;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtHistoryText);
            groupBox1.Location = new Point(12, 88);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(294, 506);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "История действий (пока пусто)";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dataGridView1);
            groupBox2.Location = new Point(312, 88);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(558, 506);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Результат парсинга";
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Location = new Point(-601, 273);
            webView21.Name = "webView21";
            webView21.Size = new Size(613, 421);
            webView21.Source = new Uri("https://www.ozon.ru/product/1746727978", UriKind.Absolute);
            webView21.TabIndex = 9;
            webView21.ZoomFactor = 1D;
            // 
            // btnStartParse
            // 
            btnStartParse.BackColor = Color.Teal;
            btnStartParse.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 192, 192);
            btnStartParse.FlatStyle = FlatStyle.Flat;
            btnStartParse.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btnStartParse.ForeColor = Color.White;
            btnStartParse.Location = new Point(684, 46);
            btnStartParse.Name = "btnStartParse";
            btnStartParse.Size = new Size(180, 31);
            btnStartParse.TabIndex = 7;
            btnStartParse.Text = "Запустить парсер сейчас";
            btnStartParse.UseVisualStyleBackColor = false;
            btnStartParse.Click += btnStartParse_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "parser OZON";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // button1
            // 
            button1.Location = new Point(684, 9);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 8;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(881, 606);
            Controls.Add(webView21);
            Controls.Add(button1);
            Controls.Add(btnStartParse);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(linkLabel1);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "Парсер OZON";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            Resize += Form1_Resize;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private LinkLabel linkLabel1;
        private TextBox txtHistoryText;
        private DataGridView dataGridView1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button btnStartParse;
        private DataGridViewTextBoxColumn Номер;
        private DataGridViewTextBoxColumn Артикул;
        private DataGridViewTextBoxColumn Цена_с_картой;
        private DataGridViewTextBoxColumn Цена;
        private DataGridViewTextBoxColumn Цена_старая;
        private NotifyIcon notifyIcon1;
        private System.Windows.Forms.Timer timer1;
        private Button button1;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
    }
}
