using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Godot;
using GodotOnReady.Attributes;
using HarmonyLib;
using Microsoft.Win32;
using TinyScreen.Framework.Extensions;

namespace TinyScreen.Scripts.Application {
    public partial class Application : Control {

        private enum State {
            Home,
            Games,
            Settings
        }
        
        [OnReadyGet] private BoxContainer _content;

        [Export] private PackedScene _home;
        [Export] private PackedScene _games;
        [Export] private PackedScene _settings;

        private State _currentState = State.Home;

        [OnReady]
        private void Start() {
            var scene = _home.Instance();
            _content.AddChild(scene);
            // Mess with light settings
        }

        private void SetState(State newState) {
            _currentState = newState;
        }
    }
}