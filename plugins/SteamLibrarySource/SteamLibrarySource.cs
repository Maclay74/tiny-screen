using TinyScreen.Framework.Interfaces;

namespace SteamLibrarySource {
    public class SteamLibrarySource: ILibrarySource {
        public string GetName() {
            return "Steam";
        }
    }
}