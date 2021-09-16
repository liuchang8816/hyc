namespace RasterDetectionMod.Controls
{
    partial class UcMilCamera
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tbpnlCamera = new System.Windows.Forms.TableLayoutPanel();
            this.pictImg = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkGrayTest = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtExposure = new System.Windows.Forms.TextBox();
            this.cboCamera = new System.Windows.Forms.ComboBox();
            this.txtGain = new System.Windows.Forms.TextBox();
            this.btnSetExposure = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.btnGrab = new System.Windows.Forms.Button();
            this.btnGetGain = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSetGain = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnImgTestAoi = new System.Windows.Forms.Button();
            this.btnGetExposure = new System.Windows.Forms.Button();
            this.chkBlack = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtCellAoi = new System.Windows.Forms.TextBox();
            this.txtGrayLevel = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbpnlCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictImg)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbpnlCamera
            // 
            this.tbpnlCamera.ColumnCount = 1;
            this.tbpnlCamera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbpnlCamera.Controls.Add(this.pictImg, 0, 1);
            this.tbpnlCamera.Controls.Add(this.panel1, 0, 0);
            this.tbpnlCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbpnlCamera.Location = new System.Drawing.Point(0, 0);
            this.tbpnlCamera.Name = "tbpnlCamera";
            this.tbpnlCamera.RowCount = 2;
            this.tbpnlCamera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tbpnlCamera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbpnlCamera.Size = new System.Drawing.Size(944, 615);
            this.tbpnlCamera.TabIndex = 140;
            // 
            // pictImg
            // 
            this.pictImg.BackColor = System.Drawing.Color.LightGray;
            this.pictImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictImg.Location = new System.Drawing.Point(0, 120);
            this.pictImg.Margin = new System.Windows.Forms.Padding(0);
            this.pictImg.Name = "pictImg";
            this.pictImg.Size = new System.Drawing.Size(944, 495);
            this.pictImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictImg.TabIndex = 123;
            this.pictImg.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkGrayTest);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtExposure);
            this.panel1.Controls.Add(this.cboCamera);
            this.panel1.Controls.Add(this.txtGain);
            this.panel1.Controls.Add(this.btnSetExposure);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.btnGrab);
            this.panel1.Controls.Add(this.btnGetGain);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnSetGain);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnImgTestAoi);
            this.panel1.Controls.Add(this.btnGetExposure);
            this.panel1.Controls.Add(this.chkBlack);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.txtCellAoi);
            this.panel1.Controls.Add(this.txtGrayLevel);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(944, 120);
            this.panel1.TabIndex = 124;
            // 
            // chkGrayTest
            // 
            this.chkGrayTest.AutoSize = true;
            this.chkGrayTest.Location = new System.Drawing.Point(305, 11);
            this.chkGrayTest.Name = "chkGrayTest";
            this.chkGrayTest.Size = new System.Drawing.Size(75, 21);
            this.chkGrayTest.TabIndex = 139;
            this.chkGrayTest.Text = "灰度测试";
            this.chkGrayTest.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 121;
            this.label1.Text = "相机选择：";
            // 
            // txtExposure
            // 
            this.txtExposure.Location = new System.Drawing.Point(60, 47);
            this.txtExposure.Name = "txtExposure";
            this.txtExposure.Size = new System.Drawing.Size(60, 23);
            this.txtExposure.TabIndex = 138;
            // 
            // cboCamera
            // 
            this.cboCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCamera.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboCamera.FormattingEnabled = true;
            this.cboCamera.Location = new System.Drawing.Point(83, 8);
            this.cboCamera.Name = "cboCamera";
            this.cboCamera.Size = new System.Drawing.Size(121, 25);
            this.cboCamera.TabIndex = 120;
            // 
            // txtGain
            // 
            this.txtGain.Location = new System.Drawing.Point(335, 48);
            this.txtGain.Name = "txtGain";
            this.txtGain.Size = new System.Drawing.Size(60, 23);
            this.txtGain.TabIndex = 137;
            // 
            // btnSetExposure
            // 
            this.btnSetExposure.Location = new System.Drawing.Point(209, 46);
            this.btnSetExposure.Name = "btnSetExposure";
            this.btnSetExposure.Size = new System.Drawing.Size(50, 25);
            this.btnSetExposure.TabIndex = 122;
            this.btnSetExposure.Text = "设定";
            this.btnSetExposure.UseVisualStyleBackColor = true;
            this.btnSetExposure.Click += new System.EventHandler(this.btnSetExposure_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(290, 51);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(44, 17);
            this.label16.TabIndex = 136;
            this.label16.Text = "增益：";
            // 
            // btnGrab
            // 
            this.btnGrab.Location = new System.Drawing.Point(215, 8);
            this.btnGrab.Name = "btnGrab";
            this.btnGrab.Size = new System.Drawing.Size(75, 25);
            this.btnGrab.TabIndex = 124;
            this.btnGrab.Text = "拍照";
            this.btnGrab.UseVisualStyleBackColor = true;
            this.btnGrab.Click += new System.EventHandler(this.btnGrab_Click);
            // 
            // btnGetGain
            // 
            this.btnGetGain.Location = new System.Drawing.Point(401, 47);
            this.btnGetGain.Name = "btnGetGain";
            this.btnGetGain.Size = new System.Drawing.Size(50, 25);
            this.btnGetGain.TabIndex = 135;
            this.btnGetGain.Text = "读取";
            this.btnGetGain.UseVisualStyleBackColor = true;
            this.btnGetGain.Click += new System.EventHandler(this.btnGetGain_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(122, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 17);
            this.label6.TabIndex = 125;
            this.label6.Text = "ms";
            // 
            // btnSetGain
            // 
            this.btnSetGain.Location = new System.Drawing.Point(457, 47);
            this.btnSetGain.Name = "btnSetGain";
            this.btnSetGain.Size = new System.Drawing.Size(50, 25);
            this.btnSetGain.TabIndex = 134;
            this.btnSetGain.Text = "设定";
            this.btnSetGain.UseVisualStyleBackColor = true;
            this.btnSetGain.Click += new System.EventHandler(this.btnSetGain_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 17);
            this.label7.TabIndex = 126;
            this.label7.Text = "曝光：";
            // 
            // btnImgTestAoi
            // 
            this.btnImgTestAoi.Location = new System.Drawing.Point(594, 87);
            this.btnImgTestAoi.Name = "btnImgTestAoi";
            this.btnImgTestAoi.Size = new System.Drawing.Size(75, 25);
            this.btnImgTestAoi.TabIndex = 133;
            this.btnImgTestAoi.Text = "图片测试";
            this.btnImgTestAoi.UseVisualStyleBackColor = true;
            this.btnImgTestAoi.Click += new System.EventHandler(this.btnImgTestAoi_Click);
            // 
            // btnGetExposure
            // 
            this.btnGetExposure.Location = new System.Drawing.Point(153, 46);
            this.btnGetExposure.Name = "btnGetExposure";
            this.btnGetExposure.Size = new System.Drawing.Size(50, 25);
            this.btnGetExposure.TabIndex = 127;
            this.btnGetExposure.Text = "读取";
            this.btnGetExposure.UseVisualStyleBackColor = true;
            this.btnGetExposure.Click += new System.EventHandler(this.btnGetExposure_Click);
            // 
            // chkBlack
            // 
            this.chkBlack.AutoSize = true;
            this.chkBlack.Location = new System.Drawing.Point(13, 90);
            this.chkBlack.Name = "chkBlack";
            this.chkBlack.Size = new System.Drawing.Size(63, 21);
            this.chkBlack.TabIndex = 132;
            this.chkBlack.Text = "黑画面";
            this.chkBlack.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(114, 91);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 17);
            this.label13.TabIndex = 128;
            this.label13.Text = "灰度值：";
            // 
            // txtCellAoi
            // 
            this.txtCellAoi.Location = new System.Drawing.Point(416, 88);
            this.txtCellAoi.Name = "txtCellAoi";
            this.txtCellAoi.ReadOnly = true;
            this.txtCellAoi.Size = new System.Drawing.Size(145, 23);
            this.txtCellAoi.TabIndex = 131;
            // 
            // txtGrayLevel
            // 
            this.txtGrayLevel.Location = new System.Drawing.Point(175, 88);
            this.txtGrayLevel.Name = "txtGrayLevel";
            this.txtGrayLevel.ReadOnly = true;
            this.txtGrayLevel.Size = new System.Drawing.Size(129, 23);
            this.txtGrayLevel.TabIndex = 129;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(323, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 17);
            this.label9.TabIndex = 130;
            this.label9.Text = "AOI(x,y,w,h)：";
            // 
            // UcMilCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbpnlCamera);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UcMilCamera";
            this.Size = new System.Drawing.Size(944, 615);
            this.tbpnlCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictImg)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbpnlCamera;
        private System.Windows.Forms.PictureBox pictImg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkGrayTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExposure;
        private System.Windows.Forms.ComboBox cboCamera;
        private System.Windows.Forms.TextBox txtGain;
        private System.Windows.Forms.Button btnSetExposure;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnGrab;
        private System.Windows.Forms.Button btnGetGain;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSetGain;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnImgTestAoi;
        private System.Windows.Forms.Button btnGetExposure;
        private System.Windows.Forms.CheckBox chkBlack;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtCellAoi;
        private System.Windows.Forms.TextBox txtGrayLevel;
        private System.Windows.Forms.Label label9;
    }
}
