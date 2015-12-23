using Microsoft.Win32;
using SeriesUpdater.Internal;
using System.IO;
using System.Windows.Forms;

namespace SeriesUpdater.Context
{
    class Settings
    {
        const string startupKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public static bool SetAutorun(bool IsAdd = false)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(startupKeyPath, true);
            
            if (IsAdd || (!IsAdd && !IsStartupItem()))
            {
                DialogResult copyExecutableDialogResult = Notifications.ShowQuestion(
                    "Do you want to save a copy of the program? In this way start with Windows will be possible even if this file will be deleted.",
                    "Start with Windows");

                if (copyExecutableDialogResult == DialogResult.Yes && !File.Exists(Variables.ExecutableFileName))
                {
                    File.Copy(Application.ExecutablePath, Variables.ExecutableFileName);
                    registryKey.SetValue(Variables.AppName, Variables.ExecutableFileName);
                }

                else
                {
                    registryKey.SetValue(Variables.AppName, Application.ExecutablePath);
                }

                return true;
            }

            else
            {
                registryKey.DeleteValue(Variables.AppName, false);
                return false;
            }
        }

        public static bool IsStartupItem()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(startupKeyPath, true);
            return registryKey.GetValue(Variables.AppName) != null;
        }

        public static bool IsFirst()
        {
            return !Directory.Exists(Variables.DataFolderPath);
        }
    }


}
