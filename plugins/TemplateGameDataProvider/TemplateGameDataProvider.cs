using Common.Framework;
using Common.Interfaces;

namespace TestGameDataProvider {
    
    /**
     * Extend this class by implementing IGameDataProvider<GameDataType> interface.
     * For instance TemplateGameDataProvider : IGameDataProvider, IGameDataProvider<ArtworkGameDataType>
     */
    public class TemplateGameDataProvider : IGameDataProvider {

        public int Priority() => 1;

        /*
        public string GetData(ArtworkGameDataType dataType, string gameName) {
            throw new NotImplementedException();
        }
        */
    }
}