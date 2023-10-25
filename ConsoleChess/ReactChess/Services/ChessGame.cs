
using ConsoleChess;
namespace ReactBoardGame.Services
{
    [AttributeUsage(AttributeTargets.All)]
    public class IdAttribute : Attribute
    {
        public string Id { get; set; }
        public IdAttribute(string id)
        {
            Id = id;
        }
    }
    
    [IdAttribute("3CE78846-2617-4FCF-B3A9-8ACEA38C7F65")]
     public class ChessGame : Game<ChessBoard, ChessBoardState, ChessMove>
    {
        public ChessGame()
        {
            Type = GameType.CHESS;
            Board=new ChessBoard();
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
