using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RetroArchLibrarySource {
    internal class RetroArchHelper {

        private IRetroArchInstallation _installation;

        public RetroArchHelper(IRetroArchInstallation installation) {
            _installation = installation;
        }
        
        public int GamesCount() => GetRomsInPlayLists().Count;

        public List<LibrarySourceGameData> GetRomsInPlayLists() {
            var roms = new List<LibrarySourceGameData>();
            
            var playlistsPath = Path.Combine(_installation.InstallPath(), "playlists");
            
            if (!Directory.Exists(playlistsPath))
                return roms;
            
            foreach (var playlist in Directory.GetFiles(playlistsPath, "*.lpl")) {
                roms.AddRange(ParsePlaylist(playlist));
            }
            
            return roms;
        }

        private List<LibrarySourceGameData> ParsePlaylist(string fileName) {
            var raw = File.ReadAllText(fileName);
            dynamic json = JsonConvert.DeserializeObject<dynamic>(raw);

            if (json == null || json.items == null) return null;

            if (json.items is JArray games) {
                return games.Select(gameData => new LibrarySourceGameData {
                    Name = gameData.Value<string>("label"),
                    SourceId = gameData.Value<string>("crc32"),
                    Description = "", // RetroArch doesn't have description
                    ArtworkUrl = "", // Relay on SteamGridDB
                    BackgroundUrl = "" // Relay on SteamGridDB
                }).ToList();
            }

            return null;
        }
    }
}