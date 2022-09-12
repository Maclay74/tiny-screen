using System;
using System.IO;
using System.Linq;
using TinyScreen.Framework.Interfaces;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SteamLibrarySource {
    public class SteamLibrarySource : ILibrarySource {
    
        // Steam system-level information there
        private const string _registryKey = "Software\\Wow6432Node\\Valve\\Steam";

        // Helper for steam
        private SteamHelper _steamHelper;
        
        string ILibrarySource.Name() => "Steam";
        
        public SteamLibrarySource() {
            _steamHelper = new SteamHelper(Registry.LocalMachine.OpenSubKey(_registryKey));
        
            // When library tries to resolve another assembly, we try to do this using embedded resources
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        public byte[] Icon() {
            var assembly = Assembly.GetExecutingAssembly();
            
            // Get icon from embedded resources
            var resourceName = Assembly.GetExecutingAssembly().GetName().Name + ".assets.icon.png";

            // Convert it to byte array and return
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public int GamesCount() {
            if (!IsInstalled()) return 0;
            return _steamHelper.GamesCount();
        }

        public bool IsInstalled() {
            
            // If key present in the system, application is installed
            RegistryKey steamKey = Registry.LocalMachine.OpenSubKey(_registryKey);
            return steamKey != null;
        }

        public async Task<int[]> GamesIds() {
            return _steamHelper.GetInstalledGamesIds()
                .Select(Int32.Parse)
                .ToArray();
        }

        public async Task<LibrarySourceGameData> Game(int sourceId) {
            return await _steamHelper.GetGameInfo(sourceId);
        }

        private Assembly ResolveAssembly(object sender, ResolveEventArgs args) {
            string keyName = new AssemblyName(args.Name).Name;
            
            // Try to find library in embedded resources
            using (var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + ".assets." + keyName + ".dll")) {
                var assemblyData = new Byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }
    }
}