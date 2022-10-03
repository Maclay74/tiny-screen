namespace RetroArchLibrarySource {
    internal interface IRetroArchInstallation {
        bool Installed();
        string InstallPath();
    }
}