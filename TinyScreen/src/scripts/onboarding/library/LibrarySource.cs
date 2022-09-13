using System;
using Godot;
using GodotOnReady.Attributes;
using TinyScreen.Framework.Interfaces;

namespace TinyScreen.Scripts.Onboarding {
    public partial class LibrarySource : Panel {

        public ILibrarySource source;
        
        [OnReadyGet] private Label _name;
        [OnReadyGet] private Label _gamesCount;
        [OnReadyGet] private TextureRect _icon;
        [OnReadyGet] private CheckBox _include;

        public Action<ILibrarySource, bool> toogle;

        [OnReady] public void Setup() {
            
            _icon.Texture = PrepareIcon();
            _name.Text = source.Name();

            if (source.IsInstalled()) {
                _gamesCount.Text = "Games installed: " + source.GamesCount();
                _include.Connect("pressed", this, nameof(Toggle));
            }
            else {
                _gamesCount.Text = "Not installed";
                _include.Pressed = false;
                _include.Disabled = true;
            }
        }

        private ImageTexture PrepareIcon() {
            var bytes = source.Icon();

            var tex = new ImageTexture();
            var img = new Image();
            img.LoadPngFromBuffer(bytes);
            img.Resize(50, 50, Image.Interpolation.Cubic);
            tex.CreateFromImage(img);

            return tex;
        }

        private void Toggle() {
            toogle.Invoke(source, _include.Pressed);
        }
    }
}