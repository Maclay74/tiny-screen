using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Gameloop.Vdf;
using Gameloop.Vdf.JsonConverter;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TinyScreen.Framework.Interfaces;

namespace SteamLibrarySource {
    internal class SteamHelper {
        private RegistryKey _registryKey;

        private string[] ignoreIds = {
            "228980", // Steamworks 
            "1118310" // RetroArch
        };

        private const string apiBase = "https://store.steampowered.com/api/";

        private string GetInstalledDirectory() {
            return _registryKey?.GetValue("InstallPath")?.ToString();
        }

        public SteamHelper(RegistryKey registryKey) {
            _registryKey = registryKey;
        }

        public int GamesCount() => GetInstalledGamesIds().Count;

        public List<string> GetInstalledGamesIds() {
            var installedPath = GetInstalledDirectory();

            // In this file we have information about installed games
            var libraryFilePath = Path.Combine(installedPath, "config", "libraryfolders.vdf");

            // Text => Json
            var configContent = File.ReadAllText(libraryFilePath);
            var configRoot = VdfConvert.Deserialize(configContent).Value.ToJson();
            var gamesIds = new List<string>();

            // The hell of a parse
            foreach (var library in configRoot) {
                foreach (var param in library.OfType<JObject>()) {
                    foreach (var apps in param.Property("apps")) {
                        foreach (var app in apps.OfType<JProperty>()) {
                            if (ignoreIds.Contains(app.Name))
                                continue;

                            gamesIds.Add(app.Name);
                        }
                    }
                }
            }

            return gamesIds;
        }

        public async Task<LibrarySourceGameData> GetGameInfo(string id) {
            var client = new HttpClient();
            var apiGameLink = apiBase + "appdetails?appids=" + id;

            // Get information about the game
            var response = await client.GetAsync(apiGameLink);

            // Bad response
            if (!response.IsSuccessStatusCode) return null;

            var content = response.Content.ReadAsStringAsync().Result;
            dynamic json = JsonConvert.DeserializeObject<dynamic>(content)[id];

            // Bad game
            if (json.success == false) return null;

            return new LibrarySourceGameData {
                SourceId = id,
                Name = json.data.name.ToString(),
                Description = json.data.short_description.ToString(),
                ArtworkUrl = GenerateArtworkUrl(id),
                BackgroundUrl = GenerateBackgroundUrl(json.data.background_raw.ToString()),
            };
        }

        private string GenerateArtworkUrl(string id) {
            return $"https://cdn.akamai.steamstatic.com/steam/apps/{id}/library_600x900_2x.jpg";
        }

        private string GenerateBackgroundUrl(string backgroundUrl) {
            var pattern = @"^([^?]+)";
            return Regex.Match(backgroundUrl, pattern).ToString();
        }
    }
}