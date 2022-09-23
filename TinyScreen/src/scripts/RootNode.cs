using Godot;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
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

        private void CheckScene() {
            if (_currentScene == null) {
                _currentScene = Onboarding.Instance() as BaseRouter;
                AddChild(_currentScene);
            }
            else if (_currentScene.Filename != Onboarding.ResourcePath) {
                RemoveChild(_currentScene);
                _currentScene = Onboarding.Instance() as BaseRouter;
                AddChild(_currentScene);
            }
        }

        [Route("onboarding")]
        private async void OnboardingRoute(string path) {
            CheckScene();
            await ToSignal(GetTree(), "idle_frame");
            _currentScene.Navigate(path);
        }

        [Route("application")]
        private async void ApplicationRoute(string path, bool showTutorial = false) {
            CheckScene();
            _databaseService.InitDatabase();

            _currentScene = Application.Instance() as BaseRouter;
            AddChild(_currentScene);
            await ToSignal(GetTree(), "idle_frame");
            _currentScene.Navigate(path);
        }
    }
}