namespace ReactChess.Services
{
    public enum GameType
    {
        CHESS =0,
        TICTACTOE =1
    }
    public class GameFactory
    {
        public static Game Make(GameType type)
        {
            switch (type)
            {
                case GameType.CHESS:
                    return new ChessGame();
                case GameType.TICTACTOE:
                    return new TicTacToeGame();
                default:
                    return null;
            }
        }
    }
}
