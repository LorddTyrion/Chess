
using ConsoleChess;
using ConsoleChess.Pieces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace ReactChess.Hubs
{
    [Authorize]
    public class ChessHub : Hub<ChessClient>
    {
      
        public static Board board=new Board();
        public async Task GameStarted()
        {
            board= new Board();
            List<Square> b =boardToList(board);
            await Clients.All.GameCreated(b);
        }
        public async Task MakeMove(int initialX, int initialY, int targetX, int targetY, int promoteTo)
        {
            bool result = board.Move(initialX, initialY, targetX, targetY, (PieceName)promoteTo);
            Color end=board.CheckEndGame();
            
            if (result) await Clients.All.RefreshBoard(boardToList(board), true);
            else await Clients.All.RefreshBoard(boardToList(board), false);

            if (end != Color.NONE) await Clients.All.GameEnds((int)end);
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
