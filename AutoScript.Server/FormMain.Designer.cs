namespace AutoScript.Server
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            dgvList = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            BtnImport = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            tabPage2 = new TabPage();
            openFileDialog = new OpenFileDialog();
            BtnRefresh = new Button();
            txtScript = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            btnStartAll = new Button();
            btnStopAll = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvList).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(3, -2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(631, 512);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dgvList);
            tabPage1.Controls.Add(BtnImport);
            tabPage1.Controls.Add(textBox1);
            tabPage1.Controls.Add(label1);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(623, 482);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "主控窗口";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvList
            // 
            dgvList.AllowUserToAddRows = false;
            dgvList.AllowUserToDeleteRows = false;
            dgvList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvList.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column6, Column4, Column5 });
            dgvList.Location = new Point(6, 68);
            dgvList.Name = "dgvList";
            dgvList.ReadOnly = true;
            dgvList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvList.Size = new Size(624, 409);
            dgvList.TabIndex = 3;
            dgvList.CellContentClick += dgvList_CellContentClick;
            // 
            // Column1
            // 
            Column1.HeaderText = "用户名";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            // 
            // Column2
            // 
            Column2.HeaderText = "密码";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // Column3
            // 
            Column3.HeaderText = "设备名称";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            // 
            // Column6
            // 
            Column6.HeaderText = "窗口句柄";
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            // 
            // Column4
            // 
            Column4.HeaderText = "当前脚本";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            // 
            // Column5
            // 
            Column5.HeaderText = "运行状态";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            // 
            // BtnImport
            // 
            BtnImport.Location = new Point(522, 25);
            BtnImport.Name = "BtnImport";
            BtnImport.Size = new Size(99, 26);
            BtnImport.TabIndex = 2;
            BtnImport.Text = "导入账号文件";
            BtnImport.UseVisualStyleBackColor = true;
            BtnImport.Click += BtnImport_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(71, 25);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(445, 23);
            textBox1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 29);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 0;
            label1.Text = "账号文件";
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(623, 482);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "设置";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            // 
            // BtnRefresh
            // 
            BtnRefresh.Location = new Point(640, 25);
            BtnRefresh.Name = "BtnRefresh";
            BtnRefresh.Size = new Size(87, 26);
            BtnRefresh.TabIndex = 1;
            BtnRefresh.Text = "刷新设备";
            BtnRefresh.UseVisualStyleBackColor = true;
            BtnRefresh.Click += BtnRefresh_Click;
            // 
            // txtScript
            // 
            txtScript.Location = new Point(636, 93);
            txtScript.Multiline = true;
            txtScript.Name = "txtScript";
            txtScript.Size = new Size(161, 186);
            txtScript.TabIndex = 2;
            txtScript.Text = "打宝图\r\n秘境降妖";
            txtScript.TextChanged += txtScript_TextChanged;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(641, 388);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 26);
            btnStart.TabIndex = 3;
            btnStart.Text = "启动";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(722, 388);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 26);
            btnStop.TabIndex = 4;
            btnStop.Text = "停止";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnStartAll
            // 
            btnStartAll.Location = new Point(643, 420);
            btnStartAll.Name = "btnStartAll";
            btnStartAll.Size = new Size(75, 26);
            btnStartAll.TabIndex = 5;
            btnStartAll.Text = "启动全部";
            btnStartAll.UseVisualStyleBackColor = true;
            btnStartAll.Click += btnStartAll_Click;
            // 
            // btnStopAll
            // 
            btnStopAll.Location = new Point(722, 420);
            btnStopAll.Name = "btnStopAll";
            btnStopAll.Size = new Size(75, 26);
            btnStopAll.TabIndex = 6;
            btnStopAll.Text = "停止全部";
            btnStopAll.UseVisualStyleBackColor = true;
            btnStopAll.Click += btnStopAll_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 510);
            Controls.Add(btnStopAll);
            Controls.Add(btnStartAll);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(txtScript);
            Controls.Add(BtnRefresh);
            Controls.Add(tabControl1);
            Name = "FormMain";
            Text = "FormMain";
            Load += FormMain_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvList).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button BtnImport;
        private TextBox textBox1;
        private Label label1;
        private DataGridView dgvList;
        private OpenFileDialog openFileDialog;
        private Button BtnRefresh;
        private TextBox txtScript;
        private Button btnStart;
        private Button btnStop;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private Button btnStartAll;
        private Button btnStopAll;
    }
}