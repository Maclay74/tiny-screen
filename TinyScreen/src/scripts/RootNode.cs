using Godot;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Scripts.Onboarding;
using static SQLitePCL.Batteries_V2;

namespace TinyScreen.scripts {
	public class RootNode: Control {

		[Inject] public ISettingsService _settingsService;
		[Inject] public IDatabaseService _databaseService;

		[Export] private PackedScene Onboarding;
		[Export] private PackedScene Application;


      public override async void _Ready() {
            base._Ready();
            
            if (!_settingsService.IsAppInstalled()) {

                var onboarding = Onboarding.Instance() as Onboarding;
                AddChild(onboarding);

                var onboardingResult = await ToSignal(onboarding, "Finished");
                bool showTutorial = (bool)onboardingResult[0];
                
                RemoveChild(onboarding);
            }
            
            _databaseService.InitDatabase();

        }
    }
}

