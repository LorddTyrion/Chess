using ConsoleChess;

namespace ReactChess.Services
{
    public enum GameState
    {
        PREPARING, STARTED, FINISHED
    }
    public class Game
    {
        public Board Board { get; set; }
        public string WhitePlayer { get; set; }
        public string BlackPlayer { get; set; }
        public int GameID { get; set; }
        public GameState State { get; set; }
        public Game()
        {
            Random random = new Random();
            GameID=random.Next(1,int.MaxValue);
            Board = new Board();
        }
    }
}
