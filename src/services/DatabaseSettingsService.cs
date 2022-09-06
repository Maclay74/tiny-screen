using System;
using TinyScreen.framework.interfaces;

namespace TinyScreen.services {
    public class DatabaseSettingsService: ISettingsInterface {
        
        private IDatabaseService _databaseService;
        
        public DatabaseSettingsService(IDatabaseService databaseService) {
            _databaseService = databaseService;
        }

        public bool IsAppInstalled() {
            // Since we store settings in database, we need it.
            return _databaseService.IsDatabaseExists();
        }

        public void InstallApp() {
            _databaseService.CreateDatabase();
        }

        public void Set(Setting setting) {
            
        }

        public string Get(Setting setting) {
            return "Temporary value";
        }
    }
}