namespace RasterDetectionMod.View
{
    partial class ViewMain
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
            this.ucDetectionGroupEx1 = new RasterDetectionMod.Controls.UcDetectionGroupEx();
            this.SuspendLayout();
            // 
            // ucDetectionGroupEx1
            // 
            this.ucDetectionGroupEx1.DetectionType = HYC.StandardAoi.Parameter.DetectionType.Aoi;
            this.ucDetectionGroupEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDetectionGroupEx1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucDetectionGroupEx1.Location = new System.Drawing.Point(0, 0);
            this.ucDetectionGroupEx1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucDetectionGroupEx1.Name = "ucDetectionGroupEx1";
            this.ucDetectionGroupEx1.PanelID = "";
            this.ucDetectionGroupEx1.Size = new System.Drawing.Size(807, 540);
            this.ucDetectionGroupEx1.TabIndex = 4;
            this.ucDetectionGroupEx1.Title = "Title";
            // 
            // ViewMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 540);
            this.Controls.Add(this.ucDetectionGroupEx1);
            this.Name = "ViewMain";
            this.Text = "ViewMain";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.UcDetectionGroupEx ucDetectionGroupEx1;
    }
}