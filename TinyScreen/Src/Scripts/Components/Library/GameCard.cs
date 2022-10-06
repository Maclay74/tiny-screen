using Godot;
using TinyScreen.Models;

namespace TinyScreen.Scripts.Components.Library; 

public partial class GameCard : AspectRatioContainer {
    [Export] private TextureRect _cover;
        
    public Game Game;
    private ImageTexture _texture;
    private Image _image;
    private System.Timers.Timer _updateTimer;
    private const float UpdateEventDelay = 0.1f;

    public override void _Ready() {
        base._Ready();
        //_image.Load(System.IO.Path.Combine(OS.GetUserDataDir(), Game.Artwork));
        // TODO replace with relative paths
            
        if (Game != null) {
            _image = Image.LoadFromFile(Game.Artwork);
            _texture = ImageTexture.CreateFromImage(_image);
            _cover.Texture = _texture;
        }
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
                
            //_updateTimer = null;
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