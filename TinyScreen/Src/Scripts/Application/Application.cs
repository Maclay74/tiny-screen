using Godot;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;

namespace TinyScreen.Scripts.Application; 

public partial class Application : BaseRouter {
    [Export] private Control _content;

    [Export] private PackedScene _home;
    [Export] private PackedScene _games;
    [Export] private PackedScene _settings;

    [Export] private Button _homeButton;
    [Export] private Button _gamesButton;
    [Export] private Button _settingsButton;

    private Control _currentScene;
    private Tween _tween;
        
    public override partial void _Ready();
        
    [Ready]
    private void BindEvents() {
        _homeButton.Pressed += () => Navigate("home");
        _gamesButton.Pressed += () => Navigate("games");
        _settingsButton.Pressed += () => Navigate("settings");
    }
        
    [Route("home")]
    private void Home(string path) {
        TransitionToScene(_home, path);
    }

    [Route("games", true)]
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
            _currentScene = newScene.Instantiate() as Control;
            _content.AddChild(_currentScene);
            await ToSignal(GetTree(), "process_frame");
            if (_currentScene is BaseRouter router)
                router.Navigate(path);
            return;
        }

        // If previous transition isn't finished yet
        if (_tween != null && _tween.IsRunning()) {
            await ToSignal(_tween, "finished");
            await ToSignal(GetTree(), "process_frame");
        }

        // If we have scene, show transition
        if (newScene.Instantiate() is Control newSceneInstance) {
            // Move new scene slightly to the right and hide
            newSceneInstance.Position = new Vector2(50, 0);
            newSceneInstance.Modulate = new Color(1, 1, 1, 0);
                
            _tween = CreateTween()
                .SetTrans(Tween.TransitionType.Cubic)
                .SetEase(Tween.EaseType.InOut);
                
            // Show new
            _tween
                .Parallel()
                .TweenProperty(newSceneInstance, "modulate", new Color(1, 1, 1), 0.3f)
                .FromCurrent();
                
            _tween
                .Parallel()
                .TweenProperty(newSceneInstance, "position", new Vector2(0, 0), 0.3f)
                .FromCurrent();
                
            _tween
                .Parallel()
                .TweenProperty(_currentScene, "modulate", new Color(1, 1, 1, 0), 0.3f)
                .FromCurrent();
                
            _tween
                .Parallel()
                .TweenProperty(_currentScene, "position", new Vector2(-50, 0), 0.3f)
                .FromCurrent();
                
            _content.AddChild(newSceneInstance);
            await ToSignal(GetTree(), "process_frame");
            if (_currentScene is BaseRouter router)
                router.Navigate(path);

            await ToSignal(_tween, "finished");

            _content.RemoveChild(_currentScene);
            _currentScene = newSceneInstance;
        }
    }
}