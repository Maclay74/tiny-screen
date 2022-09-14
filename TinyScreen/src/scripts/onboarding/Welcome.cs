using Godot;
using GodotOnReady.Attributes;
using TinyScreen.Framework.Attributes;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Onboarding {
    public partial class Welcome : Control {

        [OnReadyGet] private Button _startButton;

        [Inject] private ModalService _modalService;
        
        [OnReady] public void BindEvents() {
            _startButton.Connect("pressed", GetParent(), nameof(Onboarding.NextStage));
            //_startButton.Connect("pressed", this, nameof(Test));
        }

        private async void Test() {
            if (await _modalService.Confirm("Oh dude, are you sure about that?")) {
                GD.Print("Confirmed!");
            }
            else {
                GD.Print("Declined!");
            }
        }
    }
}