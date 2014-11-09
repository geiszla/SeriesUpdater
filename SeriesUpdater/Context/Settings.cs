using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SeriesUpdater.Context
{
    class Settings
    {
        public static List<string[]> settings = new List<string[]> { new string[] { "SendNotifications", "True" }, new string[] { "RunOnStartup", "False" } };

        public static bool setAutorun(bool isAdd)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            if (isAdd || (!isStartupItem() && !isAdd))
            {
                if (!File.Exists(MainProgram.Variables.dataPath + @"\SeriesUpdater.exe") && MessageBox.Show("Szeretné, ha a programról készülne egy másolat? Így a file törlése esetén is lehetséges a Windows indításakor való futtatás.", "Indítás a Windowszal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    File.Copy(Application.ExecutablePath.ToString(), MainProgram.Variables.dataPath + @"\SeriesUpdater.exe");
                    registryKey.SetValue("MainProgram", MainProgram.Variables.dataPath + @"\SeriesUpdater.exe");
                }

                else
                {
                    registryKey.SetValue("MainProgram", Application.ExecutablePath.ToString());
                }

                return true;
            }

            else
            {
                registryKey.DeleteValue("MainProgram", false);
                return false;
            }
        }

        public static bool isStartupItem()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            if (registryKey.GetValue("MainProgram") == null)
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        public static bool isFirstCheck()
        {
            if (Directory.Exists(MainProgram.Variables.dataPath))
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        public static void changeSettings(int number, string value)
        {
            Context.Settings.settings[number][1] = value;
            Context.IO.writeSettings(Context.Settings.settings[number][0], value);
        }
    }
}
