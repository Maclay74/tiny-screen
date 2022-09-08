using TinyScreen.Framework.Interfaces;
using TinyScreen.Services;

namespace TinyScreen.Services {
    public class HardwareFactory {
        
        public static IHardwareService GetHardwareService() {
        
            // TODO Return correct hardware service instead!
            return new GenericHardwareService();
        }
    }
}