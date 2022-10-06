using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Framework;
using Common.Interfaces;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Models;

namespace TinyScreen.Services; 

public class LibraryService {

    private IDatabaseService _databaseService;
    private ImageService _imageService;
    private ModalService _modalService;
    private IEnumerable<IGameDataProvider> _gameDataProviders;

    public LibraryService(IDatabaseService databaseService, ImageService imageService, ModalService modalService, IEnumerable<IGameDataProvider> gameDataProviders) {
        _databaseService = databaseService;
        _imageService = imageService;
        _modalService = modalService;
        _gameDataProviders = gameDataProviders;
    }

    public void RunGame(int id) {
        // TODO Implement
    }
        
    public async Task UpdateSource(ILibrarySource source, EventHandler<float> onProgress) {
        var sourceRecord = _databaseService.GetLibrarySourceByName(source.Name());
        if (sourceRecord == null) return;

        //var progress = new Progress<float>();
        //progress.ProgressChanged += onProgress;

        var gamesInSource = source.GamesIds().Result.ToList();
        var gamesInLibrary = _databaseService.GetAllGames(sourceRecord)?.Select(g => g.OriginalId).ToList();
        
        var newGames = gamesInSource.Where(sourceId => !gamesInLibrary.Contains(sourceId)).ToArray();
        var removedGames = gamesInLibrary.Where(sourceId => !gamesInSource.Contains(sourceId)).ToArray();

        if (removedGames.Length > 0) {
            _databaseService.DeleteByOriginalIds(removedGames.ToList());
        }
            
        for (int i = 0; i < newGames.Length; i++) {
            //((IProgress<float>) progress).Report((float)i / newGames.Length);
                
            await AddGameToLibrary(source, newGames[i], sourceRecord);
        }
    }

    private async Task AddGameToLibrary(ILibrarySource source, string sourceId, LibrarySource sourceRecord) {

        try {
            // Throw source-level exception
            var gameData = await source.Game(sourceId);

            // Throw image-level exception
            var gameRecord = new Game {
                OriginalId = gameData.SourceId,
                Source = sourceRecord,
                Name = gameData.Name,
                Description = gameData.Description,
                Artwork = await _imageService.Save(gameData.ArtworkUrl, ImageService.ImageType.Artwork,
                    gameData.Name),
                Background = await _imageService.Save(gameData.BackgroundUrl, ImageService.ImageType.Background,
                    gameData.Name),
                LastPlayed = new DateTime()
            };
            
            _databaseService.Add(gameRecord);
        }
        catch (LibrarySourceGameDataException) {
            if (await _modalService.Confirm(
                    "Error with getting information from " + source.Name() + " about game #" + sourceId, "Try again",
                    "Skip the game")) {
                await AddGameToLibrary(source, sourceId, sourceRecord);
            }
        }
        catch (LibraryGraphicsException e) {
            if (await _modalService.Confirm(
                    $"Error downloading artwork or background for #{sourceId}:\n{e.Message}" , "Try again",
                    "Skip the game")) {
                await AddGameToLibrary(source, sourceId, sourceRecord);
            }
        }
        catch (Exception e) {
            if (await _modalService.Confirm(
                    "Something went wrong with #" + sourceId, "Try again",
                    "Skip the game")) {
                await AddGameToLibrary(source, sourceId, sourceRecord);
            }
        }
    }

    // Adds source to the table, but don't import games from it
    public void AddSource(ILibrarySource source) {
            
        // Check if library source is already added
        if (_databaseService.GetLibrarySourceByName(source.Name()) != null)
            return;
            
        var record = new LibrarySource {
            Name = source.Name(),
            GamesCount = source.GamesCount()
        };
        
        _databaseService.Add(record);
    }

    private IEnumerable<IGameDataProvider> GetProviders<T, T1>() {

        return _gameDataProviders
            .Where(provider => provider is IGameDataProvider<T, T1>)
            .OrderBy(provider => provider.Priority());
    }
        
    public async Task<T1> GetData<T, T1>(string gameName) where T: GameDataType, new() {
        var dataType = new T();

        // Iterate over providers, try to get data for the game
        foreach (var provider in  GetProviders<T, T1>()) {
            if (await dataType.Accept<T1>(provider, gameName, out var data))
                return data;
        }

        return default;
    }
}