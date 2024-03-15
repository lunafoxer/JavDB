namespace JavDB.Client
{
    partial class MainForm
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            btnCopy = new Button();
            picPoster = new PictureBox();
            picCover = new PictureBox();
            listInfo = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            contextMenuStrip1 = new ContextMenuStrip(components);
            menuItemOpen = new ToolStripMenuItem();
            btnOutputVsMeta = new Button();
            btnGrab = new Button();
            chbCacheFirst = new CheckBox();
            txtUID = new TextBox();
            tabPage2 = new TabPage();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            tabPage3 = new TabPage();
            webView22 = new Microsoft.Web.WebView2.WinForms.WebView2();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPoster).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCover).BeginInit();
            contextMenuStrip1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView22).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(8, 8);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1049, 607);
            tabControl1.TabIndex = 0;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(btnCopy);
            tabPage1.Controls.Add(picPoster);
            tabPage1.Controls.Add(picCover);
            tabPage1.Controls.Add(listInfo);
            tabPage1.Controls.Add(btnOutputVsMeta);
            tabPage1.Controls.Add(btnGrab);
            tabPage1.Controls.Add(chbCacheFirst);
            tabPage1.Controls.Add(txtUID);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1041, 577);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "基本信息";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            btnCopy.Enabled = false;
            btnCopy.Location = new Point(469, 4);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(40, 32);
            btnCopy.TabIndex = 7;
            btnCopy.Text = "复制";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            // 
            // picPoster
            // 
            picPoster.BackColor = Color.White;
            picPoster.BorderStyle = BorderStyle.FixedSingle;
            picPoster.InitialImage = Properties.Resources.loading2;
            picPoster.Location = new Point(515, 217);
            picPoster.Name = "picPoster";
            picPoster.Size = new Size(520, 352);
            picPoster.SizeMode = PictureBoxSizeMode.Zoom;
            picPoster.TabIndex = 6;
            picPoster.TabStop = false;
            // 
            // picCover
            // 
            picCover.BackColor = Color.White;
            picCover.BorderStyle = BorderStyle.FixedSingle;
            picCover.InitialImage = Properties.Resources.loading2;
            picCover.Location = new Point(702, 11);
            picCover.Name = "picCover";
            picCover.Size = new Size(147, 200);
            picCover.SizeMode = PictureBoxSizeMode.Zoom;
            picCover.TabIndex = 5;
            picCover.TabStop = false;
            // 
            // listInfo
            // 
            listInfo.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            listInfo.ContextMenuStrip = contextMenuStrip1;
            listInfo.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            listInfo.FullRowSelect = true;
            listInfo.GridLines = true;
            listInfo.Location = new Point(6, 42);
            listInfo.MultiSelect = false;
            listInfo.Name = "listInfo";
            listInfo.ShowItemToolTips = true;
            listInfo.Size = new Size(503, 527);
            listInfo.TabIndex = 4;
            listInfo.UseCompatibleStateImageBehavior = false;
            listInfo.View = View.Details;
            listInfo.DoubleClick += listInfo_DoubleClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "属性";
            columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "值";
            columnHeader2.Width = 350;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { menuItemOpen });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(125, 26);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // menuItemOpen
            // 
            menuItemOpen.Name = "menuItemOpen";
            menuItemOpen.Size = new Size(124, 22);
            menuItemOpen.Text = "打开链接";
            menuItemOpen.Click += menuItemOpen_Click;
            // 
            // btnOutputVsMeta
            // 
            btnOutputVsMeta.Enabled = false;
            btnOutputVsMeta.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnOutputVsMeta.Location = new Point(357, 4);
            btnOutputVsMeta.Name = "btnOutputVsMeta";
            btnOutputVsMeta.Size = new Size(106, 32);
            btnOutputVsMeta.TabIndex = 3;
            btnOutputVsMeta.Text = "输出VSMETA";
            btnOutputVsMeta.UseVisualStyleBackColor = true;
            btnOutputVsMeta.Click += btnOutputVsMeta_Click;
            // 
            // btnGrab
            // 
            btnGrab.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnGrab.Location = new Point(270, 4);
            btnGrab.Name = "btnGrab";
            btnGrab.Size = new Size(81, 32);
            btnGrab.TabIndex = 2;
            btnGrab.Text = "抓取";
            btnGrab.UseVisualStyleBackColor = true;
            btnGrab.Click += btnGrab_Click;
            // 
            // chbCacheFirst
            // 
            chbCacheFirst.AutoSize = true;
            chbCacheFirst.Checked = true;
            chbCacheFirst.CheckState = CheckState.Checked;
            chbCacheFirst.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            chbCacheFirst.Location = new Point(180, 8);
            chbCacheFirst.Name = "chbCacheFirst";
            chbCacheFirst.Size = new Size(84, 24);
            chbCacheFirst.TabIndex = 1;
            chbCacheFirst.Text = "缓存优先";
            chbCacheFirst.UseVisualStyleBackColor = true;
            // 
            // txtUID
            // 
            txtUID.CharacterCasing = CharacterCasing.Upper;
            txtUID.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtUID.Location = new Point(6, 6);
            txtUID.MaxLength = 32;
            txtUID.Name = "txtUID";
            txtUID.Size = new Size(168, 28);
            txtUID.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(webView21);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1041, 577);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "视频预览";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Location = new Point(6, 6);
            webView21.Name = "webView21";
            webView21.Size = new Size(1029, 565);
            webView21.TabIndex = 1;
            webView21.ZoomFactor = 1D;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(webView22);
            tabPage3.Location = new Point(4, 26);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1041, 577);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "磁力链接";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // webView22
            // 
            webView22.AllowExternalDrop = true;
            webView22.CreationProperties = null;
            webView22.DefaultBackgroundColor = Color.White;
            webView22.Location = new Point(6, 6);
            webView22.Name = "webView22";
            webView22.Size = new Size(1029, 565);
            webView22.TabIndex = 2;
            webView22.ZoomFactor = 1D;
            // 
            // MainForm
            // 
            AcceptButton = btnGrab;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1063, 621);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "JavDB";
            Activated += MainForm_Activated;
            FormClosed += MainForm_FormClosed;
            Shown += MainForm_Shown;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picPoster).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCover).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView22).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private Button btnGrab;
        private CheckBox chbCacheFirst;
        private TextBox txtUID;
        private Button btnOutputVsMeta;
        private ListView listInfo;
        private PictureBox picCover;
        private PictureBox picPoster;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView22;
        private Button btnCopy;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem menuItemOpen;
    }
}