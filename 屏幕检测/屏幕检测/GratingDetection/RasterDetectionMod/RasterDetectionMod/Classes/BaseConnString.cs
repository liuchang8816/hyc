using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RasterDetectionMod.classes
{
   
    static class BaseConnString
    {
       public static readonly string mysql = "";
        public static readonly string mssql = "";
        static BaseConnString()
        {
            string path = Path.Combine(Application.StartupPath, "DataBase.conf");
            if (File.Exists(path))
            {
                string[] line = File.ReadAllLines(path);
                mysql = line[0];
                mssql = line[1];
            }
        }
    }
}
