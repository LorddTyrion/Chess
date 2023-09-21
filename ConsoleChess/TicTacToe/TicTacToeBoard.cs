using FrameworkBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class TicTacToeBoard : Board<TicTacToeMove, TicTacToeBoardState>
    {
        public TicTacToeBoard()
        {
            boardState = new TicTacToeBoardState();
            boardState.turnOf = Color.WHITE; //change to circle
        }
        public override Color CheckEndGame()
        {
            bool[] circle=new bool[8];
            bool[] cross=new bool[8];
            for (int i = 0; i < 8; i++)
            {
                circle[i] = true;
                cross[i] = true;
            }
            for(int i=0; i<3; i++)
            {
                for (int j = 0; j < 3; j++) {
                    if (boardState.TicTacToeField[i, 0] != boardState.TicTacToeField[i, j])
                    {
                        circle[i] = false;
                        cross[i] = false;
                    }
                    if (boardState.TicTacToeField[i, 0]!=TicTacToeField.CIRCLE) circle[i] = false;
                    if (boardState.TicTacToeField[i, 0] != TicTacToeField.CROSS) cross[i] = false;
                    if (boardState.TicTacToeField[0, j] != boardState.TicTacToeField[i, j])
                    {
                        circle[j + 3] = false;
                        cross[j + 3] = false;
                    }
                    if (boardState.TicTacToeField[0, j] != TicTacToeField.CIRCLE) circle[j+3] = false;
                    if (boardState.TicTacToeField[0, j] != TicTacToeField.CROSS) cross[j+3] = false;
                    if (i==j && boardState.TicTacToeField[0, 0] != boardState.TicTacToeField[i, j])
                    {
                        circle[6] = false;
                        cross[6] = false;
                    }
                    if (boardState.TicTacToeField[0, 0] != TicTacToeField.CIRCLE) circle[6] = false;
                    if (boardState.TicTacToeField[0, 0] != TicTacToeField.CROSS) cross[6] = false;
                    if (i+j==2 && boardState.TicTacToeField[0, 2] != boardState.TicTacToeField[i, j])
                    {
                        circle[7] = false;
                        cross[7] = false;
                    }
                    if (boardState.TicTacToeField[0, 2] != TicTacToeField.CIRCLE) circle[7] = false;
                    if (boardState.TicTacToeField[0, 2] != TicTacToeField.CROSS) cross[7] = false;
                }
            }
            for (int i = 0; i < 8; i++)
            {
                //Console.WriteLine(i);
                if (circle[i] == true) return Color.WHITE;                
                if(cross[i] == true) return Color.BLACK;
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (boardState.TicTacToeField[i, j] == TicTacToeField.EMPTY) return Color.NONE;
                }
            }
            return Color.DRAW;

        }

        public override List<TicTacToeMove> getPossibleMoves(int x, int y)
        {
            List<TicTacToeMove> possibleMoves=new List<TicTacToeMove>();
            if(boardState==null) return possibleMoves;
            if (boardState.TicTacToeField[x, y] == TicTacToeField.EMPTY)
            {
                TicTacToeMove possibleMove=new TicTacToeMove();
                possibleMove.X=x;
                possibleMove.Y=y;
                possibleMoves.Add(possibleMove);
            }
            return possibleMoves;

        }

        public override bool Move(TicTacToeMove move)
        {
            if(boardState==null) return false;
            int x=move.X;
            int y = move.Y;
            if (boardState.TicTacToeField[x, y] == TicTacToeField.EMPTY)
            {
                if (boardState.turnOf == Color.WHITE)                
                    boardState.TicTacToeField[x, y] = TicTacToeField.CIRCLE;
                
                else if (boardState.turnOf == Color.BLACK)
                    boardState.TicTacToeField[x, y] = TicTacToeField.CROSS;
                changeStarter();
                return true;
            }
            return false;
        }
        private void changeStarter()
        {
            if (boardState.turnOf == Color.WHITE)
                boardState.turnOf = Color.BLACK;

            else if (boardState.turnOf == Color.BLACK)
                boardState.turnOf = Color.WHITE;
        }
    }
}
