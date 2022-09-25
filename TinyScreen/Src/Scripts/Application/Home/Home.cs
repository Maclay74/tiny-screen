using DatabaseWrapper.Core;
using ExpressionTree;
using Godot;
using GodotOnReady.Attributes;
using TinyScreen.Framework;
using TinyScreen.Framework.Attributes;
using TinyScreen.Scripts.Components.Library;
using TinyScreen.Services;

namespace TinyScreen.Scripts.Application.Home {
    public partial class Home : BaseRouter {
        [Inject] private LibraryService _libraryService;

        [OnReadyGet] private GridContainer _gamesContainer;

        [Export] private PackedScene _gameCard;

        [OnReady]
        private void AddGames() {
            var games = _libraryService
                .GetAllGames(
                    0,
                    8,
                    new Expr("id", OperatorEnum.IsNotNull, true),
                    new[] {new ResultOrder {ColumnName = "LastPlayed", Direction = OrderDirection.Descending}});

            foreach (var game in games) {
                var gameCard = _gameCard.Instance() as GameCard;
                gameCard.Game = game;
                _gamesContainer.AddChild(gameCard);
            }
        }

        [OnReady]
        private async void UpdateLayout() {
            await ToSignal(GetTree(), "idle_frame");

            if (_gamesContainer != null) {
                var columns = Mathf.FloorToInt(_gamesContainer.RectSize.x / 150);
                if (_gamesContainer.Columns != columns) {
                    _gamesContainer.Columns = columns;
                    GD.Print("Resized");
                    Update();
                }
            }
        }

        public override void _Notification(int what) {
            base._Notification(what);

            switch (what) {
                case NotificationResized:
                    UpdateLayout();
                    break;
            }
        }
    }
}