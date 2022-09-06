using System;

namespace TinyScreen.Framework.Attributes {

    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InjectAttribute : Attribute {
    }
}