using Godot;

namespace TinyScreen.Scripts.Onboarding {
    public class Onboarding : Control {

        [Export] public PackedScene WelcomeScene;
        [Export] public PackedScene UpdateScene;
        [Export] public PackedScene LibraryScene;

        private enum Stage {
            Welcome,
            Update,
            Library,
            Emulation,
            Device,
            Widgets
        }

        private Stage _currentStage = Stage.Welcome;
        private Control _currentScene;
        private Tween _tween;
        
        public override void _Ready() {
            base._Ready();
            _tween = new Tween();
            AddChild(_tween);
            SetStage(_currentStage);
        }
        
        private void SetStage(Stage newStage) {
            _currentStage = newStage;
            
            switch (newStage) {
                case Stage.Welcome:
                    TransitionToScene(WelcomeScene);
                    break;
                
                case Stage.Update:
                    TransitionToScene(UpdateScene);
                    break;
                
                default:
                    TransitionToScene(LibraryScene);
                    break;
            }
        }

        public void NextStage() {
            if (_currentStage != Stage.Widgets) {
                SetStage(_currentStage + 1);
                return;
            }
            
            // At this point onboarding is finished
            // Persist settings and games
            // Open application
        }

        private async void TransitionToScene(PackedScene newScene) {
            
            // If there is no scene, just show it (for now)
            if (_currentScene == null) {
                _currentScene = newScene.Instance() as Control;
                AddChild(_currentScene);
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
                _tween.InterpolateProperty(newSceneInstance, "modulate", new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.3f, Tween.TransitionType.Cubic);
                _tween.InterpolateProperty(newSceneInstance, "rect_position", newSceneInstance.RectPosition, new Vector2(0, 0), 0.3f, Tween.TransitionType.Cubic);
                
                // Hide old
                _tween.InterpolateProperty(_currentScene, "modulate", new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 0.3f, Tween.TransitionType.Cubic);
                _tween.InterpolateProperty(_currentScene, "rect_position", _currentScene.RectPosition, new Vector2(-50, 0), 0.3f, Tween.TransitionType.Cubic);
                
                _tween.Start();
                AddChild(newSceneInstance);
                
                await ToSignal(_tween, "tween_all_completed");
                
                RemoveChild(_currentScene);
                _currentScene = newSceneInstance;
            }
        }
    }
}