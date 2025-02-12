using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinSimpleIDriver
{
    class INIfile
    {
        public string path { get; private set; }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);

        public INIfile(string INIPath)
        {
            path = INIPath;
        }
        public List<string> GetKeys(string iniFile, string category)
        {
            byte[] buffer = new byte[2048];

            GetPrivateProfileSection(category, buffer, 2048, iniFile);

            String[] tmp = Encoding.ASCII.GetString(buffer).Trim('\0').Split('\0');

            List<string> result = new List<string>();

            foreach (String entry in tmp)
            {
                if (String.IsNullOrWhiteSpace(entry))
                    continue;
                result.Add(entry.Substring(0, entry.IndexOf("=")));
            }

            return result;
        }
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }
    }
}
