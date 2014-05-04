using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;

namespace SeriesUpdater.Context
{
    class Settings
    {
        public static bool setAutorun(bool addAnyway)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            if (!isStartupItem() || addAnyway)
            {
                if (!File.Exists(MainProgram.Variables.dataPath + @"\MainProgram.exe") && MessageBox.Show("Szeretné, ha a programról készülne egy másolat? Így a file törlése esetén is lehetséges a Windows indításakor való futtatás.", "Indítás a Windowszal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    File.Copy(Application.ExecutablePath.ToString(), MainProgram.Variables.dataPath + @"\MainProgram.exe");
                    registryKey.SetValue("MainProgram", MainProgram.Variables.dataPath + @"\MainProgram.exe");
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
    }
}
