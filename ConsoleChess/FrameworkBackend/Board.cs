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
    public abstract class Board<TBoardState, TField> where TBoardState : BoardState<TBoardState, TField> where TField : Field
    {
        public TBoardState boardState;
        public abstract bool Move<TMove>(TMove move) where TMove: Move;
        public abstract IEnumerable<Move> getPossibleMoves(int x, int y);

        public abstract Color CheckEndGame();
    }
}
