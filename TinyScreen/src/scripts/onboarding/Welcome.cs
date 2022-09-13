using Godot;
using GodotOnReady.Attributes;

namespace TinyScreen.Scripts.Onboarding {
    public partial class Welcome : Control {

        [OnReadyGet] private Button _startButton;
        
        [OnReady] public void BindEvents() {
            _startButton.Connect("pressed", GetParent(), nameof(Onboarding.NextStage));
        }
    }
}