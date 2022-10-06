using System.Threading.Tasks;
using Godot;

namespace TinyScreen.Scripts.Modals; 

public abstract partial class BaseModal : Node {
    
    [Export] private PanelContainer _content;
    [Export] private Panel _background;

    private Tween _tween;

    [Signal]
    public delegate void DecisionEventHandler();
    
    [Signal]
    public delegate void FadeEventHandler();

    protected async Task FadeIn() {
        _content.Modulate = new Color(1, 1, 1, 0);
            
        // Hack so godot could resize content window
        await ToSignal(GetTree(), "process_frame");
        _content.QueueRedraw();
        await ToSignal(GetTree(), "process_frame");
            
        _content.PivotOffset = _content.Size / 2;

        _tween = CreateTween()
            .SetEase(Tween.EaseType.InOut);

        _tween
            .Parallel()
            .TweenProperty(_content, "modulate", new Color(1, 1, 1, 1), .2f)
            .FromCurrent()
            .SetTrans(Tween.TransitionType.Cubic);

        _tween
            .Parallel()
            .TweenProperty(_content, "scale", new Vector2(1, 1), .2f)
            .From(new Vector2(0.5f, 0.5f))
            .SetTrans(Tween.TransitionType.Back);

        _tween
            .Parallel()
            .TweenProperty(_background.Material, "shader_param/lod", 3, .15f)
            .FromCurrent()
            .SetTrans(Tween.TransitionType.Cubic);
        
        await ToSignal(_tween, "finished");
    }

    protected async Task FadeOut() {
        if (_tween != null && _tween.IsRunning()) {
            _tween.Stop();
            _tween = null;
        }
        
        _tween = CreateTween()
            .SetEase(Tween.EaseType.InOut);
        
        _tween.Parallel()
            .TweenProperty(_content, "modulate", new Color(1, 1, 1, 0), .15f)
            .FromCurrent()
            .SetTrans(Tween.TransitionType.Cubic);

        _tween.Parallel()
            .TweenProperty(_content, "scale", new Vector2(1, 1), .15f)
            .FromCurrent()
            .SetTrans(Tween.TransitionType.Back);
        
        _tween.Parallel()
            .TweenProperty(_background.Material, "shader_param/lod", 0, .2f)
            .FromCurrent()
            .SetTrans(Tween.TransitionType.Cubic);

        await ToSignal(_tween, "finished");
    }
}