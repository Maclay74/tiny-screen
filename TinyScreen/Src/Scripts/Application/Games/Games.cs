using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Scripts.Components.Library;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Application.Games; 

public partial class Games : BaseRouter {
    [Inject] private LibraryService _libraryService;

    [Export] private Container _gamesContainer;
    [Export] private Container _foldersContainer;

    [Export] private PackedScene _gameCard;
    [Export] private PackedScene _folder;
        
    public override partial void _Ready();

    [Ready]
    private void Start() {
        base._Ready();
        var folders = new List<string> {"Arcade", "Racing", "Shooter", "Retro"};
            
        foreach (var folderName in folders) {
            var folderCard = _folder.Instantiate() as FolderCard;
            folderCard.FolderName = folderName;
            folderCard.OnPress = OnFolderPress;  
            _foldersContainer.AddChild(folderCard);
        }
            
        Navigate("folder/" + folders.ElementAt(0));
    }
        
    private void OnFolderPress(string name) => Navigate("folder/" + name);

    [Route("folder")]
    private void OpenFolder(string path) {
        Console.WriteLine(path);
    }
}