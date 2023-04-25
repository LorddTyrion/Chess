
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
        public static GameController gameController=new GameController();
      
        public static Board board=new Board();
        public ApplicationDbContext _context;
        private string? CurrentUserId => Context.UserIdentifier;

        private static int playerCount = 0;
        private static int firstColor=0;

        static List<string> _players = new List<string>();
        public ChessHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task GameStarted()
        {
            board= new Board();
            List<Square> b =boardToList(board);
            await Clients.All.GameCreated(b);
        }
        public async Task MakeMove(int initialX, int initialY, int targetX, int targetY, int promoteTo)
        {
            var user = _context.Users.Where(au => au.Id == CurrentUserId).FirstOrDefault();
            var username = user.UserName;
            int gameID=gameController.IdByName(username);
            Game game=gameController.GameById(gameID);
            if (gameController.IsValid(username, gameID))
            {
                bool result = game.Board.Move(initialX, initialY, targetX, targetY, (PieceName)promoteTo);
                Color end = game.Board.CheckEndGame();
                if (result) await Clients.Group(gameID.ToString()).RefreshBoard(boardToList(game.Board), true);
                else await Clients.Group(gameID.ToString()).RefreshBoard(boardToList(game.Board), false);

                if (end != Color.NONE)
                {
                    game.State = GameState.FINISHED;
                    await Clients.Group(gameID.ToString()).GameEnds((int)end);
                }
            }

            /*bool result = board.Move(initialX, initialY, targetX, targetY, (PieceName)promoteTo);
            Color end=board.CheckEndGame();
            
            if (result) await Clients.All.RefreshBoard(boardToList(board), true);
            else await Clients.All.RefreshBoard(boardToList(board), false);

            if (end != Color.NONE) await Clients.All.GameEnds((int)end);*/
        }
        public async Task EnterGame()
        {
            /*if (playerCount <= 1)
            {
                var user = _context.Users.Where(au => au.Id == CurrentUserId).FirstOrDefault();
                var username = user.UserName;
                playerCount++;
                _players.Add(username);
                if (firstColor == 0)
                {
                    Random vel=new Random();
                    firstColor = vel.Next(1, 3);
                    await Clients.All.AddToGame(_players);
                    await Clients.Caller.SetColor(firstColor == 2);
                }
                else
                {
                    await Clients.All.AddToGame(_players);
                    await Clients.Caller.SetColor(firstColor != 2);
                }
                
            }*/
            var user = _context.Users.Where(au => au.Id == CurrentUserId).FirstOrDefault();
            var username = user.UserName;
            bool result=gameController.AddPlayer(username);
            int gameID = gameController.IdByName(username);
            await Groups.AddToGroupAsync(Context.ConnectionId, gameID.ToString());
            await Clients.Group(gameID.ToString()).AddToGame(gameController.PlayersById(gameID));
            if(gameController.GameById(gameID).WhitePlayer==username) await Clients.Caller.SetColor(true);
            else if(gameController.GameById(gameID).BlackPlayer == username) await Clients.Caller.SetColor(false);
            if (result)
            {
                List<Square> b = boardToList(gameController.GameById(gameID).Board);
                await Clients.Group(gameID.ToString()).GameCreated(b);
            }
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
