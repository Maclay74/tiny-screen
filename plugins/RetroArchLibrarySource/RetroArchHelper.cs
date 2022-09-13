using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RetroArchLibrarySource {
    internal class RetroArchHelper {

        private IRetroArchInstallation _installation;

        public RetroArchHelper(IRetroArchInstallation installation) {
            _installation = installation;
        }
        
        public int GamesCount() => GetRomsInPlayLists().Count;

        private List<string> GetRomsInPlayLists() {
            var roms = new List<string>();

            var playlistsPath = Path.Combine(_installation.InstallPath(), "playlists");
            if (!Directory.Exists(playlistsPath))
                return roms;

            foreach (var playlist in Directory.GetFiles(playlistsPath, ".lpl")) {
                Console.WriteLine(playlist);
            }
            
            return roms;
        }
    }
}