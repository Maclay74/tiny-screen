using System;
using System.Threading.Tasks;
using Common.Framework;
using Common.Interfaces;

namespace TemplateGameDataProvider {
    
    /**
     * Extend this class by implementing IGameDataProvider<GameDataType, ReturnType> interface.
     * For instance TemplateGameDataProvider : IGameDataProvider, IGameDataProvider<ArtworkGameDataType, byte[]
     */
    public class TemplateGameDataProvider : IGameDataProvider {

        public int Priority() => 1;
        
        /*
         public Task<bool> GetData(ArtworkGameDataType dataType, string gameName, out byte[] response) {
            throw new NotImplementedException();
        }
        */
    }
}