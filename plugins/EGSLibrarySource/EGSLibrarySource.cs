using System;
using System.Linq;
using TinyScreen.Framework.Interfaces;
using System.Threading.Tasks;
using Microsoft.Win32;
using TinyScreen.Framework;
using TinyScreen.Framework.Exceptions;

namespace EGSLibrarySource {
    public class EGSLibrarySource : BaseLibrarySource {
        // EGS system-level information there
        private const string _registryKey = "SOFTWARE\\WOW6432Node\\Epic Games\\EpicGamesLauncher";

        // Helper for EGS
        private EGSHelper _egsHelper;

        public override string Name() {
            return "EGS";
        }

        public EGSLibrarySource() {
            _egsHelper = new EGSHelper(Registry.LocalMachine.OpenSubKey(_registryKey));
        }

        public override int GamesCount() {
            if (!IsInstalled()) return 0;
            return _egsHelper.GamesCount();
        }

        public override bool IsInstalled() {
            // If key present in the system, application is installed
            RegistryKey egsKey = Registry.LocalMachine.OpenSubKey(_registryKey);
            if (egsKey == null) return false;

            // If path is not set
            string installPath = egsKey.GetValue("AppDataPath")?.ToString();
            if (installPath == null) return false;

            // Check if installer exists
            // TODO implement

            return true;
        }

        public override async Task<string[]> GamesIds() {
            try {
                return _egsHelper
                    .GetInstalledGamesIds().Select(game => game.SourceId)
                    .ToArray();
            }
            catch (Exception) {
                throw new LibraryParseException();
            }
        }

        public override async Task<LibrarySourceGameData> Game(string sourceId) {
            try {
                return _egsHelper
                    .GetInstalledGamesIds().Find(game => game.SourceId == sourceId);
            }
            catch (Exception) {
                throw new LibrarySourceGameDataException();
            }
        }
    }
}