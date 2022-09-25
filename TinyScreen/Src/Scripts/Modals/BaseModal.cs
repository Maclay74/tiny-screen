using Godot;
using GodotOnReady.Attributes;

namespace TinyScreen.scripts.modals {
    
    public abstract partial class BaseModal<T>: Node {

        public class Response: Object {
            public T Result;
        }
        
        [OnReadyGet] private Tween _tween;
        [OnReadyGet] private PanelContainer _content;
        [OnReadyGet] private Panel _background;
        
        [Signal]
        protected delegate void Decision();
        
        [OnReady(Order = 1)]
        private async void FadeIn() {
            _content.Modulate = new Color(1, 1, 1, 0);
            
            // Hack so godot could resize content window
            await ToSignal(GetTree(), "idle_frame");
            _content.Update();
            await ToSignal(GetTree(), "idle_frame");
            
            _content.RectPivotOffset = _content.RectSize / 2;
            _tween.InterpolateProperty(_content, "modulate", new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), .2f, Tween.TransitionType.Cubic);
            _tween.InterpolateProperty(_content, "rect_scale", new Vector2(0.5f, 0.5f), new Vector2(1, 1), .2f, Tween.TransitionType.Back);
            _tween.InterpolateProperty(_background.Material, "shader_param/lod", 0, 3, .15f, Tween.TransitionType.Cubic);
            _tween.Start();
        }

        private void FadeOut() {
            _tween.StopAll();
            _tween.InterpolateProperty(_content, "modulate", new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 0.15f, Tween.TransitionType.Cubic);
            _tween.InterpolateProperty(_content, "rect_scale", new Vector2(1, 1), new Vector2(0.5f, 0.5f), 0.15f, Tween.TransitionType.Cubic);
            _tween.InterpolateProperty(_background.Material, "shader_param/lod", 3, 0, 0.2f, Tween.TransitionType.Cubic);
            _tween.Start();
        }

        protected async void Close(T result) {
            FadeOut();
            await ToSignal(_tween, "tween_all_completed");
            EmitSignal("Decision", new Response { Result = result });
        }
    }
}