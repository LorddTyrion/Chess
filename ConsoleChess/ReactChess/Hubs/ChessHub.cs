
using ConsoleChess;
using ConsoleChess.Pieces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReactChess.Data;
using ReactChess.Services;
using ReactChess.Models;
using FrameworkBackend;

namespace ReactChess.Hubs
{
    [Authorize]
    public class ChessHub : Hub<ChessClient>
    {
      
        public ApplicationDbContext _context;
        public GameController _gameController;
        public DatabaseService _databaseService;
        private string? CurrentUserId => Context.UserIdentifier;

       
        public ChessHub(ApplicationDbContext context, GameController gameController, DatabaseService databaseService)
        {
            _context = context;
            _gameController = gameController;
            _databaseService = databaseService;
        }
        
        public async Task MakeMove(int initialX, int initialY, int targetX, int targetY, int promoteTo)
        {
            ChessMove move = new ChessMove();
            move.InitialX = initialX;
            move.InitialY = initialY;
            move.TargetX = targetX;
            move.TargetY = targetY;
            move.PromoteTo=(PieceName)promoteTo;
            var user = _context.Users.Where(au => au.Id == CurrentUserId).FirstOrDefault();
            var username = user.UserName;
            int gameID=_gameController.IdByName(username);
            Game game=_gameController.GameById(gameID);
            //Game<ChessBoard, ChessMove, ChessBoardState, Square> gameWithBoard=new Game<ChessBoard, ChessMove, ChessBoardState, Square>(game);
            if (_gameController.IsValid(username, gameID, game.Board.boardState.turnOf))
            {
                bool result = game.Board.Move(move);
                Color end = game.Board.CheckEndGame();
                if (result)
                {
                    await Clients.Group(gameID.ToString()).RefreshBoard(game.Board.boardState.boardToList(), true);
                    await Clients.Group(gameID.ToString()).PreviousMoves(game.Board.boardState.moves);
                    await Clients.Group(gameID.ToString()).RefreshPoints(game.Board.GetSumValue(Color.WHITE), game.Board.GetSumValue(Color.BLACK));
                  
                }
                else await Clients.Group(gameID.ToString()).RefreshBoard(game.Board.boardState.boardToList(), false);

                if (end != Color.NONE)
                {
                    game.State = GameState.FINISHED;
                    game.Result = end;
                    _databaseService.GameEndedNaturally(_context, _gameController, game.DbID, CurrentUserId, end);                    
                    await Clients.Group(gameID.ToString()).GameEnds((int)end);
                    _gameController.DeleteGame(game);
                }
            }

          
        }
        public async Task EnterGame()
        {
            var user = _context.Users.Where(au => au.Id == CurrentUserId).FirstOrDefault();
            var username = user.UserName;



            bool result=_gameController.AddPlayer(username);
            int gameID = _gameController.IdByName(username);
            Game game = _gameController.GameById(gameID);

            await Groups.AddToGroupAsync(Context.ConnectionId, gameID.ToString());
            await Clients.Group(gameID.ToString()).AddToGame(_gameController.PlayersById(gameID));

            if(_gameController.GameById(gameID).FirstPlayer==username) await Clients.Caller.SetColor(true, username);
            else if(_gameController.GameById(gameID).SecondPlayer == username) await Clients.Caller.SetColor(false, username);


            if (result)
            {               
                game.DbID=_databaseService.GameSetup(_context, _gameController, gameID);
                List<Square> b = _gameController.GameById(gameID).Board.boardState.boardToList();
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
            List<ChessMove> possibleMoves = game.Board.getPossibleMoves(x, y);
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
            _gameController.DeleteGame(game);
        }

        
        
    }
}
