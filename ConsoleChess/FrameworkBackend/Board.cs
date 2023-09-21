using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBackend
{
    public enum Color
    {
        WHITE = 0, BLACK = 1, DRAW = 2, NONE = 3
    }
    public abstract class Board<TMove, TBoardState> where TMove:Move where TBoardState : BoardState
    {
        public TBoardState boardState;
        public abstract bool Move(TMove move);
        public abstract List<TMove> getPossibleMoves(int x, int y);

        public abstract Color CheckEndGame();
    }
}
