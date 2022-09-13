using System;
using System.Linq;
using System.Threading.Tasks;
using ExpressionTree;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Models;

namespace TinyScreen.Services {
    public class LibraryService {

        private IDatabaseService _databaseService;
        private ImageService _imageService;

        public LibraryService(IDatabaseService databaseService, ImageService imageService) {
            _databaseService = databaseService;
            _imageService = imageService;
        }

        public void RunGame(int id) {
            // TODO Implement
        }
        
        public async Task UpdateSource(ILibrarySource source, EventHandler<float> onProgress) {
            var sourceRecord = GetSourceRecordByName(source.Name());
            if (sourceRecord == null) return;

            var ids = await source.GamesIds();
            var progress = new Progress<float>();
            progress.ProgressChanged += onProgress;

            // Get all source-level ids
            foreach (var item in ids.Select((value, i) => (value, i))) {
                ((IProgress<float>) progress).Report((int)((float)item.i / ids.Length * 100));
                
                // If we have this game in the library, skip it
                if (GetGameBySourceId(item.value) != null)
                    continue;

                var gameData = await source.Game(item.value);
                if (gameData == null) continue;

                // Map gameData to Games model
                var gameRecord = new Games {
                    SourceId = gameData.SourceId,
                    Source = sourceRecord.Id,
                    Name = gameData.Name,
                    Description = gameData.Description,
                    Artwork = await _imageService.Save(gameData.ArtworkUrl, ImageService.ImageType.Artwork, gameData.Name),
                    Background = await _imageService.Save(gameData.BackgroundUrl, ImageService.ImageType.Background, gameData.Name)
                };
                
                // Save game to database
                _databaseService.Insert(gameRecord);
            }
            ((IProgress<float>) progress).Report(100f);
        }

        // Adds source to the table, but don't import games from it
        public void AddSource(ILibrarySource source) {
            
            // Check if library source is already added
            if (GetSourceRecordByName(source.Name()) != null) return;
            
            var record = new LibrarySources {
                Name = source.Name(),
                GamesCount = source.GamesCount()
            };
            
            _databaseService.Insert(record);
        }

        // Remove source from the table, removes all games too 
        public void RemoveSource(ILibrarySource source) {
            // TODO Implement
        }

        private LibrarySources GetSourceRecordByName(string name) {
            return _databaseService.Select<LibrarySources>(new Expr("name", OperatorEnum.Equals, name));
        }

        private Games GetGameBySourceId(string sourceId) {
            return _databaseService.Select<Games>(new Expr("sourceId", OperatorEnum.Equals, sourceId));
        }
    }
}