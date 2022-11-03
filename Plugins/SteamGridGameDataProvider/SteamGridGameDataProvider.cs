﻿using System;
using System.Threading.Tasks;
using Common.Framework;
using Common.Interfaces;
using craftersmine.SteamGridDBNet;

namespace SteamGridGameDataProvider {
    public class SteamGridGameDataProvider : IGameDataProvider, IGameDataProvider<ArtworkGameDataType, string?>,
        IGameDataProvider<BackgroundGameDataType, string?> {
        public int Priority() => 10;

        private SteamGridDb _client;

        public SteamGridGameDataProvider() {
            _client = new SteamGridDb("c6c3d47a9c8deeff0f9481aea822bec1");
        }
        
        async Task<string?> IGameDataProvider<ArtworkGameDataType, string?>.GetData(ArtworkGameDataType dataType,
            string gameName) {
            return await GetImageUrlByGameName(gameName, SteamGridDbDimensions.W600H900);
        }

        async Task<string?> IGameDataProvider<BackgroundGameDataType, string?>.GetData(BackgroundGameDataType dataType,
            string gameName) {
            return await GetImageUrlByGameName(gameName, SteamGridDbDimensions.W920H430);
        }

        private async Task<string?> GetImageUrlByGameName(string gameName, SteamGridDbDimensions steamGridDbDimensions) {
            try {
                var games = await _client.SearchForGamesAsync(gameName);

                if (games == null || games.Length == 0) return null;

                var grids = await _client.GetGridsByGameIdAsync(
                    games[0].Id,
                    false,
                    false,
                    SteamGridDbStyles.AllGrids,
                    steamGridDbDimensions,
                    SteamGridDbFormats.All,
                    SteamGridDbTypes.Static
                );

                return grids?[0].FullImageUrl;
            }
            catch (Exception) {
                return null;
            }
        }
    }
}