namespace JavDB.Polling
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
            contextMenuStrip1 = new ContextMenuStrip(components);
            menuItemPoster = new ToolStripMenuItem();
            menuItemPreviewVideo = new ToolStripMenuItem();
            menuItemMagent = new ToolStripMenuItem();
            menuItemActor = new ToolStripMenuItem();
            menuItemCopy = new ToolStripMenuItem();
            menuItemCopyUID = new ToolStripMenuItem();
            menuItemCopyActor = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            btnNextBatch = new Button();
            label2 = new Label();
            label1 = new Label();
            btnStartPollingSeries = new Button();
            txtEnd = new TextBox();
            txtStart = new TextBox();
            numSize = new NumericUpDown();
            txtSeries = new TextBox();
            tabPage2 = new TabPage();
            txtPageValue = new TextBox();
            btnLoadPage = new Button();
            lblPage = new Label();
            btnPageDown = new Button();
            btnPageUp = new Button();
            label5 = new Label();
            txtPage = new TextBox();
            txtURL = new TextBox();
            tabPage3 = new TabPage();
            btnSortScore = new Button();
            lblFileCount = new Label();
            btnStartPollingFile = new Button();
            btnLoadFile = new Button();
            label7 = new Label();
            txtFileName = new TextBox();
            numScore = new NumericUpDown();
            label4 = new Label();
            rbFliterScore = new RadioButton();
            rbSimple = new RadioButton();
            rbStandard = new RadioButton();
            label3 = new Label();
            numDelay = new NumericUpDown();
            progressBar1 = new ProgressBar();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader9 = new ColumnHeader();
            columnHeader8 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            columnHeader7 = new ColumnHeader();
            openFileDialog1 = new OpenFileDialog();
            listInfo = new ListViewEx();
            groupBox1 = new GroupBox();
            label6 = new Label();
            numExpirationTime = new NumericUpDown();
            btnSaveUIDList = new Button();
            saveFileDialog1 = new SaveFileDialog();
            menuItemCopyJson = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSize).BeginInit();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numScore).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDelay).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numExpirationTime).BeginInit();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { menuItemPoster, menuItemPreviewVideo, menuItemMagent, menuItemActor, menuItemCopy });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(181, 136);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // menuItemPoster
            // 
            menuItemPoster.Name = "menuItemPoster";
            menuItemPoster.Size = new Size(180, 22);
            menuItemPoster.Text = "海报";
            menuItemPoster.Click += menuItemPoster_Click;
            // 
            // menuItemPreviewVideo
            // 
            menuItemPreviewVideo.Name = "menuItemPreviewVideo";
            menuItemPreviewVideo.Size = new Size(180, 22);
            menuItemPreviewVideo.Text = "视频预览";
            menuItemPreviewVideo.Click += menuItemPreviewVideo_Click;
            // 
            // menuItemMagent
            // 
            menuItemMagent.Name = "menuItemMagent";
            menuItemMagent.Size = new Size(180, 22);
            menuItemMagent.Text = "磁力链接";
            menuItemMagent.Click += menuItemMagent_Click;
            // 
            // menuItemActor
            // 
            menuItemActor.Name = "menuItemActor";
            menuItemActor.Size = new Size(180, 22);
            menuItemActor.Text = "按主演搜索";
            // 
            // menuItemCopy
            // 
            menuItemCopy.DropDownItems.AddRange(new ToolStripItem[] { menuItemCopyUID, menuItemCopyActor, menuItemCopyJson });
            menuItemCopy.Name = "menuItemCopy";
            menuItemCopy.Size = new Size(180, 22);
            menuItemCopy.Text = "复制";
            // 
            // menuItemCopyUID
            // 
            menuItemCopyUID.Name = "menuItemCopyUID";
            menuItemCopyUID.Size = new Size(180, 22);
            menuItemCopyUID.Text = "番号";
            menuItemCopyUID.Click += menuItemCopyUID_Click;
            // 
            // menuItemCopyActor
            // 
            menuItemCopyActor.Name = "menuItemCopyActor";
            menuItemCopyActor.Size = new Size(180, 22);
            menuItemCopyActor.Text = "主演";
            menuItemCopyActor.Click += menuItemCopyActor_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(5, 79);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1396, 74);
            tabControl1.TabIndex = 7;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(btnNextBatch);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(btnStartPollingSeries);
            tabPage1.Controls.Add(txtEnd);
            tabPage1.Controls.Add(txtStart);
            tabPage1.Controls.Add(numSize);
            tabPage1.Controls.Add(txtSeries);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1388, 44);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "按系列";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnNextBatch
            // 
            btnNextBatch.Location = new Point(353, 8);
            btnNextBatch.Name = "btnNextBatch";
            btnNextBatch.Size = new Size(29, 27);
            btnNextBatch.TabIndex = 15;
            btnNextBatch.Text = ">";
            btnNextBatch.UseVisualStyleBackColor = true;
            btnNextBatch.Click += btnNextBatch_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(177, 13);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 14;
            label2.Text = "范围：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 13);
            label1.Name = "label1";
            label1.Size = new Size(44, 17);
            label1.TabIndex = 13;
            label1.Text = "系列：";
            // 
            // btnStartPollingSeries
            // 
            btnStartPollingSeries.Location = new Point(388, 8);
            btnStartPollingSeries.Name = "btnStartPollingSeries";
            btnStartPollingSeries.Size = new Size(102, 27);
            btnStartPollingSeries.TabIndex = 12;
            btnStartPollingSeries.Text = "开始抓取";
            btnStartPollingSeries.UseVisualStyleBackColor = true;
            btnStartPollingSeries.Click += btnStartPolling_Click;
            // 
            // txtEnd
            // 
            txtEnd.Location = new Point(290, 10);
            txtEnd.Name = "txtEnd";
            txtEnd.Size = new Size(57, 23);
            txtEnd.TabIndex = 10;
            txtEnd.Text = "100";
            // 
            // txtStart
            // 
            txtStart.Location = new Point(227, 10);
            txtStart.Name = "txtStart";
            txtStart.Size = new Size(57, 23);
            txtStart.TabIndex = 9;
            txtStart.Text = "1";
            // 
            // numSize
            // 
            numSize.Location = new Point(123, 10);
            numSize.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            numSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numSize.Name = "numSize";
            numSize.Size = new Size(34, 23);
            numSize.TabIndex = 8;
            numSize.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // txtSeries
            // 
            txtSeries.CharacterCasing = CharacterCasing.Upper;
            txtSeries.Location = new Point(60, 10);
            txtSeries.Name = "txtSeries";
            txtSeries.Size = new Size(57, 23);
            txtSeries.TabIndex = 7;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(txtPageValue);
            tabPage2.Controls.Add(btnLoadPage);
            tabPage2.Controls.Add(lblPage);
            tabPage2.Controls.Add(btnPageDown);
            tabPage2.Controls.Add(btnPageUp);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(txtPage);
            tabPage2.Controls.Add(txtURL);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1388, 44);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "按地址";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtPageValue
            // 
            txtPageValue.Location = new Point(1324, 10);
            txtPageValue.MaxLength = 2;
            txtPageValue.Name = "txtPageValue";
            txtPageValue.Size = new Size(33, 23);
            txtPageValue.TabIndex = 20;
            txtPageValue.Text = "1";
            txtPageValue.TextAlign = HorizontalAlignment.Center;
            txtPageValue.KeyDown += txtPageValue_KeyDown;
            // 
            // btnLoadPage
            // 
            btnLoadPage.Enabled = false;
            btnLoadPage.Location = new Point(537, 8);
            btnLoadPage.Name = "btnLoadPage";
            btnLoadPage.Size = new Size(26, 27);
            btnLoadPage.TabIndex = 19;
            btnLoadPage.Text = "√";
            btnLoadPage.UseVisualStyleBackColor = true;
            btnLoadPage.Click += btnLoadPage_Click;
            // 
            // lblPage
            // 
            lblPage.Location = new Point(1249, 13);
            lblPage.Name = "lblPage";
            lblPage.Size = new Size(133, 17);
            lblPage.TabIndex = 18;
            lblPage.Text = "第            页";
            lblPage.TextAlign = ContentAlignment.TopRight;
            // 
            // btnPageDown
            // 
            btnPageDown.Enabled = false;
            btnPageDown.Location = new Point(677, 8);
            btnPageDown.Name = "btnPageDown";
            btnPageDown.Size = new Size(102, 27);
            btnPageDown.TabIndex = 17;
            btnPageDown.Text = "下一页";
            btnPageDown.UseVisualStyleBackColor = true;
            btnPageDown.Click += btnPageDown_Click;
            // 
            // btnPageUp
            // 
            btnPageUp.Enabled = false;
            btnPageUp.Location = new Point(569, 8);
            btnPageUp.Name = "btnPageUp";
            btnPageUp.Size = new Size(102, 27);
            btnPageUp.TabIndex = 16;
            btnPageUp.Text = "上一页";
            btnPageUp.UseVisualStyleBackColor = true;
            btnPageUp.Click += btnPageUp_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(10, 13);
            label5.Name = "label5";
            label5.Size = new Size(44, 17);
            label5.TabIndex = 15;
            label5.Text = "地址：";
            // 
            // txtPage
            // 
            txtPage.Location = new Point(212, 10);
            txtPage.Name = "txtPage";
            txtPage.Size = new Size(319, 23);
            txtPage.TabIndex = 14;
            txtPage.TextChanged += txtPage_TextChanged;
            txtPage.KeyDown += txtPage_KeyDown;
            // 
            // txtURL
            // 
            txtURL.Location = new Point(60, 10);
            txtURL.Name = "txtURL";
            txtURL.ReadOnly = true;
            txtURL.Size = new Size(154, 23);
            txtURL.TabIndex = 21;
            txtURL.Text = "https://javdb.com";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(btnSortScore);
            tabPage3.Controls.Add(lblFileCount);
            tabPage3.Controls.Add(btnStartPollingFile);
            tabPage3.Controls.Add(btnLoadFile);
            tabPage3.Controls.Add(label7);
            tabPage3.Controls.Add(txtFileName);
            tabPage3.Location = new Point(4, 26);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1388, 44);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "按文件";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnSortScore
            // 
            btnSortScore.Enabled = false;
            btnSortScore.Location = new Point(598, 8);
            btnSortScore.Name = "btnSortScore";
            btnSortScore.Size = new Size(102, 27);
            btnSortScore.TabIndex = 24;
            btnSortScore.Text = "按评分排序";
            btnSortScore.UseVisualStyleBackColor = true;
            btnSortScore.Click += btnSortScore_Click;
            // 
            // lblFileCount
            // 
            lblFileCount.Location = new Point(1252, 13);
            lblFileCount.Name = "lblFileCount";
            lblFileCount.Size = new Size(133, 17);
            lblFileCount.TabIndex = 23;
            lblFileCount.Text = "共 0 条";
            lblFileCount.TextAlign = ContentAlignment.TopRight;
            // 
            // btnStartPollingFile
            // 
            btnStartPollingFile.Enabled = false;
            btnStartPollingFile.Location = new Point(479, 8);
            btnStartPollingFile.Name = "btnStartPollingFile";
            btnStartPollingFile.Size = new Size(102, 27);
            btnStartPollingFile.TabIndex = 22;
            btnStartPollingFile.Text = "开始";
            btnStartPollingFile.UseVisualStyleBackColor = true;
            btnStartPollingFile.Click += btnStartPollingFile_Click;
            // 
            // btnLoadFile
            // 
            btnLoadFile.Location = new Point(435, 8);
            btnLoadFile.Name = "btnLoadFile";
            btnLoadFile.Size = new Size(26, 27);
            btnLoadFile.TabIndex = 21;
            btnLoadFile.Text = "...";
            btnLoadFile.UseVisualStyleBackColor = true;
            btnLoadFile.Click += btnLoadFile_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(8, 13);
            label7.Name = "label7";
            label7.Size = new Size(80, 17);
            label7.TabIndex = 20;
            label7.Text = "请选择文件：";
            // 
            // txtFileName
            // 
            txtFileName.Location = new Point(94, 10);
            txtFileName.Name = "txtFileName";
            txtFileName.Size = new Size(335, 23);
            txtFileName.TabIndex = 19;
            txtFileName.TextChanged += txtFileName_TextChanged;
            // 
            // numScore
            // 
            numScore.DecimalPlaces = 1;
            numScore.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numScore.Location = new Point(594, 22);
            numScore.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numScore.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numScore.Name = "numScore";
            numScore.Size = new Size(56, 23);
            numScore.TabIndex = 22;
            numScore.Value = new decimal(new int[] { 80, 0, 0, 65536 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(175, 25);
            label4.Name = "label4";
            label4.Size = new Size(68, 17);
            label4.TabIndex = 21;
            label4.Text = "抓取选项：";
            // 
            // rbFliterScore
            // 
            rbFliterScore.AutoSize = true;
            rbFliterScore.Location = new Point(406, 23);
            rbFliterScore.Name = "rbFliterScore";
            rbFliterScore.Size = new Size(182, 21);
            rbFliterScore.TabIndex = 20;
            rbFliterScore.Text = "简略抓取评分低于要求的影片";
            rbFliterScore.UseVisualStyleBackColor = true;
            rbFliterScore.CheckedChanged += rbFliterScore_CheckedChanged;
            // 
            // rbSimple
            // 
            rbSimple.AutoSize = true;
            rbSimple.Location = new Point(326, 23);
            rbSimple.Name = "rbSimple";
            rbSimple.Size = new Size(74, 21);
            rbSimple.TabIndex = 19;
            rbSimple.Text = "简略模式";
            rbSimple.UseVisualStyleBackColor = true;
            // 
            // rbStandard
            // 
            rbStandard.AutoSize = true;
            rbStandard.Checked = true;
            rbStandard.Location = new Point(246, 23);
            rbStandard.Name = "rbStandard";
            rbStandard.Size = new Size(74, 21);
            rbStandard.TabIndex = 18;
            rbStandard.TabStop = true;
            rbStandard.Text = "标准模式";
            rbStandard.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(13, 25);
            label3.Name = "label3";
            label3.Size = new Size(69, 17);
            label3.TabIndex = 15;
            label3.Text = "延迟(ms)：";
            // 
            // numDelay
            // 
            numDelay.Location = new Point(88, 22);
            numDelay.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numDelay.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numDelay.Name = "numDelay";
            numDelay.Size = new Size(64, 23);
            numDelay.TabIndex = 11;
            numDelay.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(5, 695);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(1283, 31);
            progressBar1.TabIndex = 16;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "序号";
            columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "番号";
            columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "状态";
            columnHeader3.Width = 70;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "评分";
            columnHeader4.Width = 50;
            // 
            // columnHeader9
            // 
            columnHeader9.Text = "发布日期";
            columnHeader9.Width = 90;
            // 
            // columnHeader8
            // 
            columnHeader8.Text = "标题";
            columnHeader8.Width = 360;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "主演";
            columnHeader5.Width = 200;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "地址";
            columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "标签";
            columnHeader7.Width = 360;
            // 
            // listInfo
            // 
            listInfo.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader9, columnHeader8, columnHeader5, columnHeader6, columnHeader7 });
            listInfo.ContextMenuStrip = contextMenuStrip1;
            listInfo.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            listInfo.FullRowSelect = true;
            listInfo.GridLines = true;
            listInfo.Location = new Point(5, 159);
            listInfo.MultiSelect = false;
            listInfo.Name = "listInfo";
            listInfo.ShowItemToolTips = true;
            listInfo.Size = new Size(1396, 530);
            listInfo.TabIndex = 17;
            listInfo.UseCompatibleStateImageBehavior = false;
            listInfo.View = View.Details;
            listInfo.DoubleClick += listInfo_DoubleClick;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(numExpirationTime);
            groupBox1.Controls.Add(numScore);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(rbFliterScore);
            groupBox1.Controls.Add(rbSimple);
            groupBox1.Controls.Add(rbStandard);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(numDelay);
            groupBox1.Location = new Point(5, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1396, 61);
            groupBox1.TabIndex = 18;
            groupBox1.TabStop = false;
            groupBox1.Text = "参数设置";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(677, 25);
            label6.Name = "label6";
            label6.Size = new Size(127, 17);
            label6.TabIndex = 24;
            label6.Text = "数据失效时间(hour)：";
            // 
            // numExpirationTime
            // 
            numExpirationTime.Location = new Point(810, 22);
            numExpirationTime.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numExpirationTime.Minimum = new decimal(new int[] { 24, 0, 0, 0 });
            numExpirationTime.Name = "numExpirationTime";
            numExpirationTime.Size = new Size(64, 23);
            numExpirationTime.TabIndex = 23;
            numExpirationTime.Value = new decimal(new int[] { 720, 0, 0, 0 });
            // 
            // btnSaveUIDList
            // 
            btnSaveUIDList.Location = new Point(1294, 695);
            btnSaveUIDList.Name = "btnSaveUIDList";
            btnSaveUIDList.Size = new Size(107, 31);
            btnSaveUIDList.TabIndex = 22;
            btnSaveUIDList.Text = "保存番号列表";
            btnSaveUIDList.UseVisualStyleBackColor = true;
            btnSaveUIDList.Click += btnSaveUIDList_Click;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.FileName = "无标题";
            saveFileDialog1.OverwritePrompt = false;
            // 
            // menuItemCopyJson
            // 
            menuItemCopyJson.Name = "menuItemCopyJson";
            menuItemCopyJson.Size = new Size(180, 22);
            menuItemCopyJson.Text = "Json";
            menuItemCopyJson.Click += menuItemCopyJson_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1413, 731);
            Controls.Add(btnSaveUIDList);
            Controls.Add(groupBox1);
            Controls.Add(listInfo);
            Controls.Add(tabControl1);
            Controls.Add(progressBar1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "JavDB.Polling";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSize).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numScore).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDelay).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numExpirationTime).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem menuItemPoster;
        private ToolStripMenuItem menuItemPreviewVideo;
        private ToolStripMenuItem menuItemMagent;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private NumericUpDown numScore;
        private Label label4;
        private RadioButton rbFliterScore;
        private RadioButton rbSimple;
        private RadioButton rbStandard;
        private ProgressBar progressBar1;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btnStartPollingSeries;
        private NumericUpDown numDelay;
        private TextBox txtEnd;
        private TextBox txtStart;
        private NumericUpDown numSize;
        private TextBox txtSeries;
        private ListViewEx listInfo;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader9;
        private ColumnHeader columnHeader8;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private Label label5;
        private TextBox txtPage;
        private Button btnPageUp;
        private Button btnPageDown;
        private ToolStripMenuItem menuItemActor;
        private Label lblPage;
        private TabPage tabPage3;
        private Label lblFileCount;
        private Button btnStartPollingFile;
        private Button btnLoadFile;
        private Label label7;
        private TextBox txtFileName;
        private OpenFileDialog openFileDialog1;
        private Button btnSortScore;
        private Button btnLoadPage;
        private GroupBox groupBox1;
        private Button btnSaveUIDList;
        private SaveFileDialog saveFileDialog1;
        private ToolStripMenuItem menuItemCopy;
        private ToolStripMenuItem menuItemCopyUID;
        private ToolStripMenuItem menuItemCopyActor;
        private TextBox txtPageValue;
        private Button btnNextBatch;
        private Label label6;
        private NumericUpDown numExpirationTime;
        private TextBox txtURL;
        private ToolStripMenuItem menuItemCopyJson;
    }
}