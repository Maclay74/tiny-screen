using System;
using System.Threading.Tasks;
using Common.Interfaces;

namespace Common.Framework {
    public abstract class GameDataType {
        public abstract Task<bool> Accept<ReturnType>(IGameDataProvider provider, string gameName,
            out ReturnType response);
    }

    public class ArtworkGameDataType : GameDataType {
        public override Task<bool> Accept<ReturnType>(IGameDataProvider provider, string gameName, out ReturnType response) {
            if (provider is IGameDataProvider<ArtworkGameDataType, ReturnType> artworkProvider)
                return artworkProvider.GetData(this, gameName, out response);

            throw new NotImplementedException();
        }
    }

    public class BackgroundGameDataType : GameDataType {
        public override Task<bool> Accept<ReturnType>(IGameDataProvider provider, string gameName, out ReturnType response) {
            if (provider is IGameDataProvider<BackgroundGameDataType, ReturnType> artworkProvider)
                return artworkProvider.GetData(this, gameName, out response);

            throw new NotImplementedException();
        }
    }

    public class DescriptionGameDataType : GameDataType {
        public override Task<bool> Accept<ReturnType>(IGameDataProvider provider, string gameName, out ReturnType response) {
            if (provider is IGameDataProvider<DescriptionGameDataType, ReturnType> artworkProvider)
                return artworkProvider.GetData(this, gameName, out response);

            throw new NotImplementedException();
        }
    }
}