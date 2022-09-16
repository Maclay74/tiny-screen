using Godot;
using Godot.Collections;
using GodotOnReady.Attributes;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Onboarding {
    public partial class Finish : Control {

        [OnReadyGet] private Button _finishButton;
        [OnReadyGet] private Button _tourButton;
        
        [OnReadyGet] private CheckBox _updateCheckbox;
        [OnReadyGet] private CheckBox _libraryCheckbox;
        [OnReadyGet] private CheckBox _autorunCheckbox;
        
        [Inject] public ISettingsService _settingsService;

        [OnReady] public void BindEvents() {
            _finishButton.Connect("pressed", GetParent(), nameof(Onboarding.Finish), new Array{false});
            _tourButton.Connect("pressed", GetParent(), nameof(Onboarding.Finish), new Array{true});
            
            //TODO bind checkbox to toggle settings via _settingsService
            // _settingsService.Set(Setting.Author, "Mike!");
        }
        
    }
}