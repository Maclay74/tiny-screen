using Godot;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;

namespace TinyScreen.scripts {
    public class RootNode: Control {

        [Inject] public ISettingsInterface _settingsService;
        [Inject] public IDatabaseService _databaseService;

        [Export] private PackedScene Onboarding;
        [Export] private PackedScene Application;

        public override void _Ready() {
            base._Ready();

            var scriptRes = ResourceLoader.Load<Script>("res://src/framework/ContainerNode.cs");
            var baseType = scriptRes.GetInstanceBaseType();
            GD.Print(ClassDB.IsParentClass(baseType, "Node"));
            
            // Application is not installed, we need onboarding!
            if (!_settingsService.IsAppInstalled()) {
                
                AddChild(Onboarding.Instance());
            
                // Install application
                _settingsService.InstallApp();
                
                // Add some settings during the onboarding, for example
                //_settingsService.Set(Setting.Author, "Mike!");
            }
            
            // Application is installed, show it
            else {
                
                // Set up database
                _databaseService.InitDatabase();
                
                // When app is running normally, it's possible to retrieve settings
                var author = _settingsService.Get(Setting.Author);
                GD.Print(author);
            }
        }
    }
}