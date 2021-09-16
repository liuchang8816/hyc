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
    public partial class FormRiDong_Set : Form
    {
        public FormRiDong_Set()
        {
            InitializeComponent();
            Work3DLayer_Set.Load();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {

                if (checkBox1.Checked)
                {
                    Work3DLayer_Set.IsShielded_01 = Convert.ToBoolean("True");
                }
                else
                {
                    Work3DLayer_Set.IsShielded_01 = Convert.ToBoolean("false");
                }
                if (checkBox2.Checked)
                {
                    Work3DLayer_Set.IsShielded_11 = Convert.ToBoolean("True");
                }
                else
                {
                    Work3DLayer_Set.IsShielded_11 = Convert.ToBoolean("false");
                }
                if (checkBox3.Checked)
                {
                    Work3DLayer_Set.IsShielded_21 = Convert.ToBoolean("True");
                }
                else
                {
                    Work3DLayer_Set.IsShielded_21 = Convert.ToBoolean("false");
                }   

                Work3DLayer_Set.SaveConfig();
                MessageBox.Show("设置参数成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Close();
        }

        private void FormRiDong_Set_Load(object sender, EventArgs e)
        {

            Work3DLayer_Set.Load();
            if (Convert.ToBoolean(Work3DLayer_Set.IsShielded_01))
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
            if (Convert.ToBoolean(Work3DLayer_Set.IsShielded_11))
            {
                checkBox2.Checked = true;
            }
            else
            {
                checkBox2.Checked = false;
            }
            if (Convert.ToBoolean(Work3DLayer_Set.IsShielded_21))
            {
                checkBox3.Checked = true;
            }
            else
            {
                checkBox3.Checked = false;
            }

        }



    }
}
