using System;
using System.IO;
using System.Linq;
using TinyScreen.Framework.Interfaces;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;
using TinyScreen.Framework;

namespace SteamLibrarySource {
    public class SteamLibrarySource : BaseLibrarySource {
    
        // Steam system-level information there
        private const string _registryKey = "Software\\Wow6432Node\\Valve\\Steam";

        // Helper for steam
        private SteamHelper _steamHelper;

        public override string Name() {
            return "Steam";
        }
        
        public SteamLibrarySource() {
            _steamHelper = new SteamHelper(Registry.LocalMachine.OpenSubKey(_registryKey));
        }
        
        public override int GamesCount() {
            if (!IsInstalled()) return 0;
            return _steamHelper.GamesCount();
        }

        public override bool IsInstalled() {
            
            // If key present in the system, application is installed
            RegistryKey steamKey = Registry.LocalMachine.OpenSubKey(_registryKey);
            if (steamKey == null) return false;
            
            // If path is not set
            string installPath = steamKey.GetValue("InstallPath")?.ToString();
            if (installPath == null) return false;

            // Check if installer exists
            return File.Exists(Path.Combine(installPath, "steam.exe"));
        }

        public override async Task<string[]> GamesIds() => _steamHelper.GetInstalledGamesIds().ToArray();

        public override async Task<LibrarySourceGameData> Game(string sourceId) {
            return await _steamHelper.GetGameInfo(sourceId);
        }
    }
}