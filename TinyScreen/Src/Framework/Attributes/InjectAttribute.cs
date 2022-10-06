using System;

namespace TinyScreen.Framework.Attributes; 

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class InjectAttribute : Attribute {
}
    
[AttributeUsage(AttributeTargets.Method)]
public class ReadyAttribute : Attribute {
    
}

[AttributeUsage(AttributeTargets.Method)]
public class InjectorAttribute : Attribute {
    
}