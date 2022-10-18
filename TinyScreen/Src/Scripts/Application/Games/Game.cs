using Godot;
using TinyScreen.Framework;

namespace TinyScreen.Scripts.Application.Games; 

public partial class Game: BaseRouter {
    [Export] private Button _button;


    public Models.Game GameItem;

    public override void _Ready() {
        base._Ready();
       
        _button.Pressed += Back;

        _button.Text = GameItem.Name + " close";
    }
    
}