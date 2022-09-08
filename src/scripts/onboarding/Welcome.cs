using Godot;

namespace TinyScreen.scripts.onboarding {
    public class Welcome : Node {

        [Export] public NodePath StartButtonPath;
        
        private Button _startButton;
        
        public override void _Ready() {
            base._Ready();
            _startButton = GetNode<Button>(StartButtonPath);
            _startButton.Connect("pressed", GetParent(), nameof(Onboarding.NextStage));
        }
    }
}