using Godot;
using TinyScreen.Framework.Attributes;
using TinyScreen.framework.interfaces;

namespace TinyScreen.scripts {
    public class RootNode: Control {

        [Inject] public ISettingsInterface _settingsService;
        
        public override void _Ready() {
            base._Ready();
            
            // Application is not installing, we need onboarding!
            if (!_settingsService.IsAppInstalled()) {
            
                // Install application
                _settingsService.InstallApp();
            }
            
            // Application is installed, show it
            else {
                
                
            }
            
        }
    }
}