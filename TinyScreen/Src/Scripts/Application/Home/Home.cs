using DatabaseWrapper.Core;
using ExpressionTree;
using Godot;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Scripts.Components.Library;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Application.Home {
    public partial class Home : BaseRouter {
        [Inject] private LibraryService _libraryService;

        [Export] private GridContainer _gamesContainer;

        [Export] private PackedScene _gameCard;
        
        public override partial void _Ready();


        [Ready]
        private void Start() {
            /*var games = _libraryService
                .GetAllGames(
                    0,
                    8,
                    new Expr("id", OperatorEnum.IsNotNull, true),
                    new[] { new ResultOrder { ColumnName = "LastPlayed", Direction = OrderDirection.Descending } });

            foreach (var game in games) {
                var gameCard = _gameCard.Instantiate() as GameCard;
                gameCard.Game = game;
                _gamesContainer.AddChild(gameCard);
            }

            if (_gamesContainer != null) {
                var columns = Mathf.FloorToInt(_gamesContainer.Size.x / 150);
                if (_gamesContainer.Columns != columns) {
                    _gamesContainer.Columns = columns;
                    GD.Print("Resized");
                    QueueRedraw();
                }
            }*/
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
}