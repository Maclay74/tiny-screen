using Godot;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Onboarding; 

public partial class Finish : BaseRouter {
    [Export] private Button _finishButton;
    [Export] private Button _tourButton;

    [Export] private CheckBox _updateCheckbox;
    [Export] private CheckBox _libraryCheckbox;
    [Export] private CheckBox _autorunCheckbox;

    [Inject] private IDatabaseService _databaseService;
        
    public override partial void _Ready();

    [Ready]
    private void BindEvents() {
        _finishButton.Pressed += () => Navigate("/application", false);
        _tourButton.Pressed += () => Navigate("/application", true);
        _updateCheckbox.Pressed += () => {
            _databaseService.SetSettings(ApplicationService.SettingsAutoUpdateApp, _updateCheckbox.ButtonPressed.ToString());
        };
        _libraryCheckbox.Pressed += () => {
            _databaseService.SetSettings(ApplicationService.SettingsAutoUpdateLibrary, _libraryCheckbox.ButtonPressed.ToString());
        };
        _autorunCheckbox.Pressed += () => {
            _databaseService.SetSettings(ApplicationService.SettingsStartWithWindows, _autorunCheckbox.ButtonPressed.ToString());
        };
    }
}