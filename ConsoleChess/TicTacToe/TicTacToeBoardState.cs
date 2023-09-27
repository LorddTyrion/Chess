using FrameworkBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public enum TicTacToeType
    {
        CIRCLE,
        CROSS,
        EMPTY
    }
    public class TicTacToeBoardState : BoardState<TicTacToeBoardState, TicTacToeField>
    {
        public TicTacToeField[,] TicTacToeField = new TicTacToeField[3, 3];
        public TicTacToeBoardState()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    TicTacToeField[i, j] = new TicTacToeField();
                    TicTacToeField[i,j].X = i;
                    TicTacToeField[i,j].Y = j;
                }
            }
        }

        public override List<TicTacToeField> boardToList()
        {
            List<TicTacToeField> list=new List<TicTacToeField> ();
            for(int i = 0; i < 3; i++)
            {
                for(int j= 0; j < 3; j++)
                {
                    list.Add(TicTacToeField[i,j]);
                }
            }
            return list;
        }

        public override bool PositionEquals(TicTacToeBoardState other)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (TicTacToeField[i, j].Type != other.TicTacToeField[i, j].Type) return false;
                }
            }
            return true;
        }
    }
}
