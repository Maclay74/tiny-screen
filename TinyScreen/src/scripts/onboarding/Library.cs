using Godot;
using System.Collections.Generic;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Services;

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
        [Inject] private LibraryService _libraryService;

        private List<ILibrarySource> _included = new List<ILibrarySource>();

        public override void _Ready() {
            base._Ready();

            // Bind UI
            _subtitle = GetNode<Label>(SubTitlePath);
            _sources = GetNode<GridContainer>(SourcesGrid);
            _import = GetNode<Button>(ImportButton);

            foreach (var source in _librarySources) {
                var panel = SourcePanel.Instance<LibrarySource>();
                panel.source = source;
                _sources.AddChild(panel);

                panel.toogle += (sourceChanged, include) => {
                    if (include) _included.Add(sourceChanged);
                    else _included.Remove(sourceChanged);
                };
                _included.Add(source);
            }
            
            _import.Connect("pressed", this, nameof(Import));
        }

        private async void Import() {
            foreach (var source in _included) {
                _libraryService.AddSource(source);
                await _libraryService.UpdateSource(source, (sender, progress) => {
                    //GD.Print(progress);
                    // TODO display progress on UI
                });
            }
            
            GD.Print("Import is done!");
        }
    }
}