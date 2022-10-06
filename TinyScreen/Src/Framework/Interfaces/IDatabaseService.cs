using System.Collections.Generic;
using DatabaseWrapper.Core;
using ExpressionTree;

namespace TinyScreen.Framework.Interfaces {
    public interface IDatabaseService {
        bool Exists();
        void InitDatabase();
        
    }
}