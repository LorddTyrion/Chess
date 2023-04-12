using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess
{
    public class BoardView
    {
        public Board board;
        public BoardView(Board board)
        {
            this.board = board;
        }
        public void Draw()
        {
            Console.WriteLine("{0,4}{1,4}{2,4}{3,4}{4,4}{5,4}{6,4}{7,4}", "a", "b", "c", "d", "e", "f", "g", "h");
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string output= "";
                    if (board.boardState.squares[i, j].Piece == null) { Console.Write("{0,4}", "*"); }
                    else if (board.boardState.squares[i, j].Piece.IsWhite) output += "W";
                    else output += "B";
                    if (board.boardState.squares[i, j].Piece != null)
                    {
                        switch (board.boardState.squares[i, j].Piece.getPieceName())
                        {
                            case Pieces.PieceName.KING:
                                output += "K";
                                break;
                            case Pieces.PieceName.QUEEN:
                                output += "Q";
                                break;
                            case Pieces.PieceName.KNIGHT:
                                output += "N";
                                break;
                            case Pieces.PieceName.ROOK:
                                output += "R";
                                break;
                            case Pieces.PieceName.BISHOP:
                                output += "B";
                                break;
                            case Pieces.PieceName.PAWN:
                                output += "P";
                                break;
                            default:
                                break;
                        }
                        Console.Write("{0,4}", output);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        public void ListPossibleMoves(int x, int y)
        {
            List<string> list=board.getPossibleMoves(x, y);
            foreach (string move in list)
            {
                Console.Write(move+" ");
                Console.WriteLine();
            }

        }
        public bool ValidateInput(char[] chars)
        {
            if (chars == null || chars.Length != 4) return false;
            if (chars[0] < 'a' || chars[0] > 'h' || chars[2] < 'a' || chars[2] > 'h') return false;
            if (chars[1] < '1' || chars[1] > '8' || chars[3] < '1' || chars[3] > '8') return false;
            return true;
        }
    }
}
