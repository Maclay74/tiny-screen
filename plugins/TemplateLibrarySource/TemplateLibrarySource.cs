using System;
using System.IO;
using System.Linq;
using TinyScreen.Framework.Interfaces;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace TemplateLibrarySource {
    public class TemplateLibrarySource : ILibrarySource {
        
        public TemplateLibrarySource() {
            
            // When library tries to resolve another assembly, we try to do this using embedded resources
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }
        
        public string Name() {
            throw new NotImplementedException();
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
             throw new NotImplementedException();
        }

        public bool IsInstalled() {
             throw new NotImplementedException();
        }

        public async Task<string[]> GamesIds() {
             throw new NotImplementedException();
        }

        public async Task<LibrarySourceGameData> Game(string sourceId) {
             throw new NotImplementedException();
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