using Godot;
namespace TinyScreen.scripts.modals {

    public struct AlertModalProperties {
        public string Title;
        public string OkText;
    }
    
    public partial class AlertModal: BaseModal {
        
        [Export] private Button _okButton;
        [Export] private Label _title;

        public AlertModalProperties Properties;

        public override void _Ready() {
            base._Ready();
            
            _okButton.Pressed += async () => {
                await FadeOut();
                EmitSignal("Decision");
            };
            
            _title.Text = Properties.Title;
            _okButton.Text = Properties.OkText;
        }
    }
}