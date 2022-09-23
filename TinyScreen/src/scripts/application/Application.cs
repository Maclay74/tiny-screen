using Godot;
using GodotOnReady.Attributes;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;

namespace TinyScreen.Scripts.Application {
    public partial class Application : BaseRouter {
        [OnReadyGet] private Control _content;

        [Export] private PackedScene _home;
        [Export] private PackedScene _games;
        [Export] private PackedScene _settings;

        [OnReadyGet] private Button _homeButton;
        [OnReadyGet] private Button _gamesButton;
        [OnReadyGet] private Button _settingsButton;

        private Control _currentScene;
        private Tween _tween;

        [OnReady]
        private void BindEvents() {
            _homeButton.Connect("pressed", this, nameof(OnHomePress));
            _gamesButton.Connect("pressed", this, nameof(OnGamesPress));
            _settingsButton.Connect("pressed", this, nameof(OnSettingsPress));
        }

        private void OnHomePress() => Navigate("home");
        private void OnGamesPress() => Navigate("games");
        private void OnSettingsPress() => Navigate("settings");

        [OnReady]
        private void Start() {
            _tween = new Tween();
            AddChild(_tween);
        }

        [Route("home", true)]
        private void Home(string path) {
            TransitionToScene(_home, path);
        }

        [Route("games")]
        private void Games(string path) {
            TransitionToScene(_games, path);
        }

        [Route("settings")]
        private void Settings(string path) {
            TransitionToScene(_settings, path);
        }

        private async void TransitionToScene(PackedScene newScene, string path) {
            // If there is no scene, just show it (for now)
            if (_currentScene == null) {
                _currentScene = newScene.Instance() as Control;
                _content.AddChild(_currentScene);
                await ToSignal(GetTree(), "idle_frame");
                if (_currentScene is BaseRouter router)
                    router.Navigate(path);
                return;
            }

            // If previous transition isn't finished yet
            if (_tween.IsActive()) {
                await ToSignal(_tween, "tween_all_completed");
                await ToSignal(GetTree(), "idle_frame");
            }

            // If we have scene, show transition
            if (newScene.Instance() is Control newSceneInstance) {
                // Move new scene slightly to the right and hide
                newSceneInstance.RectPosition = new Vector2(50, 0);
                newSceneInstance.Modulate = new Color(1, 1, 1, 0);
                // Show new
                _tween.InterpolateProperty(newSceneInstance, "modulate", new Color(1, 1, 1, 0), new Color(1, 1, 1, 1),
                    0.3f, Tween.TransitionType.Cubic);
                _tween.InterpolateProperty(newSceneInstance, "rect_position", newSceneInstance.RectPosition,
                    new Vector2(0, 0), 0.3f, Tween.TransitionType.Cubic);

                // Hide old
                _tween.InterpolateProperty(_currentScene, "modulate", new Color(1, 1, 1, 1), new Color(1, 1, 1, 0),
                    0.3f, Tween.TransitionType.Cubic);
                _tween.InterpolateProperty(_currentScene, "rect_position", _currentScene.RectPosition,
                    new Vector2(-50, 0), 0.3f, Tween.TransitionType.Cubic);

                _tween.Start();
                _content.AddChild(newSceneInstance);
                await ToSignal(GetTree(), "idle_frame");
                if (_currentScene is BaseRouter router)
                    router.Navigate(path);

                await ToSignal(_tween, "tween_all_completed");

                _content.RemoveChild(_currentScene);
                _currentScene = newSceneInstance;
            }
        }
    }
}