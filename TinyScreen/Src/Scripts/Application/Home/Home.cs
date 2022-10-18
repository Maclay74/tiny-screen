using Godot;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Framework.Interfaces;
using TinyScreen.Scripts.Components.Library;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Application.Home;

public partial class Home : BaseRouter {
    [Inject] private LibraryService _libraryService;

    [Inject] private IDatabaseService _databaseService;

    [Export] private GridContainer _gamesContainer;

    [Export] private PackedScene _gameCard;

    public override partial void _Ready();


    [Ready]
    private void Start() {
        foreach (var game in _databaseService.GetAllGames()) {
            var gameCard = _gameCard.Instantiate() as GameCard;
            gameCard.Game = game;
            _gamesContainer.AddChild(gameCard);
        }
    }


    public override void _Notification(long what) {
        base._Notification(what);

        switch (what) {
            case NotificationResized:
                QueueRedraw();
                break;
        }
    }
}