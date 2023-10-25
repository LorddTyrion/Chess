using ConsoleChess;
using FrameworkBackend;

namespace ReactBoardGame.Hubs
{
    public interface BoardGameClient
    {
        Task GameCreated(IEnumerable<Field> board);
        Task RefreshBoard(IEnumerable<Field> board, bool success);
        Task GameEnds(int result);
        Task AddToGame(List<string> players);
        Task SetColor(bool isStarter, string player);
        Task GetPossibleMoves(IEnumerable<Move> moves);
        Task PreviousMoves(IEnumerable<Move> moves);
        Task RefreshPoints(int first, int second);
    }
}
