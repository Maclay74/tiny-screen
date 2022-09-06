using TinyScreen.framework.interfaces;

namespace TinyScreen.services {
    public class SteamService: ILauncherService {
        
        private IDatabaseService _databaseService;
        
        public SteamService(IDatabaseService databaseService) {
            _databaseService = databaseService;
            ((DatabaseService) _databaseService).Test();
            //GD.Print();
        }
        
    }
}