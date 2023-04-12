using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.Pieces
{
    public class Pawn : Piece
    {
		public Pawn(Pawn old) : base(old) { EnPassantable = old.EnPassantable; }
		public bool EnPassantable=false;
        public override PieceName getPieceName()
        {
			return PieceName.PAWN;
		}
		public Pawn() { Value = 1; PieceName = PieceName.PAWN; }

        public override bool IsAttacked(int xNew, int yNew, Square[,] Squares)
        {
			// Check to not go backwards
			if (IsWhite)
			{
				if (xNew < X)
				{
					return false;
				}
			}
			else
			{
				if (xNew > X)
				{
					return false;
				}
			}
			// Capturing - Check if there's somebody on the new field and if yes is it the same colour
			if (Math.Abs(xNew - X) == 1 && Math.Abs(yNew - Y) == 1)
			{
				if (Squares[xNew, yNew].Piece != null)
				{
					if (Squares[xNew, yNew].Piece.IsWhite != IsWhite)
					{
						return true;
					}
				}
				else if (Squares[X, yNew].Piece != null && Squares[X, yNew].Piece.IsWhite != IsWhite && Squares[X, yNew].Piece.getPieceName() == PieceName.PAWN && ((Pawn)Squares[X, yNew].Piece).EnPassantable)
				{
					return true;
				}
			}
			return false;
		}

        public override bool IsSteppable(int xNew, int yNew, Square[,] Squares)
        {
			// Check if the new field is the same field current
			if (yNew == Y && xNew == X)
			{
				return false;
			}
			// Check to not go backwards
			if (IsWhite)
			{
				if (xNew < X)
				{
					return false;
				}
			}
			else
			{
				if (xNew > X)
				{
					return false;
				}
			}
			// Check if the pawn want to step one or two forward and nobody is in the way
			if (yNew == Y)
			{
				if (!HasMoved && Math.Abs(xNew - X) == 2)
				{
					if (Squares[(xNew + X) / 2,Y].Piece != null || Squares[xNew, yNew].Piece != null)
					{
						return false;
					}
					else
					{
						EnPassantable = true;
						return true;
					}
				}
				else
				{
					if (Math.Abs(xNew - X) == 1 && Squares[xNew,yNew].Piece == null)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}

			return false;
		}
    }
}
