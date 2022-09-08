using DatabaseWrapper.Core;
using ExpressionTree;

namespace TinyScreen.Framework.Interfaces {
    public interface IDatabaseService {
        bool IsDatabaseExists();
        void InitDatabase();

        void Insert<T>(T record) where T : class, new();

        T Select<T>(Expr expr, ResultOrder[] ro = null) where T : class, new();
    }
}