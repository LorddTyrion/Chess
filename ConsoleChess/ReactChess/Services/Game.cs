using ConsoleChess;
using FrameworkBackend;

namespace ReactChess.Services
{
    public enum GameState
    {
        PREPARING, STARTED, FINISHED
    }
    public abstract class Game<TBoard, TBoardState, TMove> : Game
        where TBoard : Board<TBoardState, TMove>, new() 
        where TBoardState : BoardState<TBoardState, TMove>
        where TMove : Move
       
    {      
        public override Color GetTurnOf()
        {
            return Board.GetTurnOf();
        }
        public override IEnumerable<Field> BoardToList()
        {
            return Board.BoardToList();
        }

        public override IEnumerable<Move> GetMoves()
        {
            return Board.GetMoves();
        }
    }
    public abstract class Game
    {
        public Board Board; 
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
            //Board = new ChessBoard();
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
        public abstract Color GetTurnOf();
        public abstract IEnumerable<Field> BoardToList();
        public abstract IEnumerable<Move> GetMoves();
    }
}
