using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.Pieces
{

    public class Knight : Piece
    {
		public Knight(Knight old) : base(old) { }
		public Knight() { Value = 3; PieceName = PieceName.KNIGHT; }

		public override PieceName getPieceName()
        {
            return PieceName.KNIGHT;
        }
	

		public override bool IsAttacked(int xNew, int yNew, Square[,] Squares)
        {
            return IsSteppable(xNew, yNew, Squares);
        }

        public override bool IsSteppable(int xNew, int yNew, Square[,] Squares)
        {
			// Check if the new field is the same field current
			if (yNew == Y && xNew == X)
			{
				return false;
			}
			// Check if there's somebody on the new field and if yes is it the same colour
			if (Math.Abs(xNew - X) == 2 && Math.Abs(yNew - Y) == 1 || Math.Abs(xNew - X) == 1 && Math.Abs(yNew - Y) == 2)
			{
				if (Squares[xNew,yNew].Piece != null)
				{
					if (Squares[xNew,yNew].Piece.IsWhite != IsWhite)
					{
						return true;
					}
					return false;
				}
				else
				{
					return true;
				}
			}

			return false;
		}
    }
}
