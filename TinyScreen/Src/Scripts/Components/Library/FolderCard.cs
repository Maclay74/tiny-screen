using System;
using Godot;

namespace TinyScreen.Scripts.Components.Library; 

public partial class FolderCard : AspectRatioContainer {
        
    [Export] private Label _label;
    [Export] private Button _button;
        
    private System.Timers.Timer _updateTimer;
    private const float UpdateEventDelay = 0.1f;

    public string FolderName;
    public Action<string> OnPress;

    public override void _Ready() {
        base._Ready();
        _label.Text = FolderName;

        _button.Pressed += () => OnPress(FolderName);

    }
        
    private void UpdateLayout() {

        if (_updateTimer != null) {
            _updateTimer.Stop();
            _updateTimer = null;
        }
            
        _updateTimer = new System.Timers.Timer {
            AutoReset = false,
            Interval = UpdateEventDelay,
        };

        _updateTimer.Elapsed += (sender, args) => {
            var newSize = new Vector2(Size.x, Size.x / Ratio);

            if (Size != newSize) {
                Size = new Vector2(Size.x, Size.x / Ratio);
                CustomMinimumSize = new Vector2i(0, (int)(Size.x / Ratio));
                QueueRedraw();
            }
                
        };

        _updateTimer?.Start();
    }

    public override void _Notification(long what) {
        base._Notification(what);

        switch (what) {
            case NotificationResized:
                UpdateLayout();
                break;
        }
    }
}