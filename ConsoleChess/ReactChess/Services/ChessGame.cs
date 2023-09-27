
using ConsoleChess;
namespace ReactChess.Services
{
    public class ChessGame : Game<ChessBoard, ChessBoardState, ChessMove>
    {
        public ChessGame()
        {
            Board=new ChessBoard();
            Console.WriteLine("nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn");
        }
        public ChessGame(ChessGame game)
        {
            Board = new ChessBoard();
            FirstPlayer = game.FirstPlayer;
            SecondPlayer = game.SecondPlayer;
            GameID = game.GameID;
            DbID = game.DbID;
            State = game.State;
            Result = game.Result;
        }
    }
}
