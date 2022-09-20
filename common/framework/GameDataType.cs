using System;
using System.Threading.Tasks;
using Common.Interfaces;

namespace Common.Framework {
    public abstract class GameDataType {
        public abstract Task<string> Accept(IGameDataProvider provider, string gameName);
    }

    public class ArtworkGameDataType : GameDataType {
        public override Task<string> Accept(IGameDataProvider provider, string gameName) {
            if (provider is IGameDataProvider<ArtworkGameDataType> artworkProvider)
                return artworkProvider.GetData(this, gameName);

            throw new NotImplementedException();
        }
    }

    public class BackgroundGameDataType : GameDataType {
        public override Task<string> Accept(IGameDataProvider provider, string gameName) {
            if (provider is IGameDataProvider<BackgroundGameDataType> artworkProvider)
                return artworkProvider.GetData(this, gameName);

            throw new NotImplementedException();
        }
    }

    public class DescriptionGameDataType : GameDataType {
        public override Task<string> Accept(IGameDataProvider provider, string gameName) {
            if (provider is IGameDataProvider<DescriptionGameDataType> artworkProvider)
                return artworkProvider.GetData(this, gameName);

            throw new NotImplementedException();
        }
    }
}