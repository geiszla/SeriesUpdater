using Microsoft.Win32;
using System.Collections.Generic;
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
                DialogResult copyExecutableDialogResult =
                    MessageBox.Show("Do you want to save a copy of the executable? In this way start with Windows will be possible even if this file will be deleted.",
                    "Start with Windows", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (copyExecutableDialogResult == DialogResult.Yes && !File.Exists(Internal.Variables.ExecutableFileName))
                {
                    File.Copy(Application.ExecutablePath, Internal.Variables.ExecutableFileName);
                    registryKey.SetValue("SeriesUpdater", Internal.Variables.ExecutableFileName);
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
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(startupKeyPath, true);
            return registryKey.GetValue("SeriesUpdater") != null;
        }

        public static bool IsFirst()
        {
            return !Directory.Exists(Internal.Variables.DataFolderPath);
        }
    }


}
