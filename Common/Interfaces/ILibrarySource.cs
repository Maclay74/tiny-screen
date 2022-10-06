using System.Threading.Tasks;

namespace Common.Interfaces; 

public interface ILibrarySource {
    string Name();

    byte[] Icon();

    int GamesCount();

    bool IsInstalled();

    Task<string[]> GamesIds();

    Task<LibrarySourceGameData> Game(string sourceId);
}
    
public class LibrarySourceGameData {
    public string SourceId;
    public string Name;
    public string Description;
    public string ArtworkUrl;
    public string BackgroundUrl;
}