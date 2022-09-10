using System;
using Godot;
using TinyScreen.Framework.Interfaces;

namespace TinyScreen.Scripts.Onboarding {
    public class LibrarySource : Panel {

        public ILibrarySource source;
        
        [Export] public NodePath NamePath;
        [Export] public NodePath GamesCountPath;
        [Export] public NodePath IconPath;
        [Export] public NodePath IncludeCheckbox;

        private Label _name;
        private Label _gamesCount;
        private TextureRect _icon;
        private CheckBox _include;

        public Action<ILibrarySource, bool> toogle;

        public override void _Ready() {

            _name = GetNode<Label>(NamePath);
            _gamesCount = GetNode<Label>(GamesCountPath);
            _icon = GetNode<TextureRect>(IconPath);
            _include = GetNode<CheckBox>(IncludeCheckbox);
            
            _icon.Texture = PrepareIcon();
            _name.Text = source.Name();

            if (source.IsInstalled()) {
                _gamesCount.Text = "Games: " + source.GamesCount();
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