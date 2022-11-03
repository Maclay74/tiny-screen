using System;
using System.Threading.Tasks;
using Common.Interfaces;

namespace Common.Framework;

public abstract class GameDataType {
    public abstract Task<ReturnType?> Accept<ReturnType>(IGameDataProvider provider, string gameName);
}

public class ArtworkGameDataType : GameDataType {
    public override Task<ReturnType?> Accept<ReturnType>(IGameDataProvider provider, string gameName)
        where ReturnType : default {
        if (provider is IGameDataProvider<ArtworkGameDataType, ReturnType> artworkProvider)
            return artworkProvider.GetData(this, gameName);

        throw new NotImplementedException();
    }
}

public class BackgroundGameDataType : GameDataType {
    public override Task<ReturnType?> Accept<ReturnType>(IGameDataProvider provider, string gameName)
        where ReturnType : default {
        if (provider is IGameDataProvider<BackgroundGameDataType, ReturnType> artworkProvider)
            return artworkProvider.GetData(this, gameName);

        throw new NotImplementedException();
    }
}

public class DescriptionGameDataType : GameDataType {
    public override Task<ReturnType?> Accept<ReturnType>(IGameDataProvider provider, string gameName)
        where ReturnType : default {
        if (provider is IGameDataProvider<DescriptionGameDataType, ReturnType> artworkProvider)
            return artworkProvider.GetData(this, gameName);

        throw new NotImplementedException();
    }
}