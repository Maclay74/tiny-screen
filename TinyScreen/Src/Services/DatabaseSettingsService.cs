using TinyScreen.Framework.Interfaces;

namespace TinyScreen.Services; 

public class ApplicationService {
    private IDatabaseService _databaseService;
    
    public const string SettingsAutoUpdateApp = "autoUpdateApp";
    public const string SettingsAutoUpdateLibrary = "autoUpdateLibrary";
    public const string SettingsStartWithWindows = "startWithWindows";

    public ApplicationService(IDatabaseService databaseService) {
        _databaseService = databaseService;
    }

    public bool IsInstalled() {
        return _databaseService.Exists();
    }

    public void Install() {
        _databaseService.InitDatabase();
        _databaseService.SetSettings(SettingsAutoUpdateApp, true.ToString());
        _databaseService.SetSettings(SettingsAutoUpdateLibrary, true.ToString());
        _databaseService.SetSettings(SettingsStartWithWindows, true.ToString());
    }
}