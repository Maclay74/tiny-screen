using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Godot;
using TinyScreen.Framework.Attributes;
using TinyScreen.scripts;

namespace TinyScreen.Framework {
    
    public class RouteState: IEquatable<RouteState> {
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
            return Equals((RouteState) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((Path != null ? Path.GetHashCode() : 0) * 397) ^ (Args != null ? Args.GetHashCode() : 0);
            }
        }
    }
    
    public abstract class BaseRouter : Control {

        protected RouteState _currentState;

        public void Navigate(string path, params object[] args) {
            
            BaseRouter root = path.ElementAtOrDefault(0) == '/' ? GetRoot() : this;
            var rootType = root.GetType();
            path = Regex.Replace(path, "^/", ""); // Remove slash from the beginning

            if (rootType.IsSubclassOf(typeof(BaseRouter))) {
                var routes = rootType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(m => m.GetCustomAttributes(typeof(RouteAttribute)).Any());
                
                foreach (var route in routes) {
                    RouteAttribute attribute = route.GetCustomAttribute(typeof(RouteAttribute)) as RouteAttribute;
                    var pathLeft = Regex.Replace(path, attribute?.Path + "([/]?)", "");
                    var newArgs = new object[] {pathLeft}.Concat(args).ToArray();
                    var state = new RouteState(pathLeft, newArgs);
                    var isDefault = pathLeft.Length == 0 && attribute.IsDefault;
                    var match = Regex.Match(path, "^" + attribute?.Path);

                    //Console.WriteLine($"[{Name}]: {path} / {attribute?.Path} {match.Success}");
                    
                    if (match.Success || isDefault) {
                        Console.WriteLine($"[{Name}]: Calling method {route.Name}");
                        if (_currentState == null || !_currentState.Equals(state)) {
                            _currentState = state;
                            route.Invoke(root, newArgs);
                        }
                    }
                }
            }
            else {
                throw new Exception($"{Name}({rootType.Name}) should be a subclass of BaseRouter");
            }
        }

        private BaseRouter GetRoot() {
            return GetNode<RootNode>("/root/Root");
        }
    }
}