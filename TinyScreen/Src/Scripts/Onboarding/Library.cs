using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Onboarding; 

public partial class Library : BaseRouter {
    [Export] public PackedScene SourcePanel;

    [Export] private Label _subtitle;
    [Export] private GridContainer _sources;
    [Export] private Button _import;
    //[Export] private Addons.StagedProgressBar.StagedProgressBar _progressBar;

    [Inject] private IEnumerable<ILibrarySource> _librarySources;
    [Inject] private LibraryService _libraryService;
    [Inject] private ModalService _modalService;

    private List<ILibrarySource> _included = new ();
        
    public override partial void _Ready();

    [Ready]
    private void Start() {
        _import.Pressed += () => Navigate("import");
    }

    [Route("default", true)]
    private void DefaultRoute(string path) {
        foreach (var source in _librarySources) {
            var panel = SourcePanel.Instantiate<Components.Library.LibrarySource>();
            panel.source = source;
            _sources.AddChild(panel);

            if (!source.IsInstalled())
                continue;

            panel.Toogle += (sourceChanged, include) => {
                if (include) _included.Add(sourceChanged);
                else _included.Remove(sourceChanged);
            };

            _included.Add(source);
        }
    }

    [Route("import")]
    private async void Import(string path) {
        var stages = new Dictionary<string, int>();

        foreach (var source in _included) {
            stages.Add(source.Name(), source.GamesCount());
        }

        var overall = stages.Aggregate(0, (acc, pair) => acc + pair.Value);

        /*_progressBar.Stages = stages;
        _progressBar.Progress = 0;

        _progressBar.Show();
        _progressBar.QueueRedraw();*/
        _import.Hide();

        for (int i = 0; i < _included.Count; i++) {
            var source = _included.ElementAt(i);
            _libraryService.AddSource(source);

            try {
                await _libraryService.UpdateSource(source, (sender, progress) => {
                    var gamesDoneForSource = progress * stages.ElementAt(i).Value;
                    var gamesDone = stages.Take(i).Aggregate(gamesDoneForSource, (acc, pair) => acc + pair.Value);
                    //_progressBar.SetProgress(gamesDone / overall);
                    GD.Print(gamesDone / overall);
                });
            }
            catch (Exception exception) {
                await _modalService.Alert($"Error updating {source.Name()}:\n{exception.Message}");
            }
        }

        //_progressBar.SetProgress(1);
        Navigate("/onboarding/finished");
    }
}