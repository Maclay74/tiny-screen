using System.Collections.Generic;
using System;
using System.IO;
using Common.Interfaces;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;

namespace EGSLibrarySource {
    internal class EGSHelper {
        private RegistryKey _registryKey;

        private List<string> exceptions = new List<string> {
            "1ce536b653df463bb2492f529d832b71", // Quixel Bridge
            "d33c86e1279b45fc9a889f5e64ed6705", // Unreal Engine
        };

        private string GetInstalledDirectory() {
            return _registryKey?.GetValue("AppDataPath")?.ToString();
        }

        public EGSHelper(RegistryKey registryKey) {
            _registryKey = registryKey;
        }

        public int GamesCount() => GetInstalledGamesIds().Count;

        public List<LibrarySourceGameData> GetInstalledGamesIds() {
            var installedPath = GetInstalledDirectory();

            // In this file we have information about installed games
            var libraryFilePath = Path.Combine(installedPath, "Manifests");

            var gamesIds = new List<LibrarySourceGameData>();

            if (!Directory.Exists(libraryFilePath)) {
                Console.WriteLine("Error: directory not found");
            }
            else {
                string[] files = Directory.GetFiles(libraryFilePath, "*.item");
                
                foreach (var file in files) {
                    //TODO: check the game folder is not deleted

                    var fileText = File.ReadAllText(file);
                    JObject fileJson = JObject.Parse(fileText);

                    if (exceptions.Contains(fileJson["CatalogItemId"].ToString())) 
                        continue;

                    var libSrc = new LibrarySourceGameData {
                        Name =  fileJson["DisplayName"].ToString(),
                        SourceId =  fileJson["CatalogItemId"].ToString(),
                        Description =  "",
                        ArtworkUrl =  "",
                        BackgroundUrl =  "",
                    };
                    
                    gamesIds.Add(libSrc);
                }
            }

            return gamesIds;
        }
    }
}