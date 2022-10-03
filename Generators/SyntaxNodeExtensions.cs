using System;
using Microsoft.CodeAnalysis;

namespace Generators; 

public static class SyntaxNodeExtensions {
    public static T GetParent<T>(this SyntaxNode node) {
        var parent = node.Parent;

        while (true) {
            if (parent == null) throw new Exception();
            if (parent is T result) return result;
            parent = parent.Parent;
        }
    }
}