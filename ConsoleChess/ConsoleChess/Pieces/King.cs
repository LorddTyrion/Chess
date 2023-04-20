using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.Pieces
{
    public class King : Piece
    {
		public King(King old) : base(old) { }
		public King() { Value = 0; PieceName = PieceName.KING; }
		public override PieceName getPieceName()
        {
            return PieceName.KING;
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
			if (Math.Abs(xNew - X) <= 1 && Math.Abs(yNew - Y) <= 1)
			{
				if (Squares[xNew,yNew].Piece == null || Squares[xNew,yNew].Piece.IsWhite != IsWhite)
				{
					return true;
				}
			}
			
			return false;
		}
		public bool canCastle(int xNew, int yNew, Board board)
        {
			if (board == null) return false;
			// Castleing
			if (Math.Abs(yNew - Y) == 2 && xNew == X && !HasMoved && !board.IsAttacked(X, Y, IsWhite, board.boardState))
			{
				//Castling short
				if (yNew > Y)
				{
					if (board.boardState.squares[X, 5].Piece == null && !board.IsAttacked(X, 5, IsWhite, board.boardState) && board.boardState.squares[X, 6].Piece == null && !board.IsAttacked(X, 6, IsWhite, board.boardState)
						&& board.boardState.squares[X, 7].Piece != null && board.boardState.squares[X, 7].Piece.getPieceName() == PieceName.ROOK && !board.boardState.squares[X, 7].Piece.HasMoved && board.boardState.squares[X, 7].Piece.IsWhite == IsWhite)
					{
						return true;
					}
				}
				else
				{
					if (board.boardState.squares[X, 3].Piece == null && !board.IsAttacked(X, 3, IsWhite, board.boardState) && board.boardState.squares[X, 2].Piece == null && !board.IsAttacked(X, 2, IsWhite, board.boardState)
						&& board.boardState.squares[X, 1].Piece == null && !board.IsAttacked(X, 1, IsWhite, board.boardState) && board.boardState.squares[X, 0].Piece != null && board.boardState.squares[X, 0].Piece.getPieceName() == PieceName.ROOK
						&& board.boardState.squares[X, 0].Piece.IsWhite == IsWhite && !board.boardState.squares[X, 0].Piece.HasMoved)
					{
						return true;
					}
				}
			}
			return false;

		}
    }
}
