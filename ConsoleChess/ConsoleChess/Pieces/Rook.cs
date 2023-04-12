using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.Pieces
{
    public class Rook : Piece
    {
		public Rook(Rook old) : base(old) { }
		public Rook() { 
			Value = 5;
			PieceName = PieceName.ROOK;
		}
		public override PieceName getPieceName()
        {
            return PieceName.ROOK;
        }

        public override bool IsAttacked(int xNew, int yNew, Square[,] Squares)
        {
			return IsSteppable(xNew, yNew, Squares);
		}
		public override string MoveNotation(int xNew, int yNew, Square[,] Squares, bool isCheck, PieceName promoted)
		{
			string initial = "R";
			initial += base.MoveNotation(xNew, yNew, Squares, isCheck, promoted);
			return initial;
		}

		public override bool IsSteppable(int xNew, int yNew, Square[,] Squares)
        {
			// Check if the new field is the same field current
			if (yNew == Y && xNew == X)
			{
				return false;
			}
			if (xNew == X || yNew == Y)
			{
				int xDifference = (xNew > X) ? 1 : -1;
				int yDifference = (yNew > Y) ? 1 : -1;

				if (xNew == X)
				{
					for (int yIter = Y + yDifference; yIter != yNew; yIter += yDifference)
					{
						if (Squares[X, yIter].Piece != null)
						{
							return false;
						}
					}
				}
				else
				{
					for (int xIter = X + xDifference; xIter != xNew; xIter += xDifference)
					{
						if (Squares[xIter, Y].Piece != null)
						{
							return false;
						}
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
