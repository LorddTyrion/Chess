using ConsoleChess;
using FrameworkBackend;

namespace ReactChess.Services
{
    public enum GameState
    {
        PREPARING, STARTED, FINISHED
    }
   /* public class Game<TBoard, TMove, TBoardState, TField> : Game
        where TBoard : Board<TMove, TBoardState, TField>, new() 
        where TMove : Move 
        where TBoardState : BoardState<TBoardState, TField> 
        where TField : Field 
    {
        public TBoard Board { get; set; }
        public Game()
        {
            Board = new TBoard();
        }
        public Game(Game game)
        {
            Board = new TBoard();
            FirstPlayer = game.FirstPlayer;
            SecondPlayer = game.SecondPlayer;
            GameID = game.GameID;
            DbID= game.DbID;
            State = game.State;
            Result = game.Result;
        }
    }*/
    public class Game
    {
        public ChessBoard Board; //to delete
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
        public int GameID { get; set; }
        public int DbID { get; set; }
        public GameState State { get; set; }
        public Color Result { get; set; }
        public Game()
        {
            Random random = new Random();
            GameID=random.Next(1,int.MaxValue);
            Board = new ChessBoard();
            Result = Color.NONE;
        }
        public void LoseGame(string user)
        {
            if (State != GameState.STARTED) return;
            if (FirstPlayer == user)
            {
                Result = Color.BLACK;
                State = GameState.FINISHED;
            }
            else if(SecondPlayer == user)
            {
                Result=Color.WHITE;
                State = GameState.FINISHED;
            }
        }
    }
}
