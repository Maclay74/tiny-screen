using System;
using Godot;
using System.Collections.Generic;
using GodotOnReady.Attributes;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.exceptions;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Onboarding {
    public partial class Library : Control {
        
        [Export] public PackedScene SourcePanel;

        [OnReadyGet] private Label _subtitle;
        [OnReadyGet] private GridContainer _sources;
        [OnReadyGet] private Button _import;

        [Inject] private IEnumerable<ILibrarySource> _librarySources;
        [Inject] private LibraryService _libraryService;
        [Inject] private ModalService _modalService;

        private List<ILibrarySource> _included = new List<ILibrarySource>();

        [OnReady] public void CreateLibrarySources() {
            
            foreach (var source in _librarySources) {
                var panel = SourcePanel.Instance<LibrarySource>();
                panel.source = source;
                _sources.AddChild(panel);

                if (!source.IsInstalled()) 
                    continue;
                
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

                try {
                    await _libraryService.UpdateSource(source, (sender, progress) => {
                        //GD.Print(progress);
                        // TODO display progress on UI
                    });
                }
                catch (Exception exception) {
                    await _modalService.Alert($"Error updating {source.Name()}:\n{exception.Message}");
                }
                
               
            }
            
            GD.Print("Import is done!");
        }
    }
}