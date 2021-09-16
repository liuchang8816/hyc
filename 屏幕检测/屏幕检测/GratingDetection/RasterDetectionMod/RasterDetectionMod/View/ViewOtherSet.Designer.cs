
namespace RasterDetectionMod.View
{
    partial class ViewOtherSet
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
            this.ucParaSet1 = new RasterDetectionMod.Controls.UCParaSet();
            this.SuspendLayout();
            // 
            // ucParaSet1
            // 
            this.ucParaSet1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucParaSet1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucParaSet1.Location = new System.Drawing.Point(0, 0);
            this.ucParaSet1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucParaSet1.Name = "ucParaSet1";
            this.ucParaSet1.Size = new System.Drawing.Size(852, 525);
            this.ucParaSet1.TabIndex = 0;
            // 
            // ViewOtherSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.ClientSize = new System.Drawing.Size(852, 525);
            this.Controls.Add(this.ucParaSet1);
            this.Name = "ViewOtherSet";
            this.Text = "其他设置";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.UCParaSet ucParaSet1;
    }
}