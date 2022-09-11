using System.Threading.Tasks;
using TinyScreen.Models;

namespace TinyScreen.Framework.Interfaces {
    public interface ILibrarySource {
        string Name();

        byte[] Icon();

        int GamesCount();

        bool IsInstalled();

        Task<int[]> GamesIds();

        Task<LibrarySourceGameData> Game(int sourceId);
    }
    
    public class LibrarySourceGameData {
        public int SourceId;
        public string Name;
        public string Description;
        public string ArtworkUrl;
        public string BackgroundUrl;
    }
}