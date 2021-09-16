using HYC.HTLog;
using RasterDetectionMod.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RasterDetectionMod.Forms
{
    public partial class FormBitMap : Form
    {
        public FormBitMap()
        {
            InitializeComponent();
            Bitmap_Set.Load();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap_Set.Width = this.txt_BitMap_Width.Text;
                Bitmap_Set.Height = this.txt_BitMap_Height.Text;
     
      
                    Bitmap_Set.SaveConfig();
                    MessageBox.Show("设置参数成功！");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Close();
        }

        private void FormBitMap_Load(object sender, EventArgs e)
        {
           
            this.txt_BitMap_Width.Text = Bitmap_Set.Width;
            this.txt_BitMap_Height.Text = Bitmap_Set.Height;


        }
    }
}
