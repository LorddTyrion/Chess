using ConsoleChess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess
{
    public class Move
    {
        public int InitialX { get; set; }
        public int InitialY { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }
        public PieceName Piece { get; set; } 
        public PieceName PromoteTo { get; set; }
        public bool isCheck { get; set; }
        public bool isCapture { get; set; }
        public Move()
        {
            InitialX = 0;
            InitialY = 0;
            TargetX = 0;
            TargetY = 0;
            isCheck = false;
            isCapture = false;
            Piece = PieceName.PAWN;
            PromoteTo = PieceName.PAWN;
        }

        public override string ToString()
        {
            if (Piece == PieceName.KING && TargetY - InitialY == 2) return "O-O";
            if (Piece == PieceName.KING && InitialY - TargetY == 2) return "O-O-O";
            string initial = "";
            switch (Piece)
            {
                case PieceName.KING: initial += "K"; break;
                case PieceName.QUEEN: initial += "Q"; break;
                case PieceName.ROOK: initial += "R"; break;
                case PieceName.BISHOP: initial += "B"; break;
                case PieceName.KNIGHT: initial += "N"; break;
                default: break;
            }
            char column = 'a';
            column = (char)(Convert.ToUInt16(column) + InitialY);
            initial += column;
            initial += (InitialX + 1);

            if (isCapture) initial += "x";

            column = 'a';
            column = (char)(Convert.ToUInt16(column) + TargetY);
            initial += column;
            initial += (TargetX + 1);
            

            switch (PromoteTo)
            {
                case PieceName.QUEEN: initial += "=Q"; break;
                case PieceName.ROOK: initial += "=R"; break;
                case PieceName.BISHOP: initial += "=B"; break;
                case PieceName.KNIGHT: initial += "=N"; break;
                default: break;
            }
            if (isCheck) initial += "+";
            return initial;
        }
    }
}
