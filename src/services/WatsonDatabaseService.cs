﻿using DatabaseWrapper.Core;
using Godot;
using TinyScreen.framework.interfaces;
using TinyScreen.models;
using Watson.ORM.Sqlite;

namespace TinyScreen.services {
    public class WatsonDatabaseService: IDatabaseService {

        private const string DbName = "application.db";
        private WatsonORM _watson;

        private string GetDatabasePath() {
            return  System.IO.Path.Combine(OS.GetUserDataDir(), DbName);
        }

        public bool IsDatabaseExists() {
            return System.IO.File.Exists(GetDatabasePath());
        }

        public void CreateDatabase() {
            DatabaseSettings settings = new DatabaseSettings(GetDatabasePath());
            _watson = new WatsonORM(settings);
            _watson.InitializeDatabase();
            _watson.InitializeTable(typeof(Settings));
        }
    }
}