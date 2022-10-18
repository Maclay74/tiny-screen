using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Godot;
using TinyScreen.Framework.Attributes;
using TinyScreen.scripts;

namespace TinyScreen.Framework;

public sealed class RouteState : IEquatable<RouteState> {
    public string Path;
    public object[] Args;

    public RouteState(string path, object[] args) {
        Path = path;
        Args = args;
    }

    public bool Equals(RouteState other) {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Path == other.Path && Equals(Args, other.Args);
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((RouteState)obj);
    }

    public override int GetHashCode() {
        unchecked {
            return ((Path != null ? Path.GetHashCode() : 0) * 397) ^ (Args != null ? Args.GetHashCode() : 0);
        }
    }
}

public abstract partial class BaseRouter : Control {
    private static Stack<RouteState> _history = new();

    public void Navigate(string path, params object[] args) {
        var state = new RouteState(path, args);
        _history.Push(state);

        Console.WriteLine($"[Navigate] path: {state.Path} args: {String.Join(",", state.Args)}");
        
        FollowRoute(state.Path, state.Args);
    }

    public void Back() {
        _history.Pop();
        var state = _history.Last();
        
        Console.WriteLine($"[Back] path: {state.Path} args: {String.Join(",", state.Args)}");
        
        FollowRoute(state.Path, state.Args);
    }

    public void FollowRoute(string path, params object[] args) {
        FindRouteMethod(path, out var routeMethod, out var pathLeft);

        if (routeMethod == null)
            return;
        
        var newArgs = new object[] { pathLeft }.Concat(args).ToArray();
        
        Console.WriteLine($"[FollowRoute] root: {GetRoot(path).Name} path: {path} method: {routeMethod.Name} pathLeft: {pathLeft} args: {String.Join(",", newArgs)}");
        
        routeMethod.Invoke(GetRoot(path), newArgs);
    }
    
    private BaseRouter GetRoot(string path) {
        return path.ElementAtOrDefault(0) == '/' ? GetNode<RootNode>("/root/Root") : this;
    }

    private void FindRouteMethod(string path, out MethodInfo? methodInfo, out string pathLeft) {
        var root = GetRoot(path).GetType();
        path = Regex.Replace(path, "^/", ""); // RemoveAt slash from the beginning
        methodInfo = null;
        pathLeft = path;

        if (!root.IsSubclassOf(typeof(BaseRouter))) {
            throw new Exception($"{Name}({root.Name}) should be a subclass of BaseRouter");
        }

        var routes = root
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => m.GetCustomAttributes(typeof(RouteAttribute)).Any());

        foreach (var route in routes) {
            RouteAttribute? attribute = route.GetCustomAttribute(typeof(RouteAttribute)) as RouteAttribute;
            pathLeft = Regex.Replace(path, attribute?.Path + "([/]?)", "");

            var isDefault = pathLeft.Length == 0 && attribute.IsDefault;
            var match = Regex.Match(path, "^" + attribute?.Path);

            if (!match.Success && !isDefault)
                continue;

            methodInfo = route;
            return;
        }
    }
}