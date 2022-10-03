using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace RetroArchLibrarySource {
    internal class StandaloneInstallation : IRetroArchInstallation {
        
        private const string _registryKey = "Software\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\RetroArch";
        
        public string InstallPath() {
            RegistryKey steamKey = Registry.LocalMachine.OpenSubKey(_registryKey);
            if (steamKey == null) return "";
            
            // If path is not set
            string installPath = steamKey.GetValue("DisplayIcon")?.ToString();
            if (installPath == null) return "";
            
            var pattern = @"([^?]+)retroarch.exe";
            var match = Regex.Match(installPath, pattern);

            // Exclude exe name from the path
            if (match.Groups.Count > 1)
                return match.Groups[1].ToString();
            
            return "";
        }

        public bool Installed() {
            return File.Exists(Path.Combine(InstallPath(), "retroarch.exe"));
        }
    }
}