using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HYC.HTTools;
using RasterDetectionMod.Entities;

namespace RasterDetectionMod
{
    static class Program
    {

        public static SoftRunParamaters softRunParamaters = new SoftRunParamaters();

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            HTInstanceHelper.CheckSingleInstance();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
