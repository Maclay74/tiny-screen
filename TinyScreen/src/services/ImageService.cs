using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using craftersmine.SteamGridDBNet;
using Godot;
using TinyScreen.Framework.exceptions;
using Path = System.IO.Path;

namespace TinyScreen.Services {
    public class ImageService {

        public ImageService() {
            System.IO.Directory.CreateDirectory(Path.Combine(OS.GetUserDataDir(), "artwork"));
            System.IO.Directory.CreateDirectory(Path.Combine(OS.GetUserDataDir(), "background"));
        }

        public enum ImageType {
            Artwork,
            Background
        }

        public async Task<string> Save(string url, ImageType type, string name = "") {
            
            // If image is valid, okay, just download it
            if (!await IsImageValid(url)) {

                try {
                    // Fallback to SteamGrid
                    url = await GetImageFromSteamGrid(name, type);
                }
                catch (Exception) {
                    throw new LibraryGraphicsException();
                }
                
                // Fallback to generate
                if (url.Length ==  0) {
                    // TODO Fallback to generate image    
                }
            }
            var fileName = Guid.NewGuid() + Path.GetExtension(url);
            var path = Path.Combine(OS.GetUserDataDir(), type.ToString().ToLower(), fileName);
            
            using (WebClient webClient = new WebClient()) {
                webClient.DownloadFile(url, path) ; 
            }
            
            // TODO implement image processing - crop, convert to webp
            return path;
        }

        private async Task<string> GetImageFromSteamGrid(string gameName, ImageType type) {
            SteamGridDb sgdb = new SteamGridDb(ProjectSettings.GetSetting("application/config/steamgrid/key") as string);
            var games = await sgdb.SearchForGamesAsync(gameName);

            SteamGridDbDimensions dimensions;

            switch (type) {
                case ImageType.Artwork:
                    dimensions = SteamGridDbDimensions.W600H900;
                    break;
                case ImageType.Background:
                    dimensions = SteamGridDbDimensions.W920H430;
                    break;
                default:
                    dimensions = SteamGridDbDimensions.AllGrids;
                    break;
            }

            if (games.Length != 0) {
                var grids = await sgdb.GetGridsByGameIdAsync(
                    games[0].Id, 
                    false, 
                    false, 
                    SteamGridDbStyles.AllGrids, 
                    dimensions, 
                    SteamGridDbFormats.All, 
                    SteamGridDbTypes.Static
                );

                if (grids.Length > 0) {
                    return grids[0].FullImageUrl;
                }
            }

            return "";
        }

        public async Task<bool> IsImageValid(string url) {
            if (url == "") return false;
            
            var client = new HttpClient();
            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            return response.IsSuccessStatusCode;
        }
    }
}