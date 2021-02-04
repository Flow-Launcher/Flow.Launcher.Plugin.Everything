using Droplex;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.Everything
{
    internal static class Utilities
    {
        internal static string GetInstalledPath()
        {
            string displayName;
            string uninstallString;

            using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            if (key != null)
            {
                foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                {
                    displayName = subkey.GetValue("DisplayName") as string;
                    if (displayName != null && displayName.Contains("Everything"))
                    {
                        uninstallString = subkey.GetValue("UninstallString") as string;
                        return Path.Combine(Path.GetDirectoryName(uninstallString), "Everything.exe");
                    }
                }
            }

            var scoopInstalledPath = Environment.ExpandEnvironmentVariables(@"%userprofile%\scoop\apps\everything\current\Everything.exe");
            if (File.Exists(scoopInstalledPath))
                return scoopInstalledPath;

            return string.Empty;
        }
    }
}
