using System;
using System.Threading.Tasks;
using Common.Framework;
using Common.Interfaces;

namespace TemplateLibrarySource; 

public class TemplateLibrarySource : BaseLibrarySource {
    public override string Name() {
        throw new NotImplementedException();
    }
        
    public override int GamesCount() {
        throw new NotImplementedException();
    }

    public override bool IsInstalled() {
        throw new NotImplementedException();
    }

    public override async Task<string[]> GamesIds() {
        throw new NotImplementedException();
    }

    public override async Task<LibrarySourceGameData> Game(string sourceId) {
        throw new NotImplementedException();
    }
}