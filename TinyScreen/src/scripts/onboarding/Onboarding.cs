using Godot;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;

namespace TinyScreen.Scripts.Onboarding {
    public class Onboarding : BaseRouter {
        [Export] public PackedScene WelcomeScene;
        [Export] public PackedScene UpdateScene;
        [Export] public PackedScene LibraryScene;
        [Export] public PackedScene FinishScene;

        [Inject] private ISettingsService _settingsService;

        private Control _currentScene;
        private Tween _tween;

        public override void _Ready() {
            base._Ready();
            _settingsService.InstallApp();

            _tween = new Tween();
            AddChild(_tween);
        }

        private async void TransitionToScene(PackedScene newScene, string path) {
            // If there is no scene, just show it (for now)
            if (_currentScene == null) {
                _currentScene = newScene.Instance() as Control;
                AddChild(_currentScene);
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
                AddChild(newSceneInstance);
                await ToSignal(GetTree(), "idle_frame");
                if (_currentScene is BaseRouter router)
                    router.Navigate(path);

                await ToSignal(_tween, "tween_all_completed");

                RemoveChild(_currentScene);
                _currentScene = newSceneInstance;
            }
        }

        [Route("welcome", true)]
        private void Welcome(string path) {
            TransitionToScene(WelcomeScene, path);
        }

        [Route("update")]
        private void Update(string path) {
            TransitionToScene(UpdateScene, path);
        }

        [Route("library")]
        private void Library(string path) {
            TransitionToScene(LibraryScene, path);
        }

        [Route("finished")]
        private void Finished(string path) {
            TransitionToScene(FinishScene, path);
        }
    }
}