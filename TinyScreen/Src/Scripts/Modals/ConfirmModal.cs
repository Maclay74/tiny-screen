using Godot;

namespace TinyScreen.Scripts.Modals; 

public struct ConfirmModalProperties {
    public string Title;
    public string ConfirmButtonText;
    public string CancelButtonText;
}

public partial class ConfirmModal : BaseModal {
    [Export] private Button _confirmButton;
    [Export] private Button _cancelButton;
    [Export] private Label _title;

    public ConfirmModalProperties Properties;

    public override void _Ready() {
        base._Ready();
       

        _confirmButton.Pressed += async () => {
            await FadeOut();
            EmitSignal("Decision", true);
        };
        _cancelButton.Pressed += async () => {
            await FadeOut();
            EmitSignal("Decision", false);
        };

        _title.Text = Properties.Title;
        _confirmButton.Text = Properties.ConfirmButtonText;
        _cancelButton.Text = Properties.CancelButtonText;
        
        FadeIn();
    }
}