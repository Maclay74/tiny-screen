using System.Collections.Generic;
using DatabaseWrapper.Core;
using ExpressionTree;

namespace TinyScreen.Framework.Interfaces {
    public interface IDatabaseService {
        bool IsDatabaseExists();
        void InitDatabase();

        void Insert<T>(T record) where T : class, new();

        T Update<T>(T obj) where T : class, new();

        T Select<T>(Expr expr, ResultOrder[] ro = null) where T : class, new();

        List<T> SelectAll<T>(Expr expr, ResultOrder[] ro = null) where T : class, new();

        List<T> SelectAll<T>(int? offset, int? count, Expr expr, ResultOrder[] ro = null) where T : class, new();

        void Delete<T>(T obj) where T : class, new();

        void DeleteAll<T>(Expr expr) where T : class, new();
    }
}