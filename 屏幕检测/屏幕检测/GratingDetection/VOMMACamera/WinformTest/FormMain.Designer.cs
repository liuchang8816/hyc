
namespace WinformTest
{
    partial class FormMain
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtExposureTime = new System.Windows.Forms.TextBox();
            this.btnGetExposureTime = new System.Windows.Forms.Button();
            this.btnSetExposureTime = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnStartAcquisition = new System.Windows.Forms.Button();
            this.btnStopAcquisition = new System.Windows.Forms.Button();
            this.btnGrab = new System.Windows.Forms.Button();
            this.pictImg = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboTriggerMode = new System.Windows.Forms.ComboBox();
            this.btnTriggerMode = new System.Windows.Forms.Button();
            this.btnGetDepthImg = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictImg)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(12, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(93, 13);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(216, 21);
            this.txtDesc.TabIndex = 1;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 41);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "曝光时间：";
            // 
            // txtExposureTime
            // 
            this.txtExposureTime.Location = new System.Drawing.Point(83, 145);
            this.txtExposureTime.Name = "txtExposureTime";
            this.txtExposureTime.Size = new System.Drawing.Size(94, 21);
            this.txtExposureTime.TabIndex = 4;
            // 
            // btnGetExposureTime
            // 
            this.btnGetExposureTime.Location = new System.Drawing.Point(198, 143);
            this.btnGetExposureTime.Name = "btnGetExposureTime";
            this.btnGetExposureTime.Size = new System.Drawing.Size(61, 23);
            this.btnGetExposureTime.TabIndex = 5;
            this.btnGetExposureTime.Text = "Get";
            this.btnGetExposureTime.UseVisualStyleBackColor = true;
            this.btnGetExposureTime.Click += new System.EventHandler(this.btnGetExposureTime_Click);
            // 
            // btnSetExposureTime
            // 
            this.btnSetExposureTime.Location = new System.Drawing.Point(265, 143);
            this.btnSetExposureTime.Name = "btnSetExposureTime";
            this.btnSetExposureTime.Size = new System.Drawing.Size(61, 23);
            this.btnSetExposureTime.TabIndex = 6;
            this.btnSetExposureTime.Text = "Set";
            this.btnSetExposureTime.UseVisualStyleBackColor = true;
            this.btnSetExposureTime.Click += new System.EventHandler(this.btnSetExposureTime_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(93, 41);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnStartAcquisition
            // 
            this.btnStartAcquisition.Location = new System.Drawing.Point(12, 70);
            this.btnStartAcquisition.Name = "btnStartAcquisition";
            this.btnStartAcquisition.Size = new System.Drawing.Size(156, 23);
            this.btnStartAcquisition.TabIndex = 8;
            this.btnStartAcquisition.Text = "StartAcquisition";
            this.btnStartAcquisition.UseVisualStyleBackColor = true;
            this.btnStartAcquisition.Click += new System.EventHandler(this.btnStartAcquisition_Click);
            // 
            // btnStopAcquisition
            // 
            this.btnStopAcquisition.Location = new System.Drawing.Point(12, 99);
            this.btnStopAcquisition.Name = "btnStopAcquisition";
            this.btnStopAcquisition.Size = new System.Drawing.Size(156, 23);
            this.btnStopAcquisition.TabIndex = 9;
            this.btnStopAcquisition.Text = "StopAcquisition";
            this.btnStopAcquisition.UseVisualStyleBackColor = true;
            this.btnStopAcquisition.Click += new System.EventHandler(this.btnStopAcquisition_Click);
            // 
            // btnGrab
            // 
            this.btnGrab.Location = new System.Drawing.Point(338, 11);
            this.btnGrab.Name = "btnGrab";
            this.btnGrab.Size = new System.Drawing.Size(61, 23);
            this.btnGrab.TabIndex = 10;
            this.btnGrab.Text = "Grab";
            this.btnGrab.UseVisualStyleBackColor = true;
            this.btnGrab.Click += new System.EventHandler(this.btnGrab_Click);
            // 
            // pictImg
            // 
            this.pictImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictImg.Location = new System.Drawing.Point(338, 43);
            this.pictImg.Name = "pictImg";
            this.pictImg.Size = new System.Drawing.Size(412, 284);
            this.pictImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictImg.TabIndex = 11;
            this.pictImg.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "触发模式：";
            // 
            // cboTriggerMode
            // 
            this.cboTriggerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTriggerMode.FormattingEnabled = true;
            this.cboTriggerMode.Items.AddRange(new object[] {
            "连续模式",
            "软触发"});
            this.cboTriggerMode.Location = new System.Drawing.Point(83, 184);
            this.cboTriggerMode.Name = "cboTriggerMode";
            this.cboTriggerMode.Size = new System.Drawing.Size(94, 20);
            this.cboTriggerMode.TabIndex = 13;
            // 
            // btnTriggerMode
            // 
            this.btnTriggerMode.Location = new System.Drawing.Point(198, 182);
            this.btnTriggerMode.Name = "btnTriggerMode";
            this.btnTriggerMode.Size = new System.Drawing.Size(61, 23);
            this.btnTriggerMode.TabIndex = 14;
            this.btnTriggerMode.Text = "Set";
            this.btnTriggerMode.UseVisualStyleBackColor = true;
            this.btnTriggerMode.Click += new System.EventHandler(this.btnTriggerMode_Click);
            // 
            // btnGetDepthImg
            // 
            this.btnGetDepthImg.Location = new System.Drawing.Point(405, 11);
            this.btnGetDepthImg.Name = "btnGetDepthImg";
            this.btnGetDepthImg.Size = new System.Drawing.Size(101, 23);
            this.btnGetDepthImg.TabIndex = 15;
            this.btnGetDepthImg.Text = "GetDepthImg";
            this.btnGetDepthImg.UseVisualStyleBackColor = true;
            this.btnGetDepthImg.Click += new System.EventHandler(this.btnGetDepthImg_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(512, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "SoftTrigger";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 442);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnGetDepthImg);
            this.Controls.Add(this.btnTriggerMode);
            this.Controls.Add(this.cboTriggerMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictImg);
            this.Controls.Add(this.btnGrab);
            this.Controls.Add(this.btnStopAcquisition);
            this.Controls.Add(this.btnStartAcquisition);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSetExposureTime);
            this.Controls.Add(this.btnGetExposureTime);
            this.Controls.Add(this.txtExposureTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.btnSearch);
            this.Name = "FormMain";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExposureTime;
        private System.Windows.Forms.Button btnGetExposureTime;
        private System.Windows.Forms.Button btnSetExposureTime;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnStartAcquisition;
        private System.Windows.Forms.Button btnStopAcquisition;
        private System.Windows.Forms.Button btnGrab;
        private System.Windows.Forms.PictureBox pictImg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboTriggerMode;
        private System.Windows.Forms.Button btnTriggerMode;
        private System.Windows.Forms.Button btnGetDepthImg;
        private System.Windows.Forms.Button button1;
    }
}

