using System.IO;

namespace TinyScreen.Framework.Interfaces {
    public interface ILibrarySource {
        string Name();

        byte[] Icon();

        int GamesCount();

        bool IsInstalled();
    }
}