using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Common.Interfaces;

namespace Common.Framework {
    public abstract class BaseLibrarySource: ILibrarySource {

        public BaseLibrarySource() {
            //AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }
        
        public abstract string Name();

        public abstract int GamesCount();

        public abstract bool IsInstalled();

        public abstract Task<string[]> GamesIds();

        public abstract Task<LibrarySourceGameData> Game(string sourceId);
        
        public byte[] Icon() {
            var assembly = GetType().Assembly;
            var resourceName = assembly.GetName().Name + ".Assets.icon.png";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        
        protected Assembly ResolveAssembly(object sender, ResolveEventArgs args) {
            string keyName = new AssemblyName(args.Name).Name;
            var assembly = GetType().Assembly;
            var currentContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
            
            if (args.RequestingAssembly != assembly)
                return null;

            using var stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Assets." + keyName + ".dll");
            var assemblyData = new Byte[stream.Length];
            //stream.Read(assemblyData, 0, assemblyData.Length);
            return currentContext.LoadFromStream(stream);
            //return Assembly.Load(assemblyData);
        }
    }
}