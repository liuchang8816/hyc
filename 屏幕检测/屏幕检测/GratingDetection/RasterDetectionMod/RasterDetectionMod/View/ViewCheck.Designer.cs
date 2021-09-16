namespace RasterDetectionMod.View
{
    partial class ViewCheck
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ucMilCamera1 = new RasterDetectionMod.Controls.UcMilCamera();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.txtMAddress = new System.Windows.Forms.TextBox();
            this.txtDAddress = new System.Windows.Forms.TextBox();
            this.txtDValue = new System.Windows.Forms.TextBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.btnMWrite = new System.Windows.Forms.Button();
            this.btnMread = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnDWrite = new System.Windows.Forms.Button();
            this.label28 = new System.Windows.Forms.Label();
            this.btnDread = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ucPower1 = new RasterDetectionMod.Controls.UcPower();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ucMilCamera1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(974, 629);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "AOI相机";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ucMilCamera1
            // 
            this.ucMilCamera1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucMilCamera1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucMilCamera1.Location = new System.Drawing.Point(3, 3);
            this.ucMilCamera1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucMilCamera1.Name = "ucMilCamera1";
            this.ucMilCamera1.Size = new System.Drawing.Size(968, 623);
            this.ucMilCamera1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(1, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(982, 659);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.ucPower1);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(974, 629);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "其他";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Controls.Add(this.txtMAddress);
            this.panel1.Controls.Add(this.txtDAddress);
            this.panel1.Controls.Add(this.txtDValue);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.btnMWrite);
            this.panel1.Controls.Add(this.btnMread);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.btnDWrite);
            this.panel1.Controls.Add(this.label28);
            this.panel1.Controls.Add(this.btnDread);
            this.panel1.Controls.Add(this.label22);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Location = new System.Drawing.Point(23, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(550, 193);
            this.panel1.TabIndex = 45;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(43, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(62, 21);
            this.radioButton1.TabIndex = 40;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "浮点数";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // txtMAddress
            // 
            this.txtMAddress.Location = new System.Drawing.Point(107, 56);
            this.txtMAddress.Name = "txtMAddress";
            this.txtMAddress.Size = new System.Drawing.Size(87, 23);
            this.txtMAddress.TabIndex = 28;
            // 
            // txtDAddress
            // 
            this.txtDAddress.Location = new System.Drawing.Point(107, 104);
            this.txtDAddress.Name = "txtDAddress";
            this.txtDAddress.Size = new System.Drawing.Size(87, 23);
            this.txtDAddress.TabIndex = 27;
            // 
            // txtDValue
            // 
            this.txtDValue.Location = new System.Drawing.Point(272, 138);
            this.txtDValue.Name = "txtDValue";
            this.txtDValue.Size = new System.Drawing.Size(87, 23);
            this.txtDValue.TabIndex = 26;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(120, 19);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(50, 21);
            this.radioButton2.TabIndex = 39;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "正数";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // btnMWrite
            // 
            this.btnMWrite.Location = new System.Drawing.Point(308, 50);
            this.btnMWrite.Name = "btnMWrite";
            this.btnMWrite.Size = new System.Drawing.Size(94, 35);
            this.btnMWrite.TabIndex = 32;
            this.btnMWrite.Text = "M区写操作";
            this.btnMWrite.UseVisualStyleBackColor = true;
            this.btnMWrite.Click += new System.EventHandler(this.btnMWrite_Click);
            // 
            // btnMread
            // 
            this.btnMread.Location = new System.Drawing.Point(206, 50);
            this.btnMread.Name = "btnMread";
            this.btnMread.Size = new System.Drawing.Size(94, 35);
            this.btnMread.TabIndex = 31;
            this.btnMread.Text = "M区读操作";
            this.btnMread.UseVisualStyleBackColor = true;
            this.btnMread.Click += new System.EventHandler(this.btnMread_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "true",
            "false"});
            this.comboBox1.Location = new System.Drawing.Point(102, 136);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(92, 25);
            this.comboBox1.TabIndex = 38;
            // 
            // btnDWrite
            // 
            this.btnDWrite.Location = new System.Drawing.Point(308, 98);
            this.btnDWrite.Name = "btnDWrite";
            this.btnDWrite.Size = new System.Drawing.Size(94, 35);
            this.btnDWrite.TabIndex = 30;
            this.btnDWrite.Text = "D区写操作";
            this.btnDWrite.UseVisualStyleBackColor = true;
            this.btnDWrite.Click += new System.EventHandler(this.btnDWrite_Click);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(213, 144);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(53, 17);
            this.label28.TabIndex = 37;
            this.label28.Text = "D区值：";
            // 
            // btnDread
            // 
            this.btnDread.Location = new System.Drawing.Point(206, 98);
            this.btnDread.Name = "btnDread";
            this.btnDread.Size = new System.Drawing.Size(94, 35);
            this.btnDread.TabIndex = 29;
            this.btnDread.Text = "D区读操作";
            this.btnDread.UseVisualStyleBackColor = true;
            this.btnDread.Click += new System.EventHandler(this.btnDread_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(40, 139);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(56, 17);
            this.label22.TabIndex = 36;
            this.label22.Text = "M区值：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(37, 59);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 17);
            this.label11.TabIndex = 24;
            this.label11.Text = "M区地址：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(37, 107);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 17);
            this.label12.TabIndex = 23;
            this.label12.Text = "D区地址：";
            // 
            // ucPower1
            // 
            this.ucPower1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucPower1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucPower1.Location = new System.Drawing.Point(23, 234);
            this.ucPower1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucPower1.Name = "ucPower1";
            this.ucPower1.Size = new System.Drawing.Size(550, 157);
            this.ucPower1.TabIndex = 44;
            // 
            // ViewCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.tabControl1);
            this.Name = "ViewCheck";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Text = "设备点检";
            this.Load += new System.EventHandler(this.ViewCheck_Load);
            this.tabPage1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPage1;
        private Controls.UcMilCamera ucMilCamera1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TextBox txtMAddress;
        private System.Windows.Forms.TextBox txtDAddress;
        private System.Windows.Forms.TextBox txtDValue;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button btnMWrite;
        private System.Windows.Forms.Button btnMread;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnDWrite;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Button btnDread;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private Controls.UcPower ucPower1;
    }
}