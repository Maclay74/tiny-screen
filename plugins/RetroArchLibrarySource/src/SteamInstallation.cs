using System.IO;
using Microsoft.Win32;

namespace RetroArchLibrarySource {
    internal class SteamInstallation : IRetroArchInstallation {
        
        private const string _steamRegistryKey = "Software\\Wow6432Node\\Valve\\Steam";
        
        public string InstallPath() {
            RegistryKey steamKey = Registry.LocalMachine.OpenSubKey(_steamRegistryKey);
            if (steamKey == null) return "";
            
            // If path is not set
            string steamPath = steamKey.GetValue("InstallPath")?.ToString();
            if (steamPath == null) return "";
            
            return Path.Combine(steamPath, "steamapps", "common", "RetroArch");
        }

        public bool Installed() {
            return File.Exists(Path.Combine(InstallPath(), "retroarch.exe"));
        }
    }
}