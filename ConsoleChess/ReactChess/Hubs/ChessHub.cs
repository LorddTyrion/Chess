
using ConsoleChess;
using ConsoleChess.Pieces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ReactChess.Data;
using ReactChess.Services;

namespace ReactChess.Hubs
{
    [Authorize]
    public class ChessHub : Hub<ChessClient>
    {
      
        public ApplicationDbContext _context;
        public GameController _gameController;
        private string? CurrentUserId => Context.UserIdentifier;

       
        public ChessHub(ApplicationDbContext context, GameController gameController)
        {
            _context = context;
            _gameController = gameController;
        }
        
        public async Task MakeMove(int initialX, int initialY, int targetX, int targetY, int promoteTo)
        {
            var user = _context.Users.Where(au => au.Id == CurrentUserId).FirstOrDefault();
            var username = user.UserName;
            int gameID=_gameController.IdByName(username);
            Game game=_gameController.GameById(gameID);
            if (_gameController.IsValid(username, gameID))
            {
                bool result = game.Board.Move(initialX, initialY, targetX, targetY, (PieceName)promoteTo);
                Color end = game.Board.CheckEndGame();
                if (result)
                {
                    await Clients.Group(gameID.ToString()).RefreshBoard(boardToList(game.Board), true);
                    await Clients.Group(gameID.ToString()).PreviousMoves(game.Board.boardState.moves);
                    await Clients.Group(gameID.ToString()).RefreshPoints(game.Board.GetSumValue(Color.WHITE), game.Board.GetSumValue(Color.BLACK));
                  
                }
                else await Clients.Group(gameID.ToString()).RefreshBoard(boardToList(game.Board), false);

                if (end != Color.NONE)
                {
                    game.State = GameState.FINISHED;
                    game.Result = end;
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
            if(_gameController.GameById(gameID).WhitePlayer==username) await Clients.Caller.SetColor(true, username);
            else if(_gameController.GameById(gameID).BlackPlayer == username) await Clients.Caller.SetColor(false, username);
            if (result)
            {
                List<Square> b = boardToList(_gameController.GameById(gameID).Board);
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
            List<Move> possibleMoves = game.Board.getPossibleMoves(x, y);
            await Clients.Group(gameID.ToString()).GetPossibleMoves(possibleMoves);
        }
        public async Task LoseGame()
        {
            var user = _context.Users.Where(au => au.Id == CurrentUserId).FirstOrDefault();
            var username = user.UserName;
            int gameID = _gameController.IdByName(username);
            Game game = _gameController.GameById(gameID);
            game.LoseGame(username);
            await Clients.Group(gameID.ToString()).GameEnds((int)game.Result);
            _gameController.DeleteGame(game);
        }

        private List<Square> boardToList(Board board)
        {
            List<Square> b = new List<Square>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    b.Add(board.boardState.squares[i, j]);
                }
            }
            return b;
        }
        
    }
}
