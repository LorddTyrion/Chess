using FrameworkBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class TicTacToeField : Field
    {
        public int X { get; set; }
        public int Y { get; set; }
        public TicTacToeType Type { get; set; }
        public TicTacToeField()
        {
            X = 0;  Y = 0; Type = TicTacToeType.EMPTY;
        }
    }
}
