using FrameworkBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public enum TicTacToeField
    {
        CIRCLE,
        CROSS,
        EMPTY
    }
    public class TicTacToeBoardState : BoardState<TicTacToeBoardState>
    {
        public TicTacToeField[,] TicTacToeField = new TicTacToeField[3, 3];
        public TicTacToeBoardState()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    TicTacToeField[i, j] = TicTacToe.TicTacToeField.EMPTY;
                }
            }
        }
       

        public override bool PositionEquals(TicTacToeBoardState other)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (TicTacToeField[i, j] != other.TicTacToeField[i, j]) return false;
                }
            }
            return true;
        }
    }
}
