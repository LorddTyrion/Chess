using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBackend
{
    public abstract class Board<TMove> where TMove:Move
    {
        public abstract bool Move(TMove move);
        public abstract IEnumerable<Move> getPossibleMoves(int x, int y);
    }
}
