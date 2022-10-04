using System;
using System.Threading.Tasks;
using Common.Framework;
using Common.Interfaces;
using craftersmine.SteamGridDBNet;

namespace SteamGridGameDataProvider {
    
    public class SteamGridGameDataProvider : IGameDataProvider, IGameDataProvider<ArtworkGameDataType, string?>, IGameDataProvider<ArtworkGameDataType, int?> {
        public int Priority() => 10;
        
        private SteamGridDb _client;
        
        public SteamGridGameDataProvider() {
            _client = new SteamGridDb("c6c3d47a9c8deeff0f9481aea822bec1");
        }
        
        async Task<string?> IGameDataProvider<ArtworkGameDataType, string?>.GetData(ArtworkGameDataType dataType, string gameName) {

            try {
                var games = await _client.SearchForGamesAsync(gameName);

                if (games.Length == 0) return null;
                
                var grids = await _client.GetGridsByGameIdAsync(
                    games[0].Id,
                    false,
                    false,
                    SteamGridDbStyles.AllGrids,
                    SteamGridDbDimensions.W600H900,
                    SteamGridDbFormats.All,
                    SteamGridDbTypes.Static
                );

                if (grids.Length == 0) return null;

                return grids[0].FullImageUrl;
            }
            catch (Exception) {
                return null;
            }
        }

        async Task<int?> IGameDataProvider<ArtworkGameDataType, int?>.GetData(ArtworkGameDataType dataType, string gameName) {
            if (gameName == "Fortnite")
                return null;

            return null;
        }
    }
}