using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBackend
{
    public abstract class Board
    {
        public abstract bool Move(Move move);
        public abstract IEnumerable<Move> getPossibleMoves(int x, int y);
    }
}
