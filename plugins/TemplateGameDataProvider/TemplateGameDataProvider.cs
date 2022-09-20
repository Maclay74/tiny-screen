using System;
using System.Threading.Tasks;
using Common.Framework;
using Common.Interfaces;

namespace TemplateGameDataProvider {
    
    /**
     * Extend this class by implementing IGameDataProvider<GameDataType> interface.
     * For instance TemplateGameDataProvider : IGameDataProvider, IGameDataProvider<ArtworkGameDataType>
     */
    public class TemplateGameDataProvider : IGameDataProvider {

        public int Priority() => 1;

        /*
        public Task<string> GetData(ArtworkGameDataType dataType, string gameName) {
            throw new NotImplementedException();
        }
        */
    }
}