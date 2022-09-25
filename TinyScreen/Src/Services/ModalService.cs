using System.Threading.Tasks;
using Godot;

using TinyScreen.scripts.modals;

namespace TinyScreen.Services {
    public class ModalService: Node {

        [Export] private PackedScene _confirmScene;
        [Export] private PackedScene _alertScene;
        
        public async Task<bool> Confirm(string title, string confirmText = "Confirm", string cancelText = "Cancel") {
            
            if (!(_confirmScene.Instance() is ConfirmModal modal)) return false;
            
            modal.Properties = new ConfirmModalProperties {
                Title = title,
                CancelButtonText = cancelText,
                ConfirmButtonText = confirmText
            };
            AddChild(modal);
            
            var response = await ToSignal(modal, "Decision");
            RemoveChild(modal);
            
            return ((ConfirmModal.Response)response[0]).Result;
        }

        public async Task Alert(string title, string okText = "OK") {
            
            if (!(_alertScene.Instance() is AlertModal modal)) return;

            modal.Properties = new AlertModalProperties {
                Title = title,
                OkText = okText
            };
            
            AddChild(modal);
            var response = await ToSignal(modal, "Decision");
            RemoveChild(modal);
        }
    }
}