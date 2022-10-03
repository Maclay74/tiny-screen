using System;
using Common.Interfaces;
using Godot;

namespace TinyScreen.Scripts.Components.Library; 

public partial class LibrarySource : Panel {
    public ILibrarySource source;

    [Export] private Label _name;
    [Export] private Label _gamesCount;
    [Export] private TextureRect _icon;
    [Export] private CheckBox _include;

    public Action<ILibrarySource, bool> Toogle;

    public override void _Ready() {
        base._Ready();
        _icon.Texture = PrepareIcon();
            
        _name.Text = source.Name();

        if (source.IsInstalled()) {
            _gamesCount.Text = "Games installed: " + source.GamesCount();
            _include.Pressed += () => Toogle.Invoke(source, _include.ButtonPressed);
        }
        else {
            _gamesCount.Text = "Not installed";
            _include.ButtonPressed = false;
            _include.Disabled = true;
        }
    }
    
    private ImageTexture PrepareIcon() {
        var bytes = source.Icon();
            
        var img = new Image();
        img.LoadPngFromBuffer(bytes);
        img.Resize(50, 50, Image.Interpolation.Cubic);
        var tex = ImageTexture.CreateFromImage(img);

        return tex;
    }
}