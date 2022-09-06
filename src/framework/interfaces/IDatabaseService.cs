using DatabaseWrapper.Core;
using ExpressionTree;

namespace TinyScreen.framework.interfaces {
    public interface IDatabaseService {
        bool IsDatabaseExists();
        void InitDatabase();

        void Insert<T>(T record) where T : class, new();

        T Select<T>(Expr expr, ResultOrder[] ro = null) where T : class, new();
    }
}