using Godot;
using GodotOnReady.Attributes;

namespace TinyScreen.scripts.modals {

    public struct ConfirmModalProperties {
        public string Title;
        public string ConfirmButtonText;
        public string CancelButtonText;
    }
    
    public partial class ConfirmModal: BaseModal<bool> {
        
        [OnReadyGet] private Button _confirmButton;
        [OnReadyGet] private Button _cancelButton;
        [OnReadyGet] private Label _title;

        public ConfirmModalProperties Properties;
        
        [OnReady(Order = 0)]
        private void BindEvents() {
            _confirmButton.Connect("pressed", this, nameof(OnConfirm));
            _cancelButton.Connect("pressed", this, nameof(OnCancel));
            
            _title.Text = Properties.Title;
            _confirmButton.Text = Properties.ConfirmButtonText;
            _cancelButton.Text = Properties.CancelButtonText;
        }

        private void OnConfirm() {
            Close(true);
        }

        private void OnCancel() {
            Close(false);
        }
    }
}