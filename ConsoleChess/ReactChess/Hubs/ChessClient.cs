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
        Task GetPossibleMoves(List<Move> moves);
        Task PreviousMoves(List<Move> moves);
        Task RefreshPoints(int white, int black);
    }
}
