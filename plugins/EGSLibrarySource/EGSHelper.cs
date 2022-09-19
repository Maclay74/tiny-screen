using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TinyScreen.Framework.Interfaces;

namespace EGSLibrarySource {
    internal class EGSHelper {

        private RegistryKey _registryKey;

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

            if (!Directory.Exists(libraryFilePath))
            {
                Console.WriteLine("Error: directory not found");
            }
            else
            {
                string[] files = Directory.GetFiles(libraryFilePath, "*.item");
                
               // Console.WriteLine("files: " + files);

                foreach (var file in files)
                {
                    //TODO: check the game folder is not deleted
                    
                    var fileText = File.ReadAllText(file);
                    JObject fileJson = JObject.Parse(fileText);
                    Console.WriteLine("fileText: " + fileJson);
                    Console.WriteLine("type: " + fileJson.GetType());
                    Console.WriteLine("DisplayName: " + fileJson["DisplayName"]);

                    var libSrc = new LibrarySourceGameData { };
                    libSrc.Name = fileJson["DisplayName"].ToString();
                    libSrc.SourceId = fileJson["CatalogItemId"].ToString();
                    libSrc.Description = "";
                    libSrc.ArtworkUrl = "";
                    libSrc.BackgroundUrl = "";

                        gamesIds.Add(libSrc);
                }
            }
            return gamesIds;
        }
    }
}