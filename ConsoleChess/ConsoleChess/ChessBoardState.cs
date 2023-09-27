using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleChess.Pieces;
using FrameworkBackend;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleChess
{
    public class ChessBoardState : BoardState<ChessBoardState, ChessMove>
    {
        
        public List<Piece> WhitePieces = new List<Piece>();        
        public List<Piece> BlackPieces = new List<Piece>();
        [JsonIgnore]
        public King? WhiteKing, BlackKing;
        [JsonIgnore]
        public Square[,] squares = new Square[8, 8];
        
        public int FiftyMoveRule = 0;
        public ChessBoardState(ChessBoardState old)
        {
            turnOf = old.turnOf;
            WhitePieces = new List<Piece>();
            BlackPieces = new List<Piece>();
            for (int i = 0; i < 8; i++)
            {

                for (int j = 0; j < 8; j++)
                {
                    squares[i, j] = new Square();
                    squares[i, j].X = i;
                    squares[i, j].Y = j;
                }
            }

            moves = new List<ChessMove>();
            for (int i = 0; i < old.moves.Count; i++)
            {
                moves.Add(old.moves[i]);
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Square s = new Square(old.squares[i, j]);
                    squares[i, j] = s;
                    if (s.Piece != null)
                    {
                        if (old.squares[i, j].Piece.IsWhite)
                        {
                            WhitePieces.Add(squares[i, j].Piece);
                            if (squares[i, j].Piece.getPieceName() == PieceName.KING) WhiteKing = (King)squares[i, j].Piece;
                        }
                        else
                        {
                            BlackPieces.Add(squares[i, j].Piece);
                            if (squares[i, j].Piece.getPieceName() == PieceName.KING) BlackKing = (King)squares[i, j].Piece;
                        }
                    }
                }
            }
        }
        public ChessBoardState()
        {
            WhitePieces = new List<Piece>();
            BlackPieces = new List<Piece>();

            squares = new Square[8, 8];
            moves = new List<ChessMove>();
        }

        public override List<Square> boardToList()
        {
            List<Square> b = new List<Square>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    b.Add(squares[i, j]);
                }
            }
            return b;
        }
        public override bool PositionEquals(BoardState otherBoard)
        {
            if (otherBoard is not ChessBoardState) return false;
            if (otherBoard == null) return false;
            ChessBoardState other = (ChessBoardState)otherBoard;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (squares[i, j].Piece == null && other.squares[i, j].Piece != null) return false;
                    if (squares[i, j].Piece != null && other.squares[i, j].Piece == null) return false;
                    if (squares[i, j].Piece != null && other.squares[i, j].Piece != null && squares[i, j].Piece.PieceName != other.squares[i, j].Piece.PieceName) return false;
                }
            }
            return true;
        }
        /*public override void SerializeBoard(string fileName)
        {
            string jsonString = JsonSerializer.Serialize<object>(this);
            File.WriteAllText(fileName, jsonString);
            Console.WriteLine(jsonString);
        }*/
        public override ChessBoardState DeserializeBoard(string fileName)
        {
            ChessBoardState boardState = new ChessBoardState();
            for (int i = 0; i < 8; i++)
            {

                for (int j = 0; j < 8; j++)
                {
                    boardState.squares[i, j] = new Square();
                    boardState.squares[i, j].X = i;
                    boardState.squares[i, j].Y = j;
                }
            }


            //var valami= JsonConvert.DeserializeObject<object>(File.ReadAllText(fileName));
            var valami = JObject.Parse(File.ReadAllText(fileName));


            foreach(var item in valami["WhitePieces"])
            {
                Pawn p = new Pawn();
                p.X = (int)item["X"];
                p.Y = (int)item["Y"];
                p.IsWhite = (bool)item["IsWhite"];
                p.HasMoved = (bool)item["HasMoved"];
                p.Value=(int)item["Value"];
                p.PieceName = (PieceName)(int)item["PieceName"];
                if (p.PieceName == PieceName.PAWN)
                {
                    p.EnPassantable = (bool)item["EnPassantable"];
                }
                switch (p.PieceName)
                {
                    case PieceName.PAWN:
                        boardState.WhitePieces.Add(p);
                        boardState.squares[p.X, p.Y].Piece = p;
                        break;
                    case PieceName.KNIGHT:
                        Knight knight = new Knight();
                        knight = (Knight)copyFromPawn(p, knight);
                        boardState.WhitePieces.Add(knight);
                        boardState.squares[knight.X, knight.Y].Piece = knight;
                        break;
                    case PieceName.BISHOP:
                        Bishop bishop = new Bishop();
                        bishop = (Bishop)copyFromPawn(p, bishop);
                        boardState.WhitePieces.Add(bishop);
                        boardState.squares[bishop.X, bishop.Y].Piece = bishop;
                        break;
                    case PieceName.QUEEN:
                        Queen queen=new Queen();
                        queen = (Queen)copyFromPawn(p, queen);
                        boardState.WhitePieces.Add(queen);
                        boardState.squares[queen.X, queen.Y].Piece = queen;
                        break;
                    case PieceName.ROOK:
                        Rook rook = new Rook();
                        rook = (Rook)copyFromPawn(p, rook);
                        boardState.WhitePieces.Add(rook);
                        boardState.squares[rook.X, rook.Y].Piece = rook;
                        break;
                    case PieceName.KING:
                        King king = new King();
                        king = (King)copyFromPawn(p, king);
                        boardState.WhitePieces.Add(king);
                        boardState.WhiteKing = king;
                        boardState.squares[king.X, king.Y].Piece = king;
                        break;

                }
                
            }
            foreach (var item in valami["BlackPieces"])
            {
                Pawn p = new Pawn();
                p.X = (int)item["X"];
                p.Y = (int)item["Y"];
                p.IsWhite = (bool)item["IsWhite"];
                p.HasMoved = (bool)item["HasMoved"];
                p.Value = (int)item["Value"];
                p.PieceName = (PieceName)(int)item["PieceName"];
                if (p.PieceName == PieceName.PAWN)
                {
                    p.EnPassantable = (bool)item["EnPassantable"];
                }
                switch (p.PieceName)
                {
                    case PieceName.PAWN:
                        boardState.BlackPieces.Add(p);
                        boardState.squares[p.X, p.Y].Piece = p;
                        break;
                    case PieceName.KNIGHT:
                        Knight knight = new Knight();
                        knight = (Knight)copyFromPawn(p, knight);
                        boardState.BlackPieces.Add(knight);
                        boardState.squares[knight.X, knight.Y].Piece = knight;
                        break;
                    case PieceName.BISHOP:
                        Bishop bishop = new Bishop();
                        bishop = (Bishop)copyFromPawn(p, bishop);
                        boardState.BlackPieces.Add(bishop);
                        boardState.squares[bishop.X, bishop.Y].Piece = bishop;
                        break;
                    case PieceName.QUEEN:
                        Queen queen = new Queen();
                        queen = (Queen)copyFromPawn(p, queen);
                        boardState.BlackPieces.Add(queen);
                        boardState.squares[queen.X, queen.Y].Piece = queen;
                        break;
                    case PieceName.ROOK:
                        Rook rook = new Rook();
                        rook = (Rook)copyFromPawn(p, rook);
                        boardState.BlackPieces.Add(rook);
                        boardState.squares[rook.X, rook.Y].Piece = rook;
                        break;
                    case PieceName.KING:
                        King king = new King();
                        king = (King)copyFromPawn(p, king);
                        boardState.BlackPieces.Add(king);
                        boardState.BlackKing = king;
                        boardState.squares[king.X, king.Y].Piece = king;
                        break;

                }               
            }
            
            foreach (var item in valami["moves"])
            {
                ChessMove move = new ChessMove();
                move.InitialX = (int)item["InitialX"];
                move.InitialY = (int)item["InitialY"];
                move.TargetX = (int)item["TargetX"];
                move.TargetY = (int)item["TargetY"];
                move.Piece = (PieceName)(int)item["Piece"];
                move.PromoteTo = (PieceName)(int)item["PromoteTo"];
                move.isCheck = (bool)item["isCheck"];
                move.isCapture = (bool)item["isCapture"];
                boardState.moves.Add(move);
            }
            
            boardState.turnOf = (Color)(int)valami["turnOf"];
            boardState.FiftyMoveRule = (int)valami["FiftyMoveRule"];

            return boardState;
        }

        private Piece copyFromPawn(Pawn pawn, Piece piece)
        {
            piece.X=pawn.X;
            piece.Y=pawn.Y;
            piece.IsWhite=pawn.IsWhite;
            piece.PieceName=pawn.PieceName;
            piece.Value=pawn.Value;           
            piece.HasMoved=pawn.HasMoved;
            return piece;           
        }

        public override List<ChessMove> GetMoves()
        {
            return moves;
        }
    }
}
