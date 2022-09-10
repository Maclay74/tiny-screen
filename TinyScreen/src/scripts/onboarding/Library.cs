using System.Collections.Generic;
using System.IO;
using Godot;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;

namespace TinyScreen.Scripts.Onboarding {
    public class Library : Control {

        [Export] public NodePath SubTitlePath;
        [Export] public NodePath SourcesGrid;
        [Export] public NodePath ImportButton;
        [Export] public PackedScene SourcePanel;

        private Label _subtitle;
        private GridContainer _sources;
        private Button _import;

        [Inject] private IEnumerable<ILibrarySource> _librarySources;

        private List<ILibrarySource> _included = new List<ILibrarySource>();

        public override void _Ready() {
            base._Ready();

            // Bind UI
            _subtitle = GetNode<Label>(SubTitlePath);
            _sources = GetNode<GridContainer>(SourcesGrid);
            _import = GetNode<Button>(ImportButton);

            foreach (var library in _librarySources) {
                var panel = SourcePanel.Instance<LibrarySource>();
                panel.source = library;
                _sources.AddChild(panel);

                panel.toogle += (source, include) => {
                    if (include) _included.Add(source);
                    else _included.Remove(source);
                };
            }
            
            _import.Connect("pressed", this, nameof(Import));
        }

        private void Import() {
            GD.Print(_included.Count);
        }
    }
}