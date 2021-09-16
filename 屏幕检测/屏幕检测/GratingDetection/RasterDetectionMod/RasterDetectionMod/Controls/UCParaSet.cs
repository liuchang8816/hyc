using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RasterDetectionMod.Controls
{
    public partial class UCParaSet : UserControl
    {
        public UCParaSet()
        {
            InitializeComponent();
            softGrid.SelectedObject = Program.softRunParamaters;
        }
    }
}
