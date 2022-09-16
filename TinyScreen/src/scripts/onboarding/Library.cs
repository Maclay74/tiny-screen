using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using GodotOnReady.Attributes;
using TinyScreen.addons.StagedProgressBar;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Exceptions;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Onboarding {
    public partial class Library : Control {
        
        [Export] public PackedScene SourcePanel;

        [OnReadyGet] private Label _subtitle;
        [OnReadyGet] private GridContainer _sources;
        [OnReadyGet] private Button _import;
        [OnReadyGet] private StagedProgressBar _progressBar;

        [Inject] private IEnumerable<ILibrarySource> _librarySources;
        [Inject] private LibraryService _libraryService;
        [Inject] private ModalService _modalService;

        private List<ILibrarySource> _included = new List<ILibrarySource>();

        [OnReady] public void CreateLibrarySources() {
            
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

            var stages = new Dictionary<string, int>();

            foreach (var source in _included) {
                stages.Add(source.Name(), source.GamesCount());
            }
            
            var overall = stages.Aggregate(0, (acc, pair) => acc + pair.Value);

            _progressBar.Stages = stages;
            _progressBar.Progress = 0;
            
            _progressBar.Show();
            _progressBar.Update();
            _import.Hide();

            for (int i = 0; i < _included.Count; i++) {
                var source = _included.ElementAt(i);
                _libraryService.AddSource(source);
                
                try {
                    await _libraryService.UpdateSource(source, (sender, progress) => {
                        var gamesDoneForSource = progress * stages.ElementAt(i).Value;
                        var gamesDone = stages.Take(i).Aggregate(gamesDoneForSource, (acc, pair) => acc + pair.Value);
                        _progressBar.SetProgress(gamesDone / overall);
                    });
                }
                catch (Exception exception) {
                    await _modalService.Alert($"Error updating {source.Name()}:\n{exception.Message}");
                }
            }
            
            _progressBar.SetProgress(1);
            
            GD.Print("Import is done!");
        }
    }
}