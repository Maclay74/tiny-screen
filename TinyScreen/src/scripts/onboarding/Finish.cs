using Godot;
using GodotOnReady.Attributes;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;

namespace TinyScreen.Scripts.Onboarding {
    public partial class Finish : BaseRouter {
        [OnReadyGet] private Button _finishButton;
        [OnReadyGet] private Button _tourButton;

        [OnReadyGet] private CheckBox _updateCheckbox;
        [OnReadyGet] private CheckBox _libraryCheckbox;
        [OnReadyGet] private CheckBox _autorunCheckbox;

        [Inject] private ISettingsService _settingsService;

        [OnReady]
        private void BindEvents() {
            _finishButton.Connect("pressed", this, nameof(OnFinishPress));
            _tourButton.Connect("pressed", this, nameof(OnTourPress));
            _updateCheckbox.Connect("pressed", this, nameof(OnUpdatePress));
            _libraryCheckbox.Connect("pressed", this, nameof(OnUpdateLibraryPress));
            _autorunCheckbox.Connect("pressed", this, nameof(OnStartupPress));
        }

        private void OnFinishPress() => Navigate("/application", false);

        private void OnTourPress() => Navigate("/application", true);

        private void OnUpdatePress() {
            _settingsService.Set(Setting.AutoUpdateApplication, _updateCheckbox.Pressed.ToString());
        }

        private void OnUpdateLibraryPress() {
            _settingsService.Set(Setting.AutoUpdateLibrary, _libraryCheckbox.Pressed.ToString());
        }

        private void OnStartupPress() {
            _settingsService.Set(Setting.StartWithWindows, _autorunCheckbox.Pressed.ToString());
        }
    }
}