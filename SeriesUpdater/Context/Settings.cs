using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SeriesUpdater.Context
{
    class Settings
    {
        public static List<string[]> GlobalSettings = new List<string[]> {
            new string[] { "SendNotifications", "True" },
            new string[] { "RunOnStartup", "False" } };

        public static bool SetAutorun(bool IsAdd)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            
            if (IsAdd || (!IsAdd && !IsStartupItem()))
            {
                DialogResult copyExecutableDialogResult =
                    MessageBox.Show("Szeretné, ha a programról készülne egy másolat? Így a file törlése esetén is lehetséges a Windows indításakor való futtatás.",
                    "Indítás a Windowszal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (!File.Exists(MainProgram.Variables.ExecutableFileName) && copyExecutableDialogResult == DialogResult.Yes)
                {
                    File.Copy(Application.ExecutablePath, MainProgram.Variables.ExecutableFileName);
                    registryKey.SetValue("SeriesUpdater", MainProgram.Variables.ExecutableFileName);
                }

                else
                {
                    registryKey.SetValue("SeriesUpdater", Application.ExecutablePath);
                }

                return true;
            }

            else
            {
                registryKey.DeleteValue("SeriesUpdater", false);
                return false;
            }
        }

        public static bool IsStartupItem()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            return registryKey.GetValue("SeriesUpdater") != null;
        }

        public static bool IsFirstCheck()
        {
            return !Directory.Exists(MainProgram.Variables.DataFolderPath);
        }

        public static void ChangeSettings(int Number, string Value)
        {
            GlobalSettings[Number][1] = Value;
            IO.WriteSettings(GlobalSettings[Number][0], Value);
        }
    }
}
