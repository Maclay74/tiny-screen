using Godot;
using TinyScreen.Framework.Attributes;
using TinyScreen.framework.interfaces;

namespace TinyScreen.scripts {
    public class RootNode: Control {

        [Inject] public ILauncherService _Launcher;
        
        public override void _Ready() {
            base._Ready();
            GD.Print(_Launcher);
        }
    }
}