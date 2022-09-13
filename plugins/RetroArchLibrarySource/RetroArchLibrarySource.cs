using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TinyScreen.Framework.Interfaces;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;
using TinyScreen.Framework;

namespace RetroArchLibrarySource {
    
    public class RetroArchLibrarySource : BaseLibrarySource {
        
        private const string _registryKey = "Software\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\RetroArch";
       
        private const string _steamId = "1118310";
        private RetroArchHelper _retroArchHelper;

        private List<IRetroArchInstallation> _installations = new List<IRetroArchInstallation> {
            new StandaloneInstallation(), // Priority is important!
            new SteamInstallation()
        };

        private IRetroArchInstallation _installation;

        public RetroArchLibrarySource() {
            
            // When library tries to resolve another assembly, we try to do this using embedded resources
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
            
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
            return new [] {"test"};
        }

        public override async Task<LibrarySourceGameData> Game(string sourceId) {
            return new LibrarySourceGameData();
        }
    }
}