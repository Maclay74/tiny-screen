using System;
using ExpressionTree;
using Godot;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Models;

namespace TinyScreen.Services {
    public class DatabaseSettingsService : ISettingsService {
        private IDatabaseService _databaseService;

        public DatabaseSettingsService(IDatabaseService databaseService) {
            _databaseService = databaseService;
        }

        public bool IsAppInstalled() {
            return false;
            return _databaseService.IsDatabaseExists();
        }

        public void InstallApp() {
            _databaseService.InitDatabase();
            Set(Setting.AutoUpdateApplication, true.ToString());
            Set(Setting.AutoUpdateLibrary, true.ToString());
            Set(Setting.StartWithWindows, true.ToString());
        }

        public void Set(Setting setting, string value) {
            var record = _databaseService.Select<Settings>(new Expr("name", OperatorEnum.Equals, setting));

            if (record == null) {
                _databaseService.Insert(new Settings {
                    Name = setting.ToString(),
                    Value = value
                });
                return;
            }

            record.Value = value;
            _databaseService.Update(record);
        }

        public string Get(Setting setting) {
            var record = _databaseService.Select<Settings>(new Expr("name", OperatorEnum.Equals, setting));
            if (record != null) return record.Value;
            throw new Exception($"Setting {setting.ToString()} was not found");
        }
    }
}