using Godot;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Scripts.Application;
using static SQLitePCL.Batteries_V2; // Mono won't pack the library without import

namespace TinyScreen.scripts {
    [Router]
    public class RootNode : BaseRouter {
        [Inject] private ISettingsService _settingsService;
        [Inject] private IDatabaseService _databaseService;
        
        [Export] private PackedScene Onboarding;
        [Export] private PackedScene Application;

        private BaseRouter _currentScene;

        public override async void _Ready() {
            base._Ready();
            
            if (!_settingsService.IsAppInstalled()) {
                Navigate("/onboarding");
            }
            else {
                Navigate("/application", false);
            }
            
        }
        
        [Route("onboarding")]
        private async void OnboardingRoute(string path) {
            _currentScene = Onboarding.Instance() as BaseRouter;
            AddChild(_currentScene);
            _currentScene.Navigate(path);
        }
        
        [Route("application")]
        private void ApplicationRoute(string path, bool showTutorial = false) {
            if (_currentScene != null) {
                RemoveChild(_currentScene);
            }
            _databaseService.InitDatabase();

            var application = Application.Instance() as Application;
            AddChild(application);
        }
    }
}