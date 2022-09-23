using System;

namespace TinyScreen.Framework.Attributes {
    
    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteAttribute : Attribute {

        public string Path { get; }
        
        public bool IsDefault { get;  }

        public RouteAttribute(string path, bool isDefault = false) {
            Path = path;
            IsDefault = isDefault;
        }
    }
    
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public class RouterAttribute : Attribute {
    }
}