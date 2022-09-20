using System;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Framework;
using Common.Interfaces;
using Microsoft.Win32;

namespace XboxLibrarySource {
    public class XboxLibrarySource : BaseLibrarySource {
        // Steam system-level information there
        private const string RegistryKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Appx\\PackageState";

        private XboxHelper _xboxHelper;

        public XboxLibrarySource() {
            _xboxHelper = new XboxHelper(Registry.LocalMachine.OpenSubKey(RegistryKey));
        }

        public override string Name() {
            return "Xbox";
        }

        public override int GamesCount() {
            if (!IsInstalled()) return 0;
            return _xboxHelper.GamesCount();
        }

        public override bool IsInstalled() {
            RegistryKey appxKey = Registry.LocalMachine.OpenSubKey(RegistryKey);

            // Totally not reliable
            // TODO check on different hardware, with removed games and removed xbox app
            if (appxKey == null || appxKey.GetSubKeyNames().Length == 0) return false;
            return true;
        }

        public override async Task<string[]> GamesIds() {
            try {
                return _xboxHelper.GetInstalledGamesIds().ToArray();
            }
            catch (Exception) {
                throw new LibraryParseException();
            }
        }

        public override async Task<LibrarySourceGameData> Game(string sourceId) {
            try {
                return await _xboxHelper.GetGameInfo(sourceId);
            }
            catch (Exception) {
                throw new LibrarySourceGameDataException();
            }
        }
    }
}