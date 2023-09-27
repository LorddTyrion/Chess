using FrameworkBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class TicTacToeBoard : Board< TicTacToeBoardState, TicTacToeField>
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
                    if (boardState.TicTacToeField[i, 0].Type != boardState.TicTacToeField[i, j].Type)
                    {
                        circle[i] = false;
                        cross[i] = false;
                    }
                    if (boardState.TicTacToeField[i, 0].Type!=TicTacToeType.CIRCLE) circle[i] = false;
                    if (boardState.TicTacToeField[i, 0].Type != TicTacToeType.CROSS) cross[i] = false;
                    if (boardState.TicTacToeField[0, j].Type != boardState.TicTacToeField[i, j].Type)
                    {
                        circle[j + 3] = false;
                        cross[j + 3] = false;
                    }
                    if (boardState.TicTacToeField[0, j].Type != TicTacToeType.CIRCLE) circle[j+3] = false;
                    if (boardState.TicTacToeField[0, j].Type != TicTacToeType.CROSS) cross[j+3] = false;
                    if (i==j && boardState.TicTacToeField[0, 0].Type != boardState.TicTacToeField[i, j].Type)
                    {
                        circle[6] = false;
                        cross[6] = false;
                    }
                    if (boardState.TicTacToeField[0, 0].Type != TicTacToeType.CIRCLE) circle[6] = false;
                    if (boardState.TicTacToeField[0, 0].Type != TicTacToeType.CROSS) cross[6] = false;
                    if (i+j==2 && boardState.TicTacToeField[0, 2].Type != boardState.TicTacToeField[i, j].Type)
                    {
                        circle[7] = false;
                        cross[7] = false;
                    }
                    if (boardState.TicTacToeField[0, 2].Type != TicTacToeType.CIRCLE) circle[7] = false;
                    if (boardState.TicTacToeField[0, 2].Type != TicTacToeType.CROSS) cross[7] = false;
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
                    if (boardState.TicTacToeField[i, j].Type == TicTacToeType.EMPTY) return Color.NONE;
                }
            }
            return Color.DRAW;

        }

        public override List<TicTacToeMove> getPossibleMoves(int x, int y)
        {
            List<TicTacToeMove> possibleMoves=new List<TicTacToeMove>();
            if(boardState==null) return possibleMoves;
            if (boardState.TicTacToeField[x, y].Type == TicTacToeType.EMPTY)
            {
                TicTacToeMove possibleMove=new TicTacToeMove();
                possibleMove.X=x;
                possibleMove.Y=y;
                possibleMoves.Add(possibleMove);
            }
            return possibleMoves;

        }

        public override bool Move<TMove>(TMove move)
        {
            if(boardState==null) return false;
            TicTacToeMove ticTacToeMove=(TicTacToeMove)(object)move;
            int x=ticTacToeMove.X;
            int y = ticTacToeMove.Y;
            if (boardState.TicTacToeField[x, y].Type == TicTacToeType.EMPTY)
            {
                if (boardState.turnOf == Color.WHITE)                
                    boardState.TicTacToeField[x, y].Type = TicTacToeType.CIRCLE;
                
                else if (boardState.turnOf == Color.BLACK)
                    boardState.TicTacToeField[x, y].Type = TicTacToeType.CROSS;
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
