using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Framework;
using Common.Interfaces;

namespace RetroArchLibrarySource; 

public class RetroArchLibrarySource : BaseLibrarySource {
    private RetroArchHelper _retroArchHelper;

    private List<IRetroArchInstallation> _installations = new List<IRetroArchInstallation> {
        new StandaloneInstallation(), // Order is important!
        new SteamInstallation()
    };

    private IRetroArchInstallation _installation;

    public RetroArchLibrarySource() {
        foreach (var installation in _installations) {
            if (installation.Installed()) {
                _installation = installation;
                break;
            }
        }

        _retroArchHelper = new RetroArchHelper(_installation);
    }

    public override string Name() {
        return "RetroArch";
    }

    public override int GamesCount() {
        if (!IsInstalled()) return 0;
        return _retroArchHelper.GamesCount();
    }

    public override bool IsInstalled() {
        return _installation != null && _installation.Installed();
    }

    public override async Task<string[]> GamesIds() {
        try {
            return _retroArchHelper
                .GetRomsInPlayLists().Select(game => game.SourceId)
                .ToArray();
        }
        catch (Exception) {
            throw new LibraryParseException();
        }
    }

    public override async Task<LibrarySourceGameData> Game(string sourceId) {
        try {
            return _retroArchHelper
                .GetRomsInPlayLists().Find(game => game.SourceId == sourceId);
        }
        catch (Exception) {
            throw new LibrarySourceGameDataException();
        }
    }
}