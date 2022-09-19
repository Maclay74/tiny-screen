using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Win32;
using TinyScreen.Framework.Extensions;
using TinyScreen.Framework.Interfaces;

namespace XboxLibrarySource {
    public class XboxHelper {
        private RegistryKey _registryKey;

        public XboxHelper(RegistryKey registryKey) {
            _registryKey = registryKey;
        }

        public int GamesCount() => GetInstalledGamesIds().Count;

        public List<string> GetInstalledGamesIds() {
            var ids = new List<string>();

            foreach (var libKey in _registryKey.GetSubKeyNames()) {
                var lib = _registryKey.OpenSubKey(libKey);
                if (lib == null) continue;

                foreach (var gameId in lib.GetSubKeyNames()) {
                    ids.Add(gameId);
                }
            }

            return ids;
        }

        public async Task<LibrarySourceGameData> GetGameInfo(string id) {
            var name = await GameNameByPackageName(id);

            return new LibrarySourceGameData {
                SourceId = id,
                Name = name,
                Description = "Game from Xbox", // TODO get information from internet
                ArtworkUrl = "",
                BackgroundUrl = "",
            };
        }

        private async Task<string> GameNameByPackageName(string packageName) {
            var pattern = new Regex("^([^*]+)_");
            var match = pattern.Match(packageName);

            if (!match.Success || match.Groups.Count == 0) {
                return packageName;
            }

            var command = "\"&  echo (Get-StartApps | ? {$_.AppId -eq '" + packageName + "!App'}).Name \"";

            var process = new Process {
                StartInfo = {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = @"C:\windows\system32\windowspowershell\v1.0\powershell.exe",
                    Arguments = command
                }
            };

            process.Start();
            string name = process.StandardOutput.ReadToEnd();
            await process.WaitForExitAsync();

            return name.Trim();
        }
    }
}