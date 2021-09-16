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
    public partial class FormSESet : Form
    {
        
        public FormSESet()
        {
            InitializeComponent();
       
            Work3DLayer_Set.Load();
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
                if (checkBox4.Checked)
                {
                    Work3DLayer_Set.IsShielded_31 = Convert.ToBoolean("True");
                }
                else
                {
                    Work3DLayer_Set.IsShielded_31 = Convert.ToBoolean("false");
                }
                if (checkBox5.Checked)
                {
                    Work3DLayer_Set.IsShielded_41 = Convert.ToBoolean("True");
                }
                else
                {
                    Work3DLayer_Set.IsShielded_41 = Convert.ToBoolean("false");
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

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormWork3DLayer_Set_Load(object sender, EventArgs e)
        {
            Work3DLayer_Set.Load();

            if (Convert.ToBoolean(Work3DLayer_Set.IsShielded_01))
            {
                checkBox1.Checked= true;
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
                checkBox3.Checked =true;
            }
            else
            {
                checkBox3.Checked = false;
            }
            if (Convert.ToBoolean(Work3DLayer_Set.IsShielded_31))
            {
                checkBox4.Checked = true;
            }
            else
            {
                checkBox4.Checked = false;
            }
            if (Convert.ToBoolean(Work3DLayer_Set.IsShielded_41))
            {
                checkBox5.Checked = true;
            }
            else
            {
                checkBox5.Checked = false;
            }

        }
    }
}
