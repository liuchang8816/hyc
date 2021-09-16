namespace RasterDetectionMod.Controls
{
    partial class UcPower
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboPower = new System.Windows.Forms.ComboBox();
            this.btnPowerOff = new System.Windows.Forms.Button();
            this.btnPowerOn = new System.Windows.Forms.Button();
            this.chkChannel1 = new System.Windows.Forms.CheckBox();
            this.chkChannel2 = new System.Windows.Forms.CheckBox();
            this.chkChannel3 = new System.Windows.Forms.CheckBox();
            this.chkChannel4 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "光源选择：";
            // 
            // cboPower
            // 
            this.cboPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPower.FormattingEnabled = true;
            this.cboPower.Location = new System.Drawing.Point(207, 12);
            this.cboPower.Name = "cboPower";
            this.cboPower.Size = new System.Drawing.Size(151, 25);
            this.cboPower.TabIndex = 16;
            // 
            // btnPowerOff
            // 
            this.btnPowerOff.Location = new System.Drawing.Point(281, 112);
            this.btnPowerOff.Name = "btnPowerOff";
            this.btnPowerOff.Size = new System.Drawing.Size(100, 32);
            this.btnPowerOff.TabIndex = 13;
            this.btnPowerOff.Text = "关闭";
            this.btnPowerOff.UseVisualStyleBackColor = true;
            this.btnPowerOff.Click += new System.EventHandler(this.btnPowerOff_Click);
            // 
            // btnPowerOn
            // 
            this.btnPowerOn.Location = new System.Drawing.Point(132, 112);
            this.btnPowerOn.Name = "btnPowerOn";
            this.btnPowerOn.Size = new System.Drawing.Size(100, 32);
            this.btnPowerOn.TabIndex = 12;
            this.btnPowerOn.Text = "开启";
            this.btnPowerOn.UseVisualStyleBackColor = true;
            this.btnPowerOn.Click += new System.EventHandler(this.btnPowerOn_Click);
            // 
            // chkChannel1
            // 
            this.chkChannel1.AutoSize = true;
            this.chkChannel1.Checked = true;
            this.chkChannel1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChannel1.Location = new System.Drawing.Point(94, 62);
            this.chkChannel1.Name = "chkChannel1";
            this.chkChannel1.Size = new System.Drawing.Size(58, 21);
            this.chkChannel1.TabIndex = 18;
            this.chkChannel1.Text = "通道1";
            this.chkChannel1.UseVisualStyleBackColor = true;
            // 
            // chkChannel2
            // 
            this.chkChannel2.AutoSize = true;
            this.chkChannel2.Checked = true;
            this.chkChannel2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChannel2.Location = new System.Drawing.Point(183, 62);
            this.chkChannel2.Name = "chkChannel2";
            this.chkChannel2.Size = new System.Drawing.Size(58, 21);
            this.chkChannel2.TabIndex = 19;
            this.chkChannel2.Text = "通道2";
            this.chkChannel2.UseVisualStyleBackColor = true;
            // 
            // chkChannel3
            // 
            this.chkChannel3.AutoSize = true;
            this.chkChannel3.Checked = true;
            this.chkChannel3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChannel3.Location = new System.Drawing.Point(274, 62);
            this.chkChannel3.Name = "chkChannel3";
            this.chkChannel3.Size = new System.Drawing.Size(58, 21);
            this.chkChannel3.TabIndex = 20;
            this.chkChannel3.Text = "通道3";
            this.chkChannel3.UseVisualStyleBackColor = true;
            // 
            // chkChannel4
            // 
            this.chkChannel4.AutoSize = true;
            this.chkChannel4.Checked = true;
            this.chkChannel4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChannel4.Location = new System.Drawing.Point(366, 62);
            this.chkChannel4.Name = "chkChannel4";
            this.chkChannel4.Size = new System.Drawing.Size(58, 21);
            this.chkChannel4.TabIndex = 21;
            this.chkChannel4.Text = "通道4";
            this.chkChannel4.UseVisualStyleBackColor = true;
            // 
            // UcPower
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkChannel4);
            this.Controls.Add(this.chkChannel3);
            this.Controls.Add(this.chkChannel2);
            this.Controls.Add(this.chkChannel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboPower);
            this.Controls.Add(this.btnPowerOff);
            this.Controls.Add(this.btnPowerOn);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UcPower";
            this.Size = new System.Drawing.Size(550, 157);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboPower;
        private System.Windows.Forms.Button btnPowerOff;
        private System.Windows.Forms.Button btnPowerOn;
        private System.Windows.Forms.CheckBox chkChannel1;
        private System.Windows.Forms.CheckBox chkChannel2;
        private System.Windows.Forms.CheckBox chkChannel3;
        private System.Windows.Forms.CheckBox chkChannel4;
    }
}
