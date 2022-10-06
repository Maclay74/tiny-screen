using Godot;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Onboarding; 

public partial class Welcome : BaseRouter {
    [Export] private Button _startButton;

    [Inject] private ModalService _modalService;
        
    public override partial void _Ready();

    [Ready]
    private void BindEvents() {
        _startButton.Pressed += () => Navigate("/onboarding/update");
    }
}