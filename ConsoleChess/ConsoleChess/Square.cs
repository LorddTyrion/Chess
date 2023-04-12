using ConsoleChess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess
{
    public class Square
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Piece Piece { get; set; }
        public Square() { }
        public Square(Square old)
        {
            X = old.X;  
            Y = old.Y;
            if(old.Piece != null)
            {
                switch (old.Piece.getPieceName())
                {
                    case PieceName.KING:
                        King PieceKing = new King((King)old.Piece);
                        Piece = PieceKing;
                        break;
                    case PieceName.QUEEN:
                        Queen PieceQueen = new Queen((Queen)old.Piece);
                        Piece = PieceQueen;
                        break;
                    case PieceName.KNIGHT:
                        Knight PieceKnight = new Knight((Knight)old.Piece);
                        Piece = PieceKnight;
                        break;
                    case PieceName.BISHOP:
                        Bishop PieceBishop = new Bishop((Bishop)old.Piece);
                        Piece=PieceBishop;
                        break;
                    case PieceName.ROOK:
                        Rook PieceRook = new Rook((Rook)old.Piece);
                        Piece= PieceRook;
                        break;
                    case PieceName.PAWN:
                        Pawn PiecePawn = new Pawn((Pawn)old.Piece);
                        Piece=PiecePawn;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
