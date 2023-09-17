
using ConsoleChess;
using ConsoleChess.Pieces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReactChess.Data;
using ReactChess.Services;
using ReactChess.Models;

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
            if (_gameController.IsValid(username, gameID))
            {
                bool result = game.Board.Move(move);
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
                    var match=_context.PlayedMatchSet.Where(m=>m.UserID==CurrentUserId && m.MatchId==game.DbID).Include(m=>m.User).Include(m=>m.Match).FirstOrDefault();
                    var match2= _context.PlayedMatchSet.Where(m => m.UserID != CurrentUserId && m.MatchId == game.DbID).Include(m => m.User).FirstOrDefault(); 
                    match.Match.Result = (int)end;
                    match.User.Games++;
                    match2.User.Games++;
                    if (match.Index == (int)end)
                    {
                        match.User.Wins++;
                        match2.User.Losses++;
                    }
                    else if ((int)end == 2)
                    {
                        match.User.Draws++;
                        match2.User.Draws++;
                    }
                    else
                    {
                        match.User.Losses++;
                        match2.User.Wins++;
                    }
                    _context.SaveChanges();
                    
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
                Match m = new Match(); m.Type = 0;
;               _context.MatchSet.Add(m);
                _context.SaveChanges();

                List<string> players = _gameController.PlayersById(gameID);
                PlayedMatch pm1=new PlayedMatch(); 
                PlayedMatch pm2 = new PlayedMatch();
                pm1.UserID = _context.Users.Where(p => p.UserName == players[0]).FirstOrDefault()!.Id;
                pm2.UserID = _context.Users.Where(p => p.UserName == players[1]).FirstOrDefault()!.Id;
                pm1.MatchId = m.Id;
                pm2.MatchId = m.Id;
                if (_gameController.GameById(gameID).WhitePlayer == players[0])
                {
                    pm1.Index = 0; pm2.Index = 1;
                } 
                else if (_gameController.GameById(gameID).BlackPlayer == username)
                {
                    pm1.Index = 1; pm2.Index = 0;
                }
                _context.PlayedMatchSet.Add(pm1);
                _context.PlayedMatchSet.Add(pm2);
                _context.SaveChanges();
                game.DbID=m.Id;

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
            List<ChessMove> possibleMoves = game.Board.getPossibleMoves(x, y) as List<ChessMove>;
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
            var match = _context.PlayedMatchSet.Where(m => m.UserID == CurrentUserId && m.MatchId == game.DbID).Include(m => m.User).Include(m => m.Match).FirstOrDefault();
            var match2 = _context.PlayedMatchSet.Where(m => m.UserID != CurrentUserId && m.MatchId == game.DbID).Include(m => m.User).FirstOrDefault();
            match.Match.Result = game.WhitePlayer==match.User.UserName ? 1:0;
            match.User.Games++;
            match2.User.Games++;
            match.User.Losses++;
            match2.User.Wins++;
            _context.SaveChanges();
            await Clients.Group(gameID.ToString()).GameEnds((int)game.Result);
            _gameController.DeleteGame(game);
        }

        private List<Square> boardToList(ChessBoard board)
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
