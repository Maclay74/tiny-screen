using Godot;
using GodotOnReady.Attributes;

namespace TinyScreen.scripts.modals {

    public struct AlertModalProperties {
        public string Title;
        public string OkText;
    }
    
    public partial class AlertModal: BaseModal<object> {
        
        [OnReadyGet] private Button _okButton;
        [OnReadyGet] private Label _title;

        public AlertModalProperties Properties;
        
        [OnReady]
        private void BindEvents() {
            _okButton.Connect("pressed", this, nameof(OnOk));
            _title.Text = Properties.Title;
            _okButton.Text = Properties.OkText;
        }

        private void OnOk() {
            Close(null);
        }
    }
}