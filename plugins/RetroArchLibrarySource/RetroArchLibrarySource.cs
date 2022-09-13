using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TinyScreen.Framework.Interfaces;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace RetroArchLibrarySource {
    
   
    
    public class RetroArchLibrarySource : ILibrarySource {
        
        private const string _registryKey = "Software\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\RetroArch";
       
        private const string _steamId = "1118310";
        private RetroArchHelper _retroArchHelper;

        private List<IRetroArchInstallation> _installations = new List<IRetroArchInstallation> {
            new SteamInstallation(),
            new StandaloneInstallation()
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
        
            // When library tries to resolve another assembly, we try to do this using embedded resources
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }
        
        public string Name() {
            return "RetroArch";
        }

        public byte[] Icon() {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = Assembly.GetExecutingAssembly().GetName().Name + ".assets.icon.png";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public int GamesCount() {
            if (!IsInstalled()) return 0;
            return 0;
        }

        public bool IsInstalled() {
            return _installation != null && _installation.Installed();
        }

        public async Task<int[]> GamesIds() {
            return new [] {1};
        }

        public async Task<LibrarySourceGameData> Game(int sourceId) {
            return new LibrarySourceGameData();
        }

        private Assembly ResolveAssembly(object sender, ResolveEventArgs args) {
            string keyName = new AssemblyName(args.Name).Name;
            
            using (var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + ".assets." + keyName + ".dll")) {
                var assemblyData = new Byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }

        private bool IsInstalledStandalone() {
            RegistryKey steamKey = Registry.LocalMachine.OpenSubKey(_registryKey);
            if (steamKey == null) return false;
            
            // If path is not set
            string installPath = steamKey.GetValue("DisplayIcon")?.ToString();
            if (installPath == null) return false;

            // Check if installer exists
            return File.Exists(Path.Combine(installPath));
        }

 
    }
}