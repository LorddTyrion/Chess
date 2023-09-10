using ConsoleChess;

namespace ReactChess.Hubs
{
    public interface ChessClient
    {
        Task GameCreated(List<Square> board);
        Task RefreshBoard(List<Square> board, bool success);
        Task GameEnds(int result);
        Task AddToGame(List<string> players);
        Task SetColor(bool isWhite, string player);
        Task GetPossibleMoves(List<ChessMove> moves);
        Task PreviousMoves(List<ChessMove> moves);
        Task RefreshPoints(int white, int black);
    }
}
