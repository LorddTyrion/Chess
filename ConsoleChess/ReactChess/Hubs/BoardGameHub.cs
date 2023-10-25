
using ConsoleChess;
using ConsoleChess.Pieces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReactBoardGame.Data;
using ReactBoardGame.Services;
using ReactBoardGame.Models;
using FrameworkBackend;
using TicTacToe;

namespace ReactBoardGame.Hubs
{
    [Authorize]
    public class BoardGameHub : Hub<BoardGameClient>
    {
      
        public ApplicationDbContext _context;
        public GameController _gameController;
        public DatabaseService _databaseService;
        private string? CurrentUserId => Context.UserIdentifier;

       
        public BoardGameHub(ApplicationDbContext context, GameController gameController, DatabaseService databaseService)
        {
            _context = context;
            _gameController = gameController;
            _databaseService = databaseService;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = _context.Users.Where(au => au.Id == CurrentUserId).FirstOrDefault();
            var username = user.UserName;
            int gameID = _gameController.IdByName(username);
            Game game = _gameController.GameById(gameID);
            if (game == null) return;
            game.LoseGame(username);
            _databaseService.GameEndedByResignation(_context, CurrentUserId, game);
            await Clients.Group(gameID.ToString()).GameEnds((int)game.Result);
            _gameController.DeleteGame(game, (int)game.Type);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task MakeMove(string stringifiedMove, int gametype)
        {
            Move move=new ChessMove();
            switch (gametype)
            {
                case 0:
                    move = new ChessMove().Generate(stringifiedMove);
                    break;
                case 1:
                    move=new TicTacToeMove().Generate(stringifiedMove);
                    break;
                default:
                    break;
            }
            
            var user = _context.Users.Where(au => au.Id == CurrentUserId).FirstOrDefault();
            var username = user.UserName;
            int gameID=_gameController.IdByName(username);
            Game game=_gameController.GameById(gameID);

            if (_gameController.IsValid(username, gameID, game.GetTurnOf()))
            {
                bool result = game.Board.Move(move);
                Color end = game.Board.CheckEndGame();
                if (result)
                {
                    _databaseService.SerializeBoard(_context, CurrentUserId, game);
                    await Clients.Group(gameID.ToString()).RefreshBoard(game.BoardToList(), true);
                    await Clients.Group(gameID.ToString()).PreviousMoves(game.GetMoves());
                    await Clients.Group(gameID.ToString()).RefreshPoints(game.Board.GetSumValue(Color.WHITE), game.Board.GetSumValue(Color.BLACK));
                  
                }
                else await Clients.Group(gameID.ToString()).RefreshBoard(game.BoardToList(), false);

                if (end != Color.NONE)
                {
                    game.State = GameState.FINISHED;
                    game.Result = end;
                    _databaseService.GameEndedNaturally(_context, _gameController, game.DbID, CurrentUserId, end);                    
                    await Clients.Group(gameID.ToString()).GameEnds((int)end);
                    _gameController.DeleteGame(game, gametype);
                }
            }

          
        }
        public async Task EnterGame(int gametype)
        {
            var user = _context.Users.Where(au => au.Id == CurrentUserId).FirstOrDefault();
            var username = user.UserName;



            bool result=_gameController.AddPlayer(username, gametype);
            int gameID = _gameController.IdByName(username);
            Game game = _gameController.GameById(gameID);

            await Groups.AddToGroupAsync(Context.ConnectionId, gameID.ToString());
            await Clients.Group(gameID.ToString()).AddToGame(_gameController.PlayersById(gameID));

            if(_gameController.GameById(gameID).FirstPlayer==username) await Clients.Caller.SetColor(true, username);
            else if(_gameController.GameById(gameID).SecondPlayer == username) await Clients.Caller.SetColor(false, username);


            if (result)
            {               
                game.DbID=_databaseService.GameSetup(_context, _gameController, gameID);
                IEnumerable<Field> b = _gameController.GameById(gameID).BoardToList();
                await Clients.Group(gameID.ToString()).GameCreated(b);
                await Clients.Group(gameID.ToString()).RefreshPoints(game.Board.GetSumValue(Color.WHITE), game.Board.GetSumValue(Color.BLACK));
            }
        }
        public async Task PossibleMoves(int x, int y)
        {
            var user = _context.Users.Where(au => au.Id == CurrentUserId).FirstOrDefault();
            var username = user.UserName;
            int gameID = _gameController.IdByName(username);
            Game game = _gameController.GameById(gameID);
            IEnumerable<Move> possibleMoves = game.Board.getPossibleMoves(x, y);
            await Clients.Group(gameID.ToString()).GetPossibleMoves(possibleMoves);
        }
        public async Task LoseGame()
        {
            var user = _context.Users.Where(au => au.Id == CurrentUserId).FirstOrDefault();
            var username = user.UserName;
            int gameID = _gameController.IdByName(username);
            Game game = _gameController.GameById(gameID);
            if (game == null) return;
            game.LoseGame(username);
            _databaseService.GameEndedByResignation(_context, CurrentUserId, game);
            await Clients.Group(gameID.ToString()).GameEnds((int)game.Result);
            _gameController.DeleteGame(game, (int)game.Type);
        }

        
        
    }
}
