using Godot;
using GodotOnReady.Attributes;

namespace TinyScreen.Scripts.Application {
    public partial class Application : Control {

        private enum State {
            Home,
            Games,
            Settings
        }
        
        [OnReadyGet] private Control _content;

        [Export] private PackedScene _home;
        [Export] private PackedScene _games;
        [Export] private PackedScene _settings;

        [OnReadyGet] private Button _homeButton;
        [OnReadyGet] private Button _gamesButton;
        [OnReadyGet] private Button _settingsButton;

        private State _currentState = State.Home;
        private Control _currentScene;
        private Tween _tween;

        [OnReady]
        private void BindEvents() {
            _homeButton.Connect("pressed", this, nameof(SetState), new Godot.Collections.Array(State.Home));
            _gamesButton.Connect("pressed", this, nameof(SetState), new Godot.Collections.Array(State.Games));
            _settingsButton.Connect("pressed", this, nameof(SetState), new Godot.Collections.Array(State.Settings));
        }

        [OnReady]
        private void Start() {
            _tween = new Tween();
            AddChild(_tween);
            SetState(_currentState);
        }

        private void SetState(State newState) {
            
            if (_currentState == newState && _currentScene != null)
                return;
           
            _currentState = newState;
            
            switch (newState) {
                case State.Home:
                    TransitionToScene(_home);
                    break;
                
                case State.Games:
                    TransitionToScene(_games);
                    break;
                
                case State.Settings:
                    TransitionToScene(_settings);
                    break;
            }
        }
        
        private async void TransitionToScene(PackedScene newScene) {
            
            // If there is no scene, just show it (for now)
            if (_currentScene == null) {
                _currentScene = newScene.Instance() as Control;
                _content.AddChild(_currentScene);
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
                _content.AddChild(newSceneInstance);
                
                await ToSignal(_tween, "tween_all_completed");
                
                _content.RemoveChild(_currentScene);
                _currentScene = newSceneInstance;
            }
        }
    }
}