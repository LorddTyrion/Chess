using ConsoleChess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess
{
    public enum Color
    {
        WHITE=0, BLACK=1, DRAW=2, NONE=3
    }
    public class BoardState
    {
        public List<Piece> WhitePieces = new List<Piece>();
        public List<Piece> BlackPieces = new List<Piece>();
        public King WhiteKing, BlackKing;
        public Square[,] squares = new Square[8, 8];
        public List<Move> moves = new List<Move>();
        public Color turnOf;
        public int FiftyMoveRule = 0;
        public BoardState(BoardState old)
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

            moves = new List<Move>();
            for (int i = 0; i < old.moves.Count; i++)
            {
                moves.Add(old.moves[i]);
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Square s= new Square(old.squares[i, j]);
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
        public BoardState() 
        {
            WhitePieces = new List<Piece>();
            BlackPieces = new List<Piece>();
            
            squares = new Square[8, 8];
            moves = new List<Move>();
        }
        public bool PositionEquals(BoardState other)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (squares[i, j].Piece == null && other.squares[i, j].Piece != null) return false;
                    if (squares[i, j].Piece != null && other.squares[i, j].Piece == null) return false;
                    if(squares[i, j].Piece != null && other.squares[i,j].Piece!=null && squares[i, j].Piece.PieceName!=other.squares[i, j].Piece.PieceName) return false;
                }
            }
            return true;
        }
       
    }
    public class Board
    {
        //public List<Piece> WhitePieces = new List<Piece>();
        //public List<Piece> BlackPieces = new List<Piece>();
        //public King WhiteKing, BlackKing;
        //public Square[,] squares = new Square[8, 8];
        //public List<string> moves = new List<string>();
        //public Color turnOf;
        public BoardState boardState=new BoardState();
        public List<BoardState> States=new List<BoardState>();
        public Board()
        {
            boardState.turnOf=Color.WHITE;
            for (int i = 0; i < 8; i++)
            {
                
                for (int j = 0; j < 8; j++)
                {
                    boardState.squares[i,j]=new Square();
                    boardState.squares[i, j].X = i;
                    boardState.squares[i, j].Y = j;
                }
            }
            for (int i = 0; i < 8; i+=7)
            {
                boardState.squares[i, 0].Piece = new Rook();
                boardState.squares[i, 1].Piece = new Knight();
                boardState.squares[i, 2].Piece = new Bishop();
                boardState.squares[i, 3].Piece = new Queen();
                boardState.squares[i, 4].Piece = new King();
                boardState.squares[i, 5].Piece = new Bishop();
                boardState.squares[i, 6].Piece = new Knight();
                boardState.squares[i, 7].Piece = new Rook();
            }
            for (int i = 0; i < 8; i++)
            {
                boardState.squares[1, i].Piece = new Pawn();
                boardState.squares[6, i].Piece = new Pawn(); 
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i == 0 || i == 1)
                    {
                        boardState.squares[i, j].Piece.IsWhite = true;
                        boardState.WhitePieces.Add(boardState.squares[i, j].Piece);
                    }
                    else if(boardState.squares[i,j].Piece!=null)
                    {
                        boardState.squares[i, j].Piece.IsWhite = false;
                        boardState.BlackPieces.Add(boardState.squares[i, j].Piece);
                    }
                    if (boardState.squares[i, j].Piece != null)
                    {
                        boardState.squares[i, j].Piece.X = i;
                        boardState.squares[i, j].Piece.Y = j;
                    }
                }
            }
            boardState.WhiteKing = (King)boardState.squares[0, 4].Piece;
            boardState.BlackKing = (King)boardState.squares[7, 4].Piece;
            
        }
       
        public bool IsAttacked(int x, int y, bool isWhite, BoardState boardState)
        {
            if (isWhite)
            {
                foreach (Piece p in boardState.BlackPieces)
                {
                    if (p.IsAttacked(x, y, boardState.squares))
                    {
                        return true;
                    }
                }
            }
            else
            {
                foreach (Piece p in boardState.WhitePieces)
                {
                    if (p.IsAttacked(x, y, boardState.squares))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
        public List<Move> getPossibleMoves(int x, int y)
        {
            List<Move> possibleMoves = new List<Move>();
            
            BoardState newState = new BoardState(boardState);
            if (newState.squares[x, y].Piece.IsWhite && boardState.turnOf == Color.BLACK) return possibleMoves;
            if (!newState.squares[x, y].Piece.IsWhite && boardState.turnOf == Color.WHITE) return possibleMoves;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bool isEmpty = newState.squares[i, j].Piece == null;
                    PieceName pieceName = PieceName.PAWN;
                    if (newState.squares[x, y].Piece != null) pieceName = newState.squares[x, y].Piece.PieceName;
                    bool valid=TryMove(newState.squares[x, y], newState.squares[i, j], newState, PieceName.QUEEN);
                    if (!valid) 
                    {
                        newState = new BoardState(boardState);
                        continue; 
                    }
                    Move possibleMove = new Move();
                    possibleMove.isCheck = isCheck(newState);
                    possibleMove.isCapture = !isEmpty;
                    possibleMove.InitialX = x;
                    possibleMove.InitialY = y;
                    possibleMove.TargetX = i;
                    possibleMove.TargetY = j;
                    possibleMove.Piece = pieceName;
                    possibleMove.PromoteTo = PieceName.PAWN;
                    if(canPromote(boardState.squares[x,y].Piece, boardState.squares[i, j], boardState))
                    {
                        Move bishopMove=new Move(possibleMove); bishopMove.PromoteTo = PieceName.BISHOP; possibleMoves.Add(bishopMove);
                        Move queenMove = new Move(possibleMove); bishopMove.PromoteTo = PieceName.QUEEN; possibleMoves.Add(queenMove);
                        Move knightMove = new Move(possibleMove); bishopMove.PromoteTo = PieceName.KNIGHT; possibleMoves.Add(knightMove);
                        Move rookMove = new Move(possibleMove); bishopMove.PromoteTo = PieceName.ROOK; possibleMoves.Add(rookMove);

                    }
                    else { possibleMoves.Add(possibleMove); }
                    newState = new BoardState(boardState);
                    
                }
            }
            return possibleMoves;
        }
        public bool Move(int initialX, int initialY, int targetX, int targetY, PieceName promoteTo=PieceName.QUEEN)
        {
            Square initialSquare = boardState.squares[initialX, initialY];
            Square targetSquare=boardState.squares[targetX, targetY];
            if (boardState.squares[initialX, initialY].Piece != null)
            {
                if (boardState.squares[initialX, initialY].Piece.IsWhite && boardState.turnOf == Color.BLACK) return false;
                if (!boardState.squares[initialX, initialY].Piece.IsWhite && boardState.turnOf == Color.WHITE) return false;
            }
            BoardState newState=new BoardState(boardState);
            if(TryMove(initialSquare, targetSquare, newState, promoteTo))
            {
                boardState=newState;
                States.Add(boardState);
                return true;
            }
            return false;
        }
        public Color CheckEndGame()
        {
            int sum = 0;
            if (boardState.turnOf == Color.WHITE)
            {
                foreach (Piece piece in boardState.WhitePieces)
                {
                    int moves = getPossibleMoves(piece.X, piece.Y).Count;
                    sum += moves;
                    if (moves != 0) break;
                }
                if (sum == 0 && isCheck(boardState)) return Color.BLACK;
                if (sum == 0 && !isCheck(boardState)) return Color.DRAW;
            }
            else if (boardState.turnOf == Color.BLACK)
            {
                foreach (Piece piece in boardState.BlackPieces)
                {
                    int moves = getPossibleMoves(piece.X, piece.Y).Count;
                    sum += moves;
                    if (moves != 0) break;
                }
                if (sum == 0 && isCheck(boardState)) return Color.WHITE;
                if (sum == 0 && !isCheck(boardState)) return Color.DRAW;
            }
            if (checkSufficientMaterial() == Color.NONE) return Color.DRAW;
            int repetition = 0;
            for (int i = 0; i < States.Count; i++)
            {
                if (boardState.PositionEquals(States[i])) repetition++;
            }
            if (repetition >= 3) return Color.DRAW;
            if(boardState.FiftyMoveRule>=50) return Color.DRAW;
            return Color.NONE;
        }
        private bool canPromote (Piece piece, Square target, BoardState boardState)
        {
            if (piece.getPieceName()!=PieceName.PAWN) return false;
            if (piece.IsWhite && piece.X != 6) return false;
            if (!piece.IsWhite && piece.X != 1) return false;
            if (piece.IsSteppable(target.X, target.Y, boardState.squares)) return true;
            if(piece.IsAttacked(target.X, target.Y, boardState.squares)) return true;
            return false;

        }
        private bool isBoardValid(BoardState boardState)
        {
            if (boardState.turnOf == Color.WHITE)
            {
                if (IsAttacked(boardState.BlackKing.X, boardState.BlackKing.Y, false, boardState)) return false;
            }
            else
            {
                if(IsAttacked(boardState.WhiteKing.X, boardState.WhiteKing.Y, true, boardState)) return false;
            }
            return true;
        }
        private bool isCheck(BoardState boardState)
        {
            if (boardState.turnOf == Color.BLACK)
            {
                if (IsAttacked(boardState.BlackKing.X, boardState.BlackKing.Y, false, boardState)) return true;
            }
            else
            {
                if (IsAttacked(boardState.WhiteKing.X, boardState.WhiteKing.Y, true, boardState)) return true;
            }
            return false;
        }
        private void changeTurn(BoardState boardState)
        {
            if(boardState.turnOf == Color.BLACK) boardState.turnOf = Color.WHITE;
            else boardState.turnOf = Color.BLACK;
        }
        public bool Promote(Square initialSquare, Square targetSquare, PieceName piece, BoardState boardState) 
        {
            if (!canPromote(initialSquare.Piece, boardState.squares[targetSquare.X, targetSquare.Y], boardState)) { return false; }
            Piece temp = initialSquare.Piece;
            initialSquare.Piece = null;
            Piece tempTarget = null;
            bool isCapture = false;
            if (targetSquare.Piece != null) { tempTarget = targetSquare.Piece; isCapture = true; }
            targetSquare.Piece = null;
            if (boardState.turnOf == Color.WHITE && temp != null)
            {
                boardState.FiftyMoveRule = 0;
                boardState.BlackPieces.Remove(temp);
            }
            else if (boardState.turnOf == Color.BLACK && temp != null)
            {
                boardState.FiftyMoveRule = 0;
                boardState.WhitePieces.Remove(temp);
            }
            if (boardState.turnOf == Color.WHITE)
            {
                boardState.WhitePieces.Remove(temp);
                addToList(boardState.WhitePieces, piece, targetSquare);
                targetSquare.Piece = boardState.WhitePieces[boardState.WhitePieces.Count - 1];
                boardState.WhitePieces[boardState.WhitePieces.Count-1].IsWhite = true;
            }
            else
            {
                boardState.BlackPieces.Remove(temp);
                addToList(boardState.BlackPieces, piece, targetSquare);
                targetSquare.Piece = boardState.BlackPieces[boardState.BlackPieces.Count - 1];
                boardState.WhitePieces[boardState.WhitePieces.Count - 1].IsWhite = false;
            }
            changeTurn(boardState);
            bool isChecked=isCheck(boardState);
            if (isBoardValid(boardState))
            {
                Move possibleMove = new Move();
                possibleMove.isCheck = isChecked;
                possibleMove.isCapture = isCapture;
                possibleMove.InitialX = initialSquare.X;
                possibleMove.InitialY = initialSquare.Y;
                possibleMove.TargetX = targetSquare.X;
                possibleMove.TargetY = targetSquare.Y;
                possibleMove.Piece = PieceName.PAWN;
                possibleMove.PromoteTo = piece;
                boardState.moves.Add(possibleMove);
                return true;
            }
            return false;

        }
        private void addToList(List<Piece> list, PieceName piece, Square square)
        {
            switch (piece)
            {
                case PieceName.BISHOP:
                    list.Add(new Bishop());
                    break;
                case PieceName.KNIGHT:
                    list.Add(new Knight());
                    break;
                case PieceName.ROOK:
                    list.Add(new Rook());
                    break;
                case PieceName.QUEEN:
                    list.Add(new Queen());
                    break;
                default:
                    break;
            }
            list[list.Count - 1].X = square.X;
            list[list.Count - 1].Y = square.Y;
        }
        public bool  Castle(Square initialSquare, Square targetSquare, BoardState boardState) 
        {
            if (initialSquare.Piece.getPieceName() != PieceName.KING) { return false; }
            if (!((King)initialSquare.Piece).canCastle(targetSquare.X, targetSquare.Y, this)) { return false; }
            King temp = (King)initialSquare.Piece;
            bool isLong = initialSquare.Y > targetSquare.Y;
            if (isLong)
            {
                boardState.squares[targetSquare.X, targetSquare.Y].Piece = initialSquare.Piece;
                initialSquare.Piece = null;
                boardState.squares[targetSquare.X, targetSquare.Y + 1].Piece = boardState.squares[targetSquare.X, 0].Piece;
                boardState.squares[targetSquare.X, 0].Piece = null;
            }
            else
            {
                boardState.squares[targetSquare.X, targetSquare.Y].Piece = initialSquare.Piece;
                initialSquare.Piece = null;
                boardState.squares[targetSquare.X, targetSquare.Y - 1].Piece = boardState.squares[targetSquare.X, 7].Piece;
                boardState.squares[targetSquare.X, 7].Piece = null;
            }
            bool isChecked = isCheck(boardState);
            Move possibleMove = new Move();
            possibleMove.isCheck = isChecked;
            possibleMove.isCapture = false;
            possibleMove.InitialX = initialSquare.X;
            possibleMove.InitialY = initialSquare.Y;
            possibleMove.TargetX = targetSquare.X;
            possibleMove.TargetY = targetSquare.Y;
            possibleMove.Piece = PieceName.KING;
            possibleMove.PromoteTo = PieceName.PAWN;
            boardState.moves.Add(possibleMove);
            return true;
        }
        public bool TryMove(Square initialSquare, Square targetSquare, BoardState boardState, PieceName promoteTo)
        {
            if (initialSquare.Piece == null) return false;
            bool hasMoved = initialSquare.Piece.HasMoved;
            if (initialSquare.Piece.PieceName == PieceName.PAWN) boardState.FiftyMoveRule = 0;
            else boardState.FiftyMoveRule++;
            bool moveValid=false;
            Square emulatedInitialSquare=new Square(), emulatedTargetSquare=new Square();
            for(int i=0; i<8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if(boardState.squares[i,j].X==initialSquare.X && boardState.squares[i, j].Y == initialSquare.Y)
                    {
                        emulatedInitialSquare=boardState.squares[i,j];
                    }
                    else if(boardState.squares[i, j].X == targetSquare.X && boardState.squares[i, j].Y == targetSquare.Y)
                    {
                        emulatedTargetSquare=boardState.squares[i,j];
                    }
                }
            }
            //get possible en passant moves, and change them
            List<Pawn> hadEnPassant = new List<Pawn>();
            if (boardState.turnOf == Color.WHITE)
            {
                hadEnPassant=takeAwayEnPassant(boardState.WhitePieces);
            }
            else
            {
                hadEnPassant= takeAwayEnPassant(boardState.BlackPieces);
            }
            
            if(Castle(emulatedInitialSquare, emulatedTargetSquare, boardState))
            {
                changeTurn(boardState);
                return true;
            }
            bool prom = canPromote(emulatedInitialSquare.Piece, emulatedTargetSquare, boardState);
            if (canPromote(emulatedInitialSquare.Piece, emulatedTargetSquare, boardState))
            {
                
                switch (promoteTo)
                {
                    case PieceName.BISHOP:
                        moveValid = Promote(emulatedInitialSquare, emulatedTargetSquare, PieceName.BISHOP, boardState);
                        break;
                    case PieceName.KNIGHT:
                        moveValid = Promote(emulatedInitialSquare, emulatedTargetSquare, PieceName.KNIGHT, boardState);
                        break;
                    case PieceName.ROOK:
                        moveValid = Promote(emulatedInitialSquare, emulatedTargetSquare, PieceName.ROOK, boardState);
                        break;
                    case PieceName.QUEEN:
                        moveValid = Promote(emulatedInitialSquare, emulatedTargetSquare, PieceName.QUEEN, boardState);
                        break;
                    default:
                        break;

                }
                return moveValid;
            }
            Piece temp=null;
            bool wasEP = false;
            bool isCapture = false;
            PieceName pieceName = emulatedInitialSquare.Piece.PieceName;
            if(emulatedTargetSquare.Piece != null) { temp= emulatedTargetSquare.Piece; isCapture = true; }
            if(emulatedInitialSquare.Piece.getPieceName()==PieceName.PAWN && emulatedTargetSquare.Piece==null 
                && boardState.squares[emulatedInitialSquare.X, emulatedTargetSquare.Y].Piece!=null
                && boardState.squares[emulatedInitialSquare.X, emulatedTargetSquare.Y].Piece.getPieceName()==PieceName.PAWN 
                && ((Pawn)boardState.squares[emulatedInitialSquare.X, emulatedTargetSquare.Y].Piece).EnPassantable)                
            { 
                temp = boardState.squares[emulatedInitialSquare.X, emulatedTargetSquare.Y].Piece;
                boardState.FiftyMoveRule = 0;
                wasEP = true;
            }
            //emulatedTargetSquare.Piece = null;
            if (boardState.turnOf == Color.WHITE && temp!=null)
            {
                boardState.FiftyMoveRule = 0;
                boardState.BlackPieces.Remove(temp);
            }
            else if(boardState.turnOf == Color.BLACK && temp != null)
            {
                boardState.FiftyMoveRule = 0;
                boardState.WhitePieces.Remove(temp);
            }
            if (emulatedInitialSquare.Piece.Move(emulatedTargetSquare.X, emulatedTargetSquare.Y, boardState.squares))
            {
                if(wasEP)boardState.squares[emulatedInitialSquare.X, emulatedTargetSquare.Y].Piece = null;
                changeTurn(boardState);
                bool isChecked = isCheck(boardState);
                if (isBoardValid(boardState))
                {
                    Move possibleMove = new Move();
                    possibleMove.isCheck = isChecked;
                    possibleMove.isCapture = isCapture || wasEP;
                    possibleMove.InitialX = initialSquare.X;
                    possibleMove.InitialY = initialSquare.Y;
                    possibleMove.TargetX = targetSquare.X;
                    possibleMove.TargetY = targetSquare.Y;
                    possibleMove.Piece = pieceName;
                    possibleMove.PromoteTo = PieceName.PAWN;
                    boardState.moves.Add(possibleMove);
                    moveValid = true;
                }
            }
            return moveValid;

        }
        private List<Pawn> takeAwayEnPassant(List<Piece> list)
        {
            List<Pawn> result = new List<Pawn>();
            foreach (Piece piece in list)
            {
                if (piece.getPieceName() == PieceName.PAWN && ((Pawn)piece).EnPassantable)
                {
                    result.Add((Pawn)piece);
                    ((Pawn)piece).EnPassantable = false;
                }
            }
            return result;

        }
        private Color checkSufficientMaterial()
        {
            if (!checkSufficientForOneColor(Color.WHITE) && !checkSufficientForOneColor(Color.BLACK)) return Color.NONE;
            if(!checkSufficientForOneColor(Color.WHITE)) return Color.BLACK;
            if (!checkSufficientForOneColor(Color.BLACK)) return Color.WHITE;
            return Color.DRAW;
        }
        private bool checkSufficientForOneColor(Color color)
        {
            int sum = 0;
            if(color == Color.WHITE)
            {
                foreach(Piece piece in boardState.WhitePieces)
                {
                    if (piece.getPieceName() == PieceName.PAWN) return true;
                    if(piece.getPieceName() == PieceName.ROOK) return true;
                    if(piece.getPieceName()==PieceName.QUEEN)  return true;
                    if (piece.getPieceName() == PieceName.KNIGHT) sum++;
                    else if(piece.getPieceName()==PieceName.BISHOP) sum++;
                }
                if (sum>=2) return true;
                return false;
            }
            else if(color == Color.BLACK)
            {
                foreach (Piece piece in boardState.BlackPieces)
                {
                    if (piece.getPieceName() == PieceName.PAWN) return true;
                    if (piece.getPieceName() == PieceName.ROOK) return true;
                    if (piece.getPieceName() == PieceName.QUEEN) return true;
                    if (piece.getPieceName() == PieceName.KNIGHT) sum++;
                    else if (piece.getPieceName() == PieceName.BISHOP) sum++;
                }
                if (sum >= 2) return true;
                return false;
            }
            return false;
        }
        public int GetSumValue(Color color)
        {
            int sum = 0;
            if(color == Color.BLACK)
            {
                foreach(Piece piece in boardState.BlackPieces)
                {
                    sum += piece.Value;
                }
            }
            else if (color == Color.WHITE)
            {
                foreach (Piece piece in boardState.WhitePieces)
                {
                    sum += piece.Value;
                }
            }
            return sum;
        }
    

    }
}
