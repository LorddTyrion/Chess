using ConsoleChess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.Pieces
{
    public class Bishop : Piece
    {
		public Bishop(Bishop old) : base(old) { }
		public Bishop() { Value = 3; PieceName = PieceName.BISHOP; }
		public override PieceName getPieceName()
        {
            return PieceName.BISHOP;
        }
		public override string MoveNotation(int xNew, int yNew, Square[,] Squares, bool isCheck, PieceName promoted)
        {
            string initial = "B";
			initial+=base.MoveNotation(xNew, yNew, Squares, isCheck, promoted);
			return initial;
        }


		public override bool IsAttacked(int xNew, int yNew, Square[,] Squares)
        {
            return IsSteppable(xNew, yNew, Squares);
        }

        public override bool IsSteppable(int xNew, int yNew, Square[,] Squares)
        {
			if (yNew == Y && xNew == X)
			{
				return false;
			}
			if (Math.Abs(xNew - X) == Math.Abs(yNew - Y))
			{

				int xDifference = (xNew > X) ? 1 : -1;
				int yDifference = (yNew > Y) ? 1 : -1;

				for (int xIter = X + xDifference, yIter = Y + yDifference; xIter != xNew; xIter += xDifference, yIter += yDifference)
				{
					if (Squares[xIter,yIter].Piece != null)
					{
						return false;
					}
				}
				// Check if there's somebody on the new field and if yes is it the same colour
				if (Squares[xNew,yNew].Piece == null)
				{
					return true;
				}
				else if (Squares[xNew,yNew].Piece.IsWhite != IsWhite)
				{
					return true;
				}

			}
			return false;
		}
    }
}
