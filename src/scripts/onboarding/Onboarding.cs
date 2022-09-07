using Godot;

namespace TinyScreen.scripts.onboarding {
    public class Onboarding : Node {

        public enum Stage {
            Welcome,
            Update,
            Library,
            Emulation,
            Device,
            Widgets
        }
        
        public override void _Ready() {
            GD.Print("Onboarding is started!");
        }
    }
}