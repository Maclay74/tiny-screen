using Godot;

namespace TinyScreen.scripts.onboarding {
    public class Onboarding : Control {

        [Export] public PackedScene WelcomeScene;
        [Export] public PackedScene UpdateScene;

        private enum Stage {
            Welcome,
            Update,
            Library,
            Emulation,
            Device,
            Widgets
        }

        private Stage _currentStage;
        private Node _currentScene;
        
        public override void _Ready() {
            GD.Print("Onboarding is started!");
            
            SetStage(_currentStage);
        }
        
        private void SetStage(Stage newStage) {
            
            switch (newStage) {
                case Stage.Welcome:
                    TransitionToScene(WelcomeScene);
                    break;
                
                case Stage.Update:
                    GD.Print("Update!!!");
                    break;
            }
        }

        public void NextStage() {
            if (_currentStage != Stage.Widgets) {
                SetStage(_currentStage + 1);
                return;
            }
            
            // At this point onboarding is finished
            // Persist settings and games
        }

        private async void TransitionToScene(PackedScene newScene) {
            
            // If there is no scene, just show it (for now)
            if (_currentScene == null) {
                _currentScene = newScene.Instance();
                AddChild(_currentScene);
            }
            
            // Otherwise replace it with animation
        }
        
    }
}