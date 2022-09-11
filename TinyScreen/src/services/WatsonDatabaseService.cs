using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DatabaseWrapper.Core;
using ExpressionTree;
using Godot;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Models;
using Watson.ORM.Sqlite;

namespace TinyScreen.Services {
    public class WatsonDatabaseService: IDatabaseService {

        private const string DbName = "application.db";
        private WatsonORM _watson;

        private string GetDatabasePath() {
            return  System.IO.Path.Combine(OS.GetUserDataDir(), DbName);
        }

        public bool IsDatabaseExists() {
            return System.IO.File.Exists(GetDatabasePath());
        }

        public void InitDatabase() {
            DatabaseSettings settings = new DatabaseSettings(GetDatabasePath());
            _watson = new WatsonORM(settings);
            _watson.InitializeDatabase();
            
            // Find all models and initialize tables for them
            var models = from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsClass && t.Namespace == Assembly.GetExecutingAssembly().GetName().Name + ".Models"
                select t;
            
            _watson.InitializeTables(models.ToList());
        }

        public void Insert<T>(T record) where T : class, new() {
            _watson.Insert(record);
        }

        public T Select<T>(Expr expr, ResultOrder[] ro = null) where T : class, new() {
            return _watson.SelectFirst<T>(expr, ro);
        }
    }
}