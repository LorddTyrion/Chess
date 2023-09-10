using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBackend
{
    public abstract class BoardState
    {
        public abstract bool PositionEquals (BoardState other);
    }
}
