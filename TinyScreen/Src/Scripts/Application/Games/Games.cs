using System;
using System.Linq;
using Godot;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Scripts.Components.Library;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Application.Games; 

public partial class Games : BaseRouter {
    [Inject] private LibraryService _libraryService;
    
    [Inject] private IDatabaseService _databaseService;

    [Export] private Container _gamesContainer;
    [Export] private Container _foldersContainer;

    [Export] private PackedScene _gameCard;
    [Export] private PackedScene _folder;
        
    public override partial void _Ready();

    [Ready]
    private void Start() {
        var folders = _databaseService.GetAllFolders();
        foreach (var folder in folders!) {
            var folderCard = _folder.Instantiate() as FolderCard;
            folderCard.FolderName = folder.Name;
            folderCard.OnPress = () => Navigate("folder/" + folder.Id);  
            _foldersContainer.AddChild(folderCard);
        }
            
        FollowRoute("folder/" + folders.ElementAt(0).Id);
    }

    [Route("folder")]
    private void OpenFolder(string id) {
        var folder = _databaseService.GetFolder(Int32.Parse(id));
        
        // Draw games!
        ClearGames();
        
        foreach (var game in folder.Games) {
            var gameCard = _gameCard.Instantiate() as GameCard;
            gameCard.Game = game;
            _gamesContainer.AddChild(gameCard);
        }
       
    }

    private void ClearGames() {
        foreach (var child in _gamesContainer.GetChildren()) {
            _gamesContainer.RemoveChild(child);
        }
        _gamesContainer.QueueRedraw();
    }
}