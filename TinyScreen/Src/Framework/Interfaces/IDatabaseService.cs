using System.Collections.Generic;
using TinyScreen.Models;

namespace TinyScreen.Framework.Interfaces; 

public interface IDatabaseService {
    bool Exists();
    void InitDatabase();
        
    // Library sources

    void Add(LibrarySource source);
    LibrarySource? GetLibrarySourceByName(string name);
        
    public void Delete(LibrarySource source);
    public void DeleteRange(List<LibrarySource> sources);
       
        
        
    // Games

    List<Game>? GetAllGames(LibrarySource source);
        
    public void Delete(Game game);
    public void DeleteRange(List<Game> games);

    public void DeleteByOriginalIds(List<string> originalIds);
        
    void Add(Game game);
    
    
    // Settings

    public void SetSettings(string name, string value);

    public string GetSettings(string name);
}