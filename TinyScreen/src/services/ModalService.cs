using System.Threading.Tasks;
using Godot;

using TinyScreen.scripts.modals;

namespace TinyScreen.Services {
    public class ModalService: Node {

        [Export] private PackedScene _confirmScene;
        
        public async Task<bool> Confirm(string title, string confirmText = "Confirm", string cancelText = "Cancel") {
            
            var modal = _confirmScene.Instance() as ConfirmModal;
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
    }
}