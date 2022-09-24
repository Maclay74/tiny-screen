using System;
using Godot;
using GodotOnReady.Attributes;
using TinyScreen.Models;

namespace TinyScreen.Scripts.Components.Library {
    public partial class FolderCard : AspectRatioContainer {
        
        [OnReadyGet] private Label _label;
        [OnReadyGet] private Button _button;
        
        private System.Timers.Timer _updateTimer;
        private const float UpdateEventDelay = 0.1f;

        public string FolderName;
        public Action<string> OnPress;

        [OnReady]
        private void Setup() {
            _label.Text = FolderName;
            _button.Connect("pressed", this, nameof(OnButtonPress));
        }

        private void OnButtonPress() {
            OnPress.Invoke(FolderName);
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
                var newSize = new Vector2(RectSize.x, RectSize.x / Ratio);

                if (RectSize != newSize) {
                    RectSize = new Vector2(RectSize.x, RectSize.x / Ratio);
                    RectMinSize = new Vector2(0, RectSize.x / Ratio);
                    Update();
                }
                
            };

            _updateTimer?.Start();
        }

        public override void _Notification(int what) {
            base._Notification(what);

            switch (what) {
                case NotificationResized:
                    UpdateLayout();
                    break;
            }
        }
    }
}