using Godot;
using GodotOnReady.Attributes;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Onboarding {
    public partial class Welcome : BaseRouter {
        [OnReadyGet] private Button _startButton;

        [Inject] private ModalService _modalService;

        [OnReady]
        public void BindEvents() {
            _startButton.Connect("pressed", this, nameof(OnUpdatePress));
        }

        private void OnUpdatePress() {
            GetParent<BaseRouter>().Navigate("update");
        }
    }
}