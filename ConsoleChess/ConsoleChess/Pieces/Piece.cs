using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess.Pieces
{
    public enum PieceName
    {
        KING, QUEEN, KNIGHT, BISHOP, ROOK, PAWN
    }
    public abstract class Piece
    {
        public int X { get; set; }
        public int Y { get; set; } 
        public bool IsWhite { get; set; }   
        public bool HasMoved { get; set; }          
        public  int Value { get; protected set; }
        public PieceName PieceName { get; protected set; }
        public abstract PieceName getPieceName();
        public Piece(Piece old)
        {
            X = old.X;
            Y = old.Y;
            IsWhite = old.IsWhite;
            HasMoved = old.HasMoved;
            Value = old.Value;
            PieceName= old.PieceName;
        }
        public Piece() { }

        public bool Move(int xNew, int yNew, Square[,] Squares)
        {
            if(IsSteppable(xNew, yNew, Squares) || IsAttacked(xNew, yNew, Squares))
            {
                Squares[xNew, yNew].Piece = this;
                Squares[X, Y].Piece = null;
                X = xNew;
                Y = yNew;
                HasMoved=true;
                return true;
                //Console.WriteLine(MoveNotation(yNew, xNew, Squares));
            }
            return false;
            
        }
        public abstract bool IsSteppable(int xNew, int yNew, Square[,] Squares);
        public abstract bool IsAttacked(int xNew, int yNew, Square[,] Squares);
        public virtual string MoveNotation(int xNew, int yNew, Square[,] Squares, bool isCheck, PieceName promoted)
        {
            string initial = "";
            char column = 'a';
            column = (char)(Convert.ToUInt16(column) + Y);
            initial += column;
            initial += (X+1);
            if (IsAttacked(xNew, yNew, Squares) && Squares[xNew, yNew].Piece!=null && Squares[xNew, yNew].Piece.IsWhite != IsWhite)
            {
                initial += "x";
            }
            column = 'a';
            column = (char)(Convert.ToUInt16(column)+yNew);
            initial += column;
            initial += (xNew+1);
            switch (promoted)
            {
                case PieceName.PAWN:
                    break;
                case PieceName.ROOK:
                    initial += "=R";
                    break;
                case PieceName.BISHOP:
                    initial += "=B";
                    break;
                case PieceName.KNIGHT:
                    initial += "=N";
                    break;
                case PieceName.QUEEN:
                    initial += "=Q";
                    break;
                case PieceName.KING:
                    break;
                default:
                    break;
            }
            if (isCheck) initial += "+";
            return initial;
            
        }
    }
}
