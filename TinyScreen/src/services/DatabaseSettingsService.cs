using System;
using ExpressionTree;
using Godot;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Models;

namespace TinyScreen.Services {
    public class DatabaseSettingsService: ISettingsService {
        
        private IDatabaseService _databaseService;
        
        public DatabaseSettingsService(IDatabaseService databaseService) {
            _databaseService = databaseService;
        }

        public bool IsAppInstalled() {
            // Since we store settings in database, we need it.
            return false;
            return _databaseService.IsDatabaseExists();
        }

        public void InstallApp() {
            _databaseService.InitDatabase();
        }

        public void Set(Setting setting, string value) {
            _databaseService.Insert(new Settings {
                Name = setting.ToString(),
                Value = value
            });
        }

        public string Get(Setting setting) {
            var record = _databaseService.Select<Settings>(new Expr("name", OperatorEnum.Equals,Setting.Author));
            if (record != null) return record.Value;
            throw new Exception($"Setting {setting.ToString()} was not found");
        }
    }
}