using System.Collections.Generic;
using Godot;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;

namespace TinyScreen.Scripts.Onboarding {
    public class Library : Control {

        [Export] public NodePath SubTitlePath;
        private Label _subtitle;

        [Inject] private IEnumerable<ILibrarySource> _librarySources;

        public override void _Ready() {
            base._Ready();

            // Bind UI
            _subtitle = GetNode<Label>(SubTitlePath);

            foreach (var library in _librarySources) {
                GD.Print(library.GetName());
            }
        }
    }
}