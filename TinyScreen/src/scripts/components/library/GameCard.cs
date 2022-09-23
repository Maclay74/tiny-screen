using Godot;
using GodotOnReady.Attributes;
using TinyScreen.Models;

namespace TinyScreen.Scripts.Components.Library {
    public partial class GameCard : AspectRatioContainer {
        [OnReadyGet] private TextureRect _cover;
        
        public Games Game;
        private ImageTexture _texture;
        private Image _image;
        private System.Timers.Timer _updateTimer;
        private const float UpdateEventDelay = 0.1f;

        [OnReady]
        private void Setup() {
            _texture = new ImageTexture();
            _image = new Image();
            //_image.Load(System.IO.Path.Combine(OS.GetUserDataDir(), Game.Artwork));
            // TODO replace with relative paths

            _image.Load(Game.Artwork);
            _texture.CreateFromImage(_image);
            _cover.Texture = _texture;
           
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
                
                //_updateTimer = null;
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