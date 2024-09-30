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
            menuItemIndex = new ToolStripMenuItem();
            menuItemBackdrop = new ToolStripMenuItem();
            menuItemPreviewVideo = new ToolStripMenuItem();
            menuItemMagent = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            menuItemSeriesNumber = new ToolStripMenuItem();
            menuItemActor = new ToolStripMenuItem();
            menuItemCopy = new ToolStripMenuItem();
            menuItemCopyUID = new ToolStripMenuItem();
            menuItemCopyActor = new ToolStripMenuItem();
            menuItemCopyJson = new ToolStripMenuItem();
            menuItemFilter = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            tabPage3 = new TabPage();
            btnStartPollingFile = new Button();
            btnLoadFile = new Button();
            label7 = new Label();
            txtFileName = new TextBox();
            tabPage2 = new TabPage();
            txtPageValue = new TextBox();
            btnLoadPage = new Button();
            lblPage = new Label();
            btnPageDown = new Button();
            btnPageUp = new Button();
            label5 = new Label();
            txtPageManual = new TextBox();
            txtURL = new TextBox();
            tabPage4 = new TabPage();
            btnLoadActorUrl = new Button();
            label8 = new Label();
            txtPageAuto = new TextBox();
            txtURL2 = new TextBox();
            btnSortScore = new Button();
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
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolStripSeparator1 = new ToolStripSeparator();
            statusLabel = new ToolStripStatusLabel();
            toolStripSeparator2 = new ToolStripSeparator();
            lblCount = new ToolStripStatusLabel();
            contextMenuStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numScore).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDelay).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numExpirationTime).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { menuItemIndex, menuItemBackdrop, menuItemPreviewVideo, menuItemMagent, toolStripMenuItem1, menuItemSeriesNumber, menuItemActor, menuItemCopy, menuItemFilter });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(181, 208);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // menuItemIndex
            // 
            menuItemIndex.Name = "menuItemIndex";
            menuItemIndex.Size = new Size(180, 22);
            menuItemIndex.Text = "主页";
            menuItemIndex.Click += menuItemIndex_Click;
            // 
            // menuItemBackdrop
            // 
            menuItemBackdrop.Name = "menuItemBackdrop";
            menuItemBackdrop.Size = new Size(180, 22);
            menuItemBackdrop.Text = "海报";
            menuItemBackdrop.Click += menuItemBackdrop_Click;
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
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(177, 6);
            // 
            // menuItemSeriesNumber
            // 
            menuItemSeriesNumber.Name = "menuItemSeriesNumber";
            menuItemSeriesNumber.Size = new Size(180, 22);
            menuItemSeriesNumber.Text = "按系列搜索";
            menuItemSeriesNumber.Click += menuItemSeriesNumber_Click;
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
            menuItemCopyUID.Size = new Size(102, 22);
            menuItemCopyUID.Text = "番号";
            menuItemCopyUID.Click += menuItemCopyUID_Click;
            // 
            // menuItemCopyActor
            // 
            menuItemCopyActor.Name = "menuItemCopyActor";
            menuItemCopyActor.Size = new Size(102, 22);
            menuItemCopyActor.Text = "主演";
            menuItemCopyActor.Click += menuItemCopyActor_Click;
            // 
            // menuItemCopyJson
            // 
            menuItemCopyJson.Name = "menuItemCopyJson";
            menuItemCopyJson.Size = new Size(102, 22);
            menuItemCopyJson.Text = "Json";
            menuItemCopyJson.Click += menuItemCopyJson_Click;
            // 
            // menuItemFilter
            // 
            menuItemFilter.Name = "menuItemFilter";
            menuItemFilter.Size = new Size(180, 22);
            menuItemFilter.Text = "筛选";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Location = new Point(5, 79);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1396, 74);
            tabControl1.TabIndex = 7;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabPage3
            // 
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
            // tabPage2
            // 
            tabPage2.Controls.Add(txtPageValue);
            tabPage2.Controls.Add(btnLoadPage);
            tabPage2.Controls.Add(lblPage);
            tabPage2.Controls.Add(btnPageDown);
            tabPage2.Controls.Add(btnPageUp);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(txtPageManual);
            tabPage2.Controls.Add(txtURL);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1388, 44);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "按地址(手动)";
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
            // txtPageManual
            // 
            txtPageManual.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtPageManual.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtPageManual.Location = new Point(212, 10);
            txtPageManual.Name = "txtPageManual";
            txtPageManual.Size = new Size(319, 23);
            txtPageManual.TabIndex = 14;
            txtPageManual.TextChanged += txtPageManual_TextChanged;
            txtPageManual.KeyDown += txtPage_KeyDown;
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
            // tabPage4
            // 
            tabPage4.Controls.Add(btnLoadActorUrl);
            tabPage4.Controls.Add(label8);
            tabPage4.Controls.Add(txtPageAuto);
            tabPage4.Controls.Add(txtURL2);
            tabPage4.Location = new Point(4, 26);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(1388, 44);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "按地址(自动)";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnLoadActorUrl
            // 
            btnLoadActorUrl.Location = new Point(537, 8);
            btnLoadActorUrl.Name = "btnLoadActorUrl";
            btnLoadActorUrl.Size = new Size(26, 27);
            btnLoadActorUrl.TabIndex = 26;
            btnLoadActorUrl.Text = "√";
            btnLoadActorUrl.UseVisualStyleBackColor = true;
            btnLoadActorUrl.Click += btnLoadActorUrl_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(10, 13);
            label8.Name = "label8";
            label8.Size = new Size(44, 17);
            label8.TabIndex = 23;
            label8.Text = "地址：";
            // 
            // txtPageAuto
            // 
            txtPageAuto.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtPageAuto.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtPageAuto.Location = new Point(212, 10);
            txtPageAuto.Name = "txtPageAuto";
            txtPageAuto.Size = new Size(319, 23);
            txtPageAuto.TabIndex = 22;
            txtPageAuto.TextChanged += txtPageAuto_TextChanged;
            // 
            // txtURL2
            // 
            txtURL2.Location = new Point(60, 10);
            txtURL2.Name = "txtURL2";
            txtURL2.ReadOnly = true;
            txtURL2.Size = new Size(154, 23);
            txtURL2.TabIndex = 27;
            txtURL2.Text = "https://javdb.com";
            // 
            // btnSortScore
            // 
            btnSortScore.Location = new Point(1186, 695);
            btnSortScore.Name = "btnSortScore";
            btnSortScore.Size = new Size(102, 31);
            btnSortScore.TabIndex = 24;
            btnSortScore.Text = "按评分排序";
            btnSortScore.UseVisualStyleBackColor = true;
            btnSortScore.Click += btnSortScore_Click;
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
            progressBar1.Size = new Size(1175, 31);
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
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, toolStripSeparator1, statusLabel, toolStripSeparator2, lblCount });
            statusStrip1.Location = new Point(0, 731);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1413, 23);
            statusStrip1.TabIndex = 23;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(32, 18);
            toolStripStatusLabel1.Text = "状态";
            toolStripStatusLabel1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 23);
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(1307, 18);
            statusLabel.Spring = true;
            statusLabel.Text = "已就绪";
            statusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 23);
            // 
            // lblCount
            // 
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(47, 18);
            lblCount.Text = "共 0 条";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1413, 754);
            Controls.Add(btnSortScore);
            Controls.Add(statusStrip1);
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
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numScore).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDelay).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numExpirationTime).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem menuItemBackdrop;
        private ToolStripMenuItem menuItemPreviewVideo;
        private ToolStripMenuItem menuItemMagent;
        private TabControl tabControl1;
        private TabPage tabPage2;
        private NumericUpDown numScore;
        private Label label4;
        private RadioButton rbFliterScore;
        private RadioButton rbSimple;
        private RadioButton rbStandard;
        private ProgressBar progressBar1;
        private Label label3;
        private NumericUpDown numDelay;
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
        private TextBox txtPageManual;
        private Button btnPageUp;
        private Button btnPageDown;
        private ToolStripMenuItem menuItemActor;
        private Label lblPage;
        private TabPage tabPage3;
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
        private Label label6;
        private NumericUpDown numExpirationTime;
        private TextBox txtURL;
        private ToolStripMenuItem menuItemCopyJson;
        private ToolStripMenuItem menuItemIndex;
        private TabPage tabPage4;
        private Label label8;
        private TextBox txtURL2;
        private Button btnLoadActorUrl;
        private TextBox txtPageAuto;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripStatusLabel statusLabel;
        private ToolStripStatusLabel lblCount;
        private ToolStripMenuItem menuItemSeriesNumber;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem menuItemFilter;
    }
}