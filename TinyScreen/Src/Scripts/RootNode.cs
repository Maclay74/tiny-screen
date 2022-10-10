using Godot;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Services;

namespace TinyScreen.scripts;

public partial class RootNode : BaseRouter {
    [Inject] private ApplicationService _application;

    [Inject] private IDatabaseService _databaseService;

    [Export] private PackedScene Onboarding;
    [Export] private PackedScene Application;

    private BaseRouter? _currentScene;

    public override partial void _Ready();

    [Ready]
    public void Start() {
        _databaseService.InitDatabase();

        if (!_application.IsInstalled()) {
            Navigate("/onboarding");
        }
        else {
            Navigate("/application", false);
        }
    }

    private void CheckScene() {
        if (_currentScene == null) {
            _currentScene = Onboarding.Instantiate() as BaseRouter;
            AddChild(_currentScene);
        }
        else if (_currentScene.SceneFilePath != Onboarding.ResourcePath) {
            RemoveChild(_currentScene);
            _currentScene = Onboarding.Instantiate() as BaseRouter;
            AddChild(_currentScene);
        }
    }

    [Route("onboarding")]
    private async void OnboardingRoute(string path) {
        CheckScene();
        await ToSignal(GetTree(), "process_frame");
        _currentScene?.Navigate(path);
    }

    [Route("application")]
    private async void ApplicationRoute(string path, bool showTutorial = false) {
        CheckScene();
        _databaseService.InitDatabase();

        _currentScene = Application.Instantiate() as BaseRouter;
        AddChild(_currentScene);
        await ToSignal(GetTree(), "process_frame");
        _currentScene?.Navigate(path);
    }
}