using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RasterDetectionMod.Entities
{
    /// <summary>
    /// 软件运行参数
    /// </summary>
    public class SoftRunParamaters
    {
        Configuration configFile;
        KeyValueConfigurationCollection settings;
        public SoftRunParamaters()
        {
            configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            settings = configFile.AppSettings.Settings;
        }



        [Category("3D相机配置"), DisplayName("Size3dW"), Description("3D相机视野宽度大小，格式为：float类型")]
        public string Size3dW { get { return settings["Size3dW"].Value; } set { AddUpdateAppSettings("Size3dW", value.ToString()); } }

        [Category("3D相机配置"), DisplayName("Size3dH"), Description("3D相机视野高度大小，格式为：float类型")]
        public string Size3dH { get { return settings["Size3dH"].Value; } set { AddUpdateAppSettings("Size3dH", value.ToString()); } }

        [Category("3D相机配置:"), DisplayName("Max_Image_Area"), Description("图像区域最大数量")]
        public string Image_Area { get { return settings["Image_Area_Length"].Value; } set { AddUpdateAppSettings("Image_Area_Length", value.ToString()); } }



        public void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }



      


        /// 读取INI文件键值stringbulider对象字节大小文件路径
        [DllImport("kernel32")]

        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);




        string Path = Application.StartupPath + "\\AppConfig.ini";
        /// <summary>
        /// 读取INI文件中的内容方法
        /// </summary>
        /// <param name="Section">键</param>
        /// <param name="key">值</param>
        /// <returns></returns>
        public string ReadContentValue(string Section, string key)

        {

            StringBuilder temp = new StringBuilder(1024);

            GetPrivateProfileString(Section, key, "", temp, 1024, this.Path);

            return temp.ToString();

        }

    }
}