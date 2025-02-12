using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinSimpleIDriver
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Any()) // Есть
            {
                Settings.settingsFileName = args.FirstOrDefault(x => x.Contains(".ini"));
            }

            // Получение настроек
            string myExeDir = ""; // new FileInfo(Assembly.GetEntryAssembly().Location).Directory.ToString();
            string fileName = myExeDir + $"\\{Settings.settingsFileName}";
            var iniFile = new INIfile(fileName);
            Settings.Set(iniFile);
            var eee = iniFile.GetKeys(fileName, "APPLICATION");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


    }
}
