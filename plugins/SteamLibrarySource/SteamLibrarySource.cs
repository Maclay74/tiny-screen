using System.Data.SqlTypes;
using System.IO;
using TinyScreen.Framework.Interfaces;
using System.Reflection;

namespace SteamLibrarySource {
    public class SteamLibrarySource : ILibrarySource {
        
        string ILibrarySource.Name() => "Steam";

        public byte[] Icon() {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SteamLibrarySource.assets.icon.png";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public int GamesCount() => 154;

        public bool IsInstalled() {
            return false;
        }
    }
}