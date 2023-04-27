using ConsoleChess;

namespace ReactChess.Hubs
{
    public interface ChessClient
    {
        Task GameCreated(List<Square> board);
        Task RefreshBoard(List<Square> board, bool success);
        Task GameEnds(int result);
        Task AddToGame(List<string> players);
        Task SetColor(bool isWhite);
        Task GetPossibleMoves(List<Move> moves);
    }
}
