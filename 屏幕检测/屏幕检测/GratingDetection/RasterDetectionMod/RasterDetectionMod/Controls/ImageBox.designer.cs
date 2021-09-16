
namespace RasterDetectionMod.Controls
{
    partial class ImageBox
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
            this.tbpnlMain = new System.Windows.Forms.TableLayoutPanel();
            this.pict = new RasterDetectionMod.Controls.ZoomablePictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblMousePos = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblColor = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSizeMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblZoomScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbpnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pict)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbpnlMain
            // 
            this.tbpnlMain.ColumnCount = 1;
            this.tbpnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbpnlMain.Controls.Add(this.pict, 0, 0);
            this.tbpnlMain.Controls.Add(this.statusStrip1, 0, 1);
            this.tbpnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbpnlMain.Location = new System.Drawing.Point(0, 0);
            this.tbpnlMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbpnlMain.Name = "tbpnlMain";
            this.tbpnlMain.RowCount = 2;
            this.tbpnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbpnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tbpnlMain.Size = new System.Drawing.Size(400, 200);
            this.tbpnlMain.TabIndex = 0;
            // 
            // pict
            // 
            this.pict.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pict.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pict.IsRegionSelectEnabled = true;
            this.pict.Location = new System.Drawing.Point(0, 0);
            this.pict.Margin = new System.Windows.Forms.Padding(0);
            this.pict.Name = "pict";
            this.pict.Size = new System.Drawing.Size(400, 175);
            this.pict.TabIndex = 0;
            this.pict.TabStop = false;
            this.pict.Paint += new System.Windows.Forms.PaintEventHandler(this.pict_Paint);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMousePos,
            this.toolStripStatusLabel1,
            this.lblColor,
            this.toolStripStatusLabel3,
            this.lblSizeMode,
            this.toolStripStatusLabel2,
            this.lblZoomScale});
            this.statusStrip1.Location = new System.Drawing.Point(0, 175);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(400, 25);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblMousePos
            // 
            this.lblMousePos.AutoSize = false;
            this.lblMousePos.Name = "lblMousePos";
            this.lblMousePos.Size = new System.Drawing.Size(80, 20);
            this.lblMousePos.Text = "0,0";
            this.lblMousePos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Gray;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(11, 20);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = false;
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(120, 20);
            this.lblColor.Text = "R:0 G:0 B:0";
            this.lblColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.Gray;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(11, 20);
            this.toolStripStatusLabel3.Text = "|";
            // 
            // lblSizeMode
            // 
            this.lblSizeMode.Name = "lblSizeMode";
            this.lblSizeMode.Size = new System.Drawing.Size(42, 20);
            this.lblSizeMode.Text = "Zoom";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.Gray;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(11, 20);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // lblZoomScale
            // 
            this.lblZoomScale.Name = "lblZoomScale";
            this.lblZoomScale.Size = new System.Drawing.Size(40, 20);
            this.lblZoomScale.Text = "100%";
            // 
            // ImageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbpnlMain);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ImageBox";
            this.Size = new System.Drawing.Size(400, 200);
            this.tbpnlMain.ResumeLayout(false);
            this.tbpnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pict)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbpnlMain;
        private ZoomablePictureBox pict;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblMousePos;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblSizeMode;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lblZoomScale;
        private System.Windows.Forms.ToolStripStatusLabel lblColor;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
    }
}
