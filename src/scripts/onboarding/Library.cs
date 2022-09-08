using Godot;

namespace TinyScreen.Scripts.Onboarding {
    public class Library : Control {

        [Export] public NodePath SubTitlePath;
        private Label _subtitle;
        

        public override void _Ready() {
            base._Ready();

            // Bind UI
            _subtitle = GetNode<Label>(SubTitlePath);
          
        }
        
    }
}