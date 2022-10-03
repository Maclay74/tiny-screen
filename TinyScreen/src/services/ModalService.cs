using System;
using System.Threading.Tasks;
using Godot;

using TinyScreen.scripts.modals;

namespace TinyScreen.Services {
    public partial class ModalService: Node {

        [Export] private PackedScene _confirmScene;
        [Export] private PackedScene _alertScene;
        
        public async Task<bool> Confirm(string title, string confirmText = "Confirm", string cancelText = "Cancel") {
            
            if (_confirmScene.Instantiate() is not ConfirmModal modal) return false;
            
            modal.Properties = new ConfirmModalProperties {
                Title = title,
                CancelButtonText = cancelText,
                ConfirmButtonText = confirmText,
            };
            AddChild(modal);

            var response = await ToSignal(modal, nameof(modal.Decision));
            RemoveChild(modal);
            return (bool)response[0];
        }

        public async Task Alert(string title, string okText = "OK") {
            
            if (_alertScene.Instantiate() is not AlertModal modal) return;

            modal.Properties = new AlertModalProperties {
                Title = title,
                OkText = okText
            };
            
            AddChild(modal);
            await ToSignal(modal, nameof(modal.Decision));
            RemoveChild(modal);
        }
    }
}