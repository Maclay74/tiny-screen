using System;
using System.IO;
using System.Linq;
using TinyScreen.Framework.Interfaces;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SteamLibrarySource {
    public class SteamLibrarySource : ILibrarySource {
        private const string _registryKey = "Software\\Wow6432Node\\Valve\\Steam";

        private SteamService _steamService;

        string ILibrarySource.Name() => "Steam";

        public SteamLibrarySource() {
            _steamService = new SteamService(Registry.LocalMachine.OpenSubKey(_registryKey));
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        public byte[] Icon() {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SteamLibrarySource.assets.icon.png";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public int GamesCount() {
            if (!IsInstalled()) return 0;

            return _steamService.GamesCount();
        }

        public bool IsInstalled() {
            RegistryKey steamKey = Registry.LocalMachine.OpenSubKey(_registryKey);
            return steamKey != null;
        }

        public async Task<int[]> GamesIds() {
            return _steamService.GetInstalledGamesIds()
                .Select(Int32.Parse)
                .ToArray();
        }

        public async Task<LibrarySourceGameData> Game(int sourceId) {
            return await _steamService.GetGameInfo(sourceId);
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
    }
}