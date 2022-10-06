using Godot;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;

namespace TinyScreen.Scripts.Onboarding; 

public partial class Onboarding : BaseRouter {
    [Export] public PackedScene WelcomeScene;
    [Export] public PackedScene UpdateScene;
    [Export] public PackedScene LibraryScene;
    [Export] public PackedScene FinishScene;

    [Inject] private ISettingsService _settingsService;

    private Control _currentScene;
    private Tween _tween;
        
    public override partial void _Ready();

    [Ready]
    private void Start() {
        base._Ready();
        _settingsService.InstallApp();
    }

    private async void TransitionToScene(PackedScene newScene, string path) {
            
        // If there is no scene, just show it (for now)
        if (_currentScene == null) {
            _currentScene = newScene.Instantiate() as Control;
            AddChild(_currentScene);
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
                
            AddChild(newSceneInstance);
            if (newSceneInstance is BaseRouter router)
                router.Navigate(path);

            await ToSignal(_tween, "finished");

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