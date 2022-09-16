using System.Linq;
using FlexLayoutSharp;
using Godot;
using Godot.Collections;

namespace TinyScreen.addons.StagedProgressBar {
    [Tool]
    public class StagedProgressBar : Control {
        private System.Collections.Generic.Dictionary<string, int> _stages;

        [Export]
        public System.Collections.Generic.Dictionary<string, int> Stages {
            get => _stages;
            set {
                _stages = value;
                UpdateLayout();
                Update();
            }
        }
        
        // Distance between stages
        private float _gap;

        [Export]
        public float Gap {
            get => _gap;
            set {
                _gap = value;
                UpdateLayout();
                Update();
            }
        }

        [Export] private StyleBox _backgroundStyle;
        
        [Export] private StyleBox _progressStyle;

        [Export] private StyleBox _stageStyle;

        [Export] private Font _textFont;

        private float _progress;

        [Export]
        public float Progress {
            get => _progress;
            set {
                _progress = value;
                Update();
            }
        }

        // Set progress with animation
        public void SetProgress(float progress) {
            _tween.StopAll();
            _tween.InterpolateProperty(this, "Progress", _progress, progress, 0.2f, Tween.TransitionType.Cubic);
            _tween.Start();
        }
        
        // Layout for calculating sizes
        private FlexLayoutSharp.Node _layoutRoot;

        // To animate progress
        private Tween _tween;
        
        // Masking for stages
        private ImageTexture _maskTexture;
        private Image _maskImage;
        
        public override Vector2 _GetMinimumSize() {
            return new Vector2(20, 30);
        }

        // Draw rect for background
        private void DrawStages() {
            if (_stageStyle == null) 
                return;
            DrawStyleBox(_stageStyle, new Rect2(new Vector2(0, 0), RectSize));
        }
        
        private void DrawProgress() {
            if (_progressStyle == null) 
                return;
            
            var size = RectSize;
            size.x *= Progress;
            DrawStyleBox(_progressStyle, new Rect2(new Vector2(0, 0), size));
        }
        
        private void DrawLabels() {
            var currentWidthPosition = 0f;

            for (int i = 0; i < Stages.Count; i++) {
                var size = new Vector2(_layoutRoot.GetChild(i).LayoutGetWidth(), RectSize.y);
                DrawString(_textFont, new Vector2(currentWidthPosition + 5, size.y / 2 + 5), Stages.ElementAt(i).Key);
                currentWidthPosition += size.x + _gap;
            }
        }

        private void UpdateLayout() {
            if (Stages == null)
                return;
            
            // Size minus gaps
            var workingWidth = RectSize.x - _gap * (Stages.Count - 1);
            
            // How many parts in progress
            var overall = Stages.Aggregate(0, (acc, pair) => acc + pair.Value);

            // Create layout
            _layoutRoot = Flex.CreateDefaultNode();
            _layoutRoot.StyleSetFlexDirection(FlexDirection.Row);
            _layoutRoot.StyleSetWidth(workingWidth);
            _layoutRoot.StyleSetMaxWidth(workingWidth);
            _layoutRoot.StyleSetMaxHeight(RectSize.y);

            // Calculate min width based on text width
            // Create child for layout
            foreach (var stage in Stages.ToList()) {
                var width = Mathf.Max(
                    _textFont.GetStringSize(stage.Key).x + 10,
                    workingWidth * ((float) stage.Value / overall)
                );
                var stageNode = Flex.CreateDefaultNode();
                stageNode.StyleSetMinWidth(_textFont.GetStringSize(stage.Key).x + 10);
                stageNode.StyleSetWidth(width);
                stageNode.StyleSetFlexGrow(stage.Value);
                stageNode.StyleSetFlexShrink(1f);
                _layoutRoot.AddChild(stageNode);
            }
            
            Flex.CalculateLayout(_layoutRoot, float.NaN, float.NaN, Direction.LTR);
            
            // Create masking image
            if (_maskImage == null) 
                _maskImage = new Image();
            
            _maskImage.Create((int)RectSize.x, (int)RectSize.y, false, Image.Format.Rgba8);
            
            // Get sizes of stages and place gap on image between them
            var currentWidthPosition = 0f;
            for (int i = 0; i < Stages.Count - 1; i++) {
                var size = new Vector2(_layoutRoot.GetChild(i).LayoutGetWidth(), RectSize.y);
                var position = new Vector2(currentWidthPosition + size.x, 0);
                _maskImage.FillRect(new Rect2(position, new Vector2(_gap, RectSize.y)), new Color(1,1,1,1));
                currentWidthPosition += size.x + _gap;
            }
            
            // Create texture
            if (_maskTexture == null)   
                _maskTexture = new ImageTexture();
            
            // Update shared
            _maskTexture.CreateFromImage(_maskImage);
            ((ShaderMaterial)Material).SetShaderParam("mask", _maskTexture);
            ((ShaderMaterial)Material).SetShaderParam("size", RectSize);
        }

        public override void _Draw() {
            base._Draw();

            if (Stages == null)
                return;
            
            if (_layoutRoot == null) UpdateLayout();
            DrawStages();
            DrawProgress();
            DrawLabels();
        }

        public override void _EnterTree() {
            base._EnterTree();
            Material = new ShaderMaterial();
            var shader = ResourceLoader.Load<Shader>("res://addons/StagedProgressBar/mask.gdshader");
            ((ShaderMaterial) Material).Shader = shader;
        }

        public override void _Ready() {
            base._Ready();
            SetNotifyTransform(true);
            SetNotifyLocalTransform(true);
            
            _tween = new Tween();
            AddChild(_tween);
        }

        public override void _Notification(int what) {
            base._Notification(what);

            // We need these notifications, because if element is hidden or resized
            // We need to recalculate the layout
            switch (what) {
                case NotificationVisibilityChanged:
                case NotificationTransformChanged:
                case NotificationLocalTransformChanged:
                    UpdateLayout();
                    Update();
                    break;
            }
        }
    }
}