using ConsoleChess;
using ConsoleChess.Pieces;
using System;
using System.IO;
using Xunit;

namespace ChessTest
{
    public class SpecialTests
    {
        [Fact]
        public void ThereAre16WhitePieces()
        {
            Board board = new Board();
            int sum = board.boardState.WhitePieces.Count;
            Assert.Equal(16, sum);

        }
        [Fact]
        public void PawnCanMoveTwoFirst()
        {
            Board board = new Board();
            bool result=board.Move(1,3,3,3);
            Assert.True(result);
        }
        [Fact]
        public void PawnCannotMoveTwoLater()
        {
            Board board = new Board();
            bool result = board.Move(1, 3, 2, 3);
            result=board.Move(2, 3, 4, 3);
            Assert.False(result);
        }
        [Fact]
        public void CanAnPassant()
        {
            Board board = new Board();
            bool result = board.Move(1, 3, 3, 3);
            result = board.Move(6, 1, 4, 1);
            result = board.Move(3, 3, 4, 3);
            result = board.Move(6, 4, 4, 4);
            result = board.Move(4, 3, 5, 4);
            Assert.True(result);
        }
        [Fact]
        public void CannotAnPassantLater()
        {
            Board board = new Board();
            bool result = board.Move(1, 3, 3, 3);
            result = board.Move(6, 1, 4, 1);
            result = board.Move(3, 3, 4, 3);
            result = board.Move(6, 4, 4, 4);
            result = board.Move(1, 2, 2, 2);
            result = board.Move(6, 0, 5, 0);
            result = board.Move(4, 3, 5, 4);
            Assert.False(result);
        }
        [Fact]
        public void Castles()
        {
            Board board=new Board();
            clearBoard(board);
            Bishop b=new Bishop();
            b.IsWhite = false; b.X = 5; b.Y = 0;
            board.boardState.squares[5, 0].Piece = b;
            board.boardState.BlackPieces.Add(b);

            King k1 = new King();
            k1.IsWhite = false; k1.X = 7; k1.Y = 4;
            board.boardState.squares[7, 4].Piece = k1;
            board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

            King k2 = new King();
            k2.IsWhite = true; k2.X = 0; k2.Y = 4;
            board.boardState.squares[0, 4].Piece = k2;
            board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

            Pawn p = new Pawn();
            p.IsWhite = true; p.X = 1; p.Y = 4;
            board.boardState.squares[1,4].Piece = p;
            board.boardState.WhitePieces.Add(p);

            Rook r = new Rook();
            r.IsWhite = true; r.X = 0; r.Y = 7;
            board.boardState.squares[0, 7].Piece = r;
            board.boardState.WhitePieces.Add(r);

            bool result = board.Move(0, 4, 0, 6);
            Assert.True(result);
        }
        [Fact]
        public void LongCastles()
        {
            Board board = new Board();
            clearBoard(board);
            King k1 = new King();
            k1.IsWhite = false; k1.X = 7; k1.Y = 4;
            board.boardState.squares[7, 4].Piece = k1;
            board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

            King k2 = new King();
            k2.IsWhite = true; k2.X = 0; k2.Y = 4;
            board.boardState.squares[0, 4].Piece = k2;
            board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

            Rook r = new Rook();
            r.IsWhite = true; r.X = 0; r.Y = 0;
            board.boardState.squares[0, 0].Piece = r;
            board.boardState.WhitePieces.Add(r);

            bool result = board.Move(0, 4, 0, 2);
            Assert.True(result);
        }
        [Fact]
        public void CannotCastleInCheck()
        {
            Board board = new Board();
            clearBoard(board);
            Bishop b = new Bishop();
            b.IsWhite = false; b.X = 4; b.Y = 0;
            board.boardState.squares[4, 0].Piece = b;
            board.boardState.BlackPieces.Add(b);

            King k1 = new King();
            k1.IsWhite = false; k1.X = 7; k1.Y = 4;
            board.boardState.squares[7, 4].Piece = k1;
            board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

            King k2 = new King();
            k2.IsWhite = true; k2.X = 0; k2.Y = 4;
            board.boardState.squares[0, 4].Piece = k2;
            board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

            Pawn p = new Pawn();
            p.IsWhite = true; p.X = 1; p.Y = 4;
            board.boardState.squares[1, 4].Piece = p;
            board.boardState.WhitePieces.Add(p);

            Rook r = new Rook();
            r.IsWhite = true; r.X = 0; r.Y = 7;
            board.boardState.squares[0, 7].Piece = r;
            board.boardState.WhitePieces.Add(r);

            bool result = board.Move(0, 4, 0, 6);
            Assert.False(result);
        }
        [Fact]
        public void CannotCastleThroughPiece()
        {
            Board board = new Board();
            clearBoard(board);
            Bishop b = new Bishop();
            b.IsWhite = false; b.X = 5; b.Y = 0;
            board.boardState.squares[5, 0].Piece = b;
            board.boardState.BlackPieces.Add(b);

            Pawn p = new Pawn();
            p.IsWhite = true; p.X = 1; p.Y = 4;
            board.boardState.squares[1, 4].Piece = p;
            board.boardState.WhitePieces.Add(p);

            King k1 = new King();
            k1.IsWhite = false; k1.X = 7; k1.Y = 4;
            board.boardState.squares[7, 4].Piece = k1;
            board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

            King k2 = new King();
            k2.IsWhite = true; k2.X = 0; k2.Y = 4;
            board.boardState.squares[0, 4].Piece = k2;
            board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

            Rook r = new Rook();
            r.IsWhite = true; r.X = 0; r.Y = 7;
            board.boardState.squares[0, 7].Piece = r;
            board.boardState.WhitePieces.Add(r);

            Knight k = new Knight();
            k.IsWhite = true; k.X=0; k.Y = 5;
            board.boardState.squares[0, 5].Piece = k;
            board.boardState.WhitePieces.Add(k);

            bool result = board.Move(0, 4, 0, 6);
            Assert.False(result);
        }

        [Fact]
        public void CannotCastleThroughCheck()
        {
            Board board = new Board();
            clearBoard(board);
            Bishop b = new Bishop();
            b.IsWhite = false; b.X = 5; b.Y = 0;
            board.boardState.squares[5, 0].Piece = b;
            board.boardState.BlackPieces.Add(b);

            King k1 = new King();
            k1.IsWhite = false; k1.X = 7; k1.Y = 4;
            board.boardState.squares[7, 4].Piece = k1;
            board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

            King k2 = new King();
            k2.IsWhite = true; k2.X = 0; k2.Y = 4;
            board.boardState.squares[0, 4].Piece = k2;
            board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

            Rook r = new Rook();
            r.IsWhite = true; r.X = 0; r.Y = 7;
            board.boardState.squares[0, 7].Piece = r;
            board.boardState.WhitePieces.Add(r);

            bool result = board.Move(0, 4, 0, 6);
            Assert.False(result);
        }
        [Fact]
        public void CannotCastleIfRookMoved()
        {
            Board board = new Board();
            clearBoard(board);
            King k1 = new King();
            k1.IsWhite = false; k1.X = 7; k1.Y = 4;
            board.boardState.squares[7, 4].Piece = k1;
            board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

            King k2 = new King();
            k2.IsWhite = true; k2.X = 0; k2.Y = 4;
            board.boardState.squares[0, 4].Piece = k2;
            board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

            Rook r = new Rook();
            r.IsWhite = true; r.X = 0; r.Y = 0;
            board.boardState.squares[0, 0].Piece = r;
            board.boardState.WhitePieces.Add(r);
            board.Move(0, 0, 0, 1);
            board.Move(7, 4, 7, 3);
            board.Move(0, 1, 0, 0);
            board.Move(7, 3, 7, 4);
            bool result = board.Move(0, 4, 0, 2);
            Assert.False(result);
        }
        [Fact]
        public void CannotCastleIfKingMoved()
        {
            Board board = new Board();
            clearBoard(board);
            King k1 = new King();
            k1.IsWhite = false; k1.X = 7; k1.Y = 4;
            board.boardState.squares[7, 4].Piece = k1;
            board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

            King k2 = new King();
            k2.IsWhite = true; k2.X = 0; k2.Y = 4;
            board.boardState.squares[0, 4].Piece = k2;
            board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

            Rook r = new Rook();
            r.IsWhite = true; r.X = 0; r.Y = 0;
            board.boardState.squares[0, 0].Piece = r;
            board.boardState.WhitePieces.Add(r);
            board.Move(0, 4, 0, 3);
            board.Move(7, 4, 7, 3);
            board.Move(0, 3, 0, 4);
            board.Move(7, 3, 7, 4);
            bool result = board.Move(0, 4, 0, 2);
            Assert.False(result);
        }
        [Fact]
        public void CannotCastleRookNotInPlace()
        {
            Board board = new Board();
            clearBoard(board);
            King k1 = new King();
            k1.IsWhite = false; k1.X = 7; k1.Y = 4;
            board.boardState.squares[7, 4].Piece = k1;
            board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

            King k2 = new King();
            k2.IsWhite = true; k2.X = 0; k2.Y = 4;
            board.boardState.squares[0, 4].Piece = k2;
            board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

            Rook r = new Rook();
            r.IsWhite = true; r.X = 0; r.Y = 0;
            board.boardState.squares[0, 0].Piece = r;
            board.boardState.WhitePieces.Add(r);
            board.Move(0, 0, 0, 1);
            board.Move(7, 4, 7, 3);
            bool result = board.Move(0, 4, 0, 2);
            Assert.False(result);
        }

        [Fact]
        public void Promote()
        {
            Board board = new Board();
            clearBoard(board);
            King k1 = new King();
            k1.IsWhite = false; k1.X = 7; k1.Y = 4;
            board.boardState.squares[7, 4].Piece = k1;
            board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

            King k2 = new King();
            k2.IsWhite = true; k2.X = 0; k2.Y = 4;
            board.boardState.squares[0, 4].Piece = k2;
            board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

            Pawn p = new Pawn();
            p.IsWhite = true; p.X = 6; p.Y = 2;
            board.boardState.squares[6, 2].Piece = p;
            board.boardState.WhitePieces.Add(p);

            var input = new StringReader("4\n4");
            Console.SetIn(input);
            bool result = board.Move(6, 2, 7, 2);
            Assert.True(result);
        }
        [Fact]
        public void Checkmate()
        {
            Board board = new Board();
            clearBoard(board);
            King k1 = new King();
            k1.IsWhite = false; k1.X = 7; k1.Y = 4;
            board.boardState.squares[7, 4].Piece = k1;
            board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

            King k2 = new King();
            k2.IsWhite = true; k2.X = 5; k2.Y = 4;
            board.boardState.squares[5, 4].Piece = k2;
            board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

            Queen q = new Queen();
            q.IsWhite = true; q.X = 6; q.Y = 4;
            board.boardState.squares[6, 4].Piece = q;
            board.boardState.WhitePieces.Add(q);
            board.boardState.turnOf = Color.BLACK;

            Color color = board.CheckEndGame();
            Assert.Equal(Color.WHITE, color);

        }
        [Fact]
        public void Stalemate()
        {
            Board board = new Board();
            clearBoard(board);
            King k1 = new King();
            k1.IsWhite = false; k1.X = 7; k1.Y = 4;
            board.boardState.squares[7, 4].Piece = k1;
            board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

            King k2 = new King();
            k2.IsWhite = true; k2.X = 5; k2.Y = 4;
            board.boardState.squares[5, 4].Piece = k2;
            board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

            Pawn p = new Pawn();
            p.IsWhite = true; p.X = 6; p.Y = 4;
            board.boardState.squares[6, 4].Piece = p;
            board.boardState.WhitePieces.Add(p);
            board.boardState.turnOf = Color.BLACK;

            Color color = board.CheckEndGame();
            Assert.Equal(Color.DRAW, color);
        }

        [Fact]
        public void InsufficientMaterial()
        {
            Board board = new Board();
            clearBoard(board);
            King k1 = new King();
            k1.IsWhite = false; k1.X = 7; k1.Y = 4;
            board.boardState.squares[7, 4].Piece = k1;
            board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

            King k2 = new King();
            k2.IsWhite = true; k2.X = 5; k2.Y = 4;
            board.boardState.squares[5, 4].Piece = k2;
            board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

            Knight n = new Knight();
            n.IsWhite = true; n.X = 6; n.Y = 4;
            board.boardState.squares[6, 4].Piece = n;
            board.boardState.WhitePieces.Add(n);
            board.boardState.turnOf = Color.BLACK;

            Color color = board.CheckEndGame();
            Assert.Equal(Color.DRAW, color);
        }

        [Fact]
        public void PinnedPiece()
        {
            Board board = new Board();
            clearBoard(board);
            Bishop b = new Bishop();
            b.IsWhite = false; b.X = 5; b.Y = 0;
            board.boardState.squares[5, 0].Piece = b;
            board.boardState.BlackPieces.Add(b);

            King k1 = new King();
            k1.IsWhite = false; k1.X = 7; k1.Y = 4;
            board.boardState.squares[7, 4].Piece = k1;
            board.boardState.BlackPieces.Add(k1); board.boardState.BlackKing = k1;

            King k2 = new King();
            k2.IsWhite = true; k2.X = 0; k2.Y = 5;
            board.boardState.squares[0, 5].Piece = k2;
            board.boardState.WhitePieces.Add(k2); board.boardState.WhiteKing = k2;

            Pawn p = new Pawn();
            p.IsWhite = true; p.X = 1; p.Y = 4;
            board.boardState.squares[1, 4].Piece = p;
            board.boardState.WhitePieces.Add(p);

            Rook r = new Rook();
            r.IsWhite = true; r.X = 0; r.Y = 7;
            board.boardState.squares[0, 7].Piece = r;
            board.boardState.WhitePieces.Add(r);

            bool result = board.Move(1, 4, 2, 4);
            Assert.False(result);
        }




        private void clearBoard(Board board)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board.boardState.squares[i, j].Piece = null;
                }
            }
            board.boardState.WhitePieces.Clear();
            board.boardState.BlackPieces.Clear();
        }
    }
}