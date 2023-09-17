using ConsoleChess;

namespace ReactChess.Services
{
    public enum GameState
    {
        PREPARING, STARTED, FINISHED
    }
    public class Game
    {
        public ChessBoard Board { get; set; }
        public string WhitePlayer { get; set; }
        public string BlackPlayer { get; set; }
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
            if (WhitePlayer == user)
            {
                Result = Color.BLACK;
                State = GameState.FINISHED;
            }
            else if(BlackPlayer == user)
            {
                Result=Color.WHITE;
                State = GameState.FINISHED;
            }
        }
    }
}
