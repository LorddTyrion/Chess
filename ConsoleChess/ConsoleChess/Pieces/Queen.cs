using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.Pieces
{
    public class Queen : Piece
    {
		public Queen(Queen old) : base(old) { }
		public Queen() { Value = 9; PieceName = PieceName.QUEEN; }
		public override PieceName getPieceName()
        {
            return PieceName.QUEEN;
        }

        public override bool IsAttacked(int xNew, int yNew, Square[,] Squares)
        {
			return IsSteppable(xNew, yNew, Squares);
		}
		

		public override bool IsSteppable(int xNew, int yNew, Square[,] Squares)
        {
			//Check if the new field is the same field current
			if (yNew == Y && xNew == X)
			{
				return false;
			}
			//Bishop's step
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
				//Check if there's somebody on the new field and if yes is it the same colour
				if (Squares[xNew,yNew].Piece == null)
				{
					return true;
				}
				else if (Squares[xNew,yNew].Piece.IsWhite != IsWhite)
				{
					return true;
				}

			}
			//Rook's step
			else if (xNew == X || yNew == Y)
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
				//Check if there's somebody on the new field and if yes is it the same colour
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
