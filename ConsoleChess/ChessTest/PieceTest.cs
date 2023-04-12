using ConsoleChess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChessTest
{
    public class PieceTest
    {

        [Fact]
        public void WrongStartPoint()
        {
            Board board = new Board();
            bool result = board.Move(2, 3, 3, 3);
            Assert.False(result);
        }
        [Fact]
        public void CannotStepIntoOwnPiece()
        {
            Board board = new Board();
            bool result = board.Move(0, 2, 1, 3);
            Assert.False(result);
        }
        [Fact]
        public void PawnGoodMove()
        {
            Board board = new Board();
            bool result = board.Move(1, 3, 2, 3);
            Assert.True(result);
        }

        [Fact]
        public void PawnBadMove()
        {
            Board board = new Board();
            bool result = board.Move(1, 3, 2, 4);
            Assert.False(result);
        }
        [Fact]
        public void KnightGoodMove()
        {
            Board board = new Board();
            bool result = board.Move(0, 1, 2, 0);
            Assert.True(result);
        }
        [Fact]
        public void KnightBadMove()
        {
            Board board = new Board();
            bool result = board.Move(0, 1, 2, 4);
            Assert.False(result);
        }
        [Fact]
        public void BishopGoodMove()
        {
            Board board = new Board();
            board.Move(1, 3, 2, 3);
            board.Move(6, 3, 5, 3);
            bool result = board.Move(0, 2, 3, 5);
            Assert.True(result);
        }
        [Fact]
        public void BishopBadMove()
        {
            Board board = new Board();
            board.Move(1, 2, 2, 2);
            board.Move(6, 3, 5, 3);
            bool result = board.Move(0, 2, 4, 2);
            Assert.False(result);
        }
        [Fact]
        public void RookGoodMove()
        {
            Board board = new Board();
            board.Move(1, 0, 3,0);
            board.Move(6, 3, 5, 3);
            bool result = board.Move(0, 0, 2, 0);
            Assert.True(result);
        }
        [Fact]
        public void RookBadMove()
        {
            Board board = new Board();
            board.Move(1, 1, 2, 1);
            board.Move(6, 3, 5, 3);
            bool result = board.Move(0, 0, 1, 1);
            Assert.False(result);
        }
        [Fact]
        public void QueenGoodMoveLikeRook()
        {
            Board board = new Board();
            board.Move(1, 3, 3, 3);
            board.Move(6, 3, 5, 3);
            bool result = board.Move(0, 3, 2, 3);
            Assert.True(result);
        }
        [Fact]
        public void QueenGoodMoveLikeBishop()
        {
            Board board = new Board();
            board.Move(1, 4, 3, 4);
            board.Move(6, 3, 5, 3);
            bool result = board.Move(0, 3, 2, 5);
            Assert.True(result);
        }
        [Fact]
        public void QueenBadMove()
        {
            Board board = new Board();
            board.Move(1, 4, 3, 4);
            board.Move(6, 3, 5, 3);
            board.Move(1, 3, 3, 3);
            board.Move(6, 4, 5, 4);
            bool result = board.Move(0, 3, 2, 4);
            Assert.False(result);
        }
        [Fact]
        public void KingGoodMove()
        {
            Board board = new Board();
            board.Move(1, 4, 3, 4);
            board.Move(6, 3, 5, 3);
            bool result = board.Move(0, 4, 1, 4);
            Assert.True(result);
        }
        [Fact]
        public void KingBadMove()
        {
            Board board = new Board();
            board.Move(1, 4, 3, 4);
            board.Move(6, 3, 5, 3);
            bool result = board.Move(0, 4, 2, 4);
            Assert.False(result);
        }
        [Fact]
        public void Captures()
        {
            Board board = new Board();
            board.Move(0, 1, 2, 2);
            board.Move(6, 3, 4, 3);
            bool result = board.Move(2, 2, 4, 3);
            Assert.True(result);
        }
        [Fact]
        public void PawnCaptures()
        {
            Board board = new Board();
            board.Move(1, 4, 3, 4);
            board.Move(6, 3, 4, 3);
            bool result=board.Move(3, 4, 4, 3);
            Assert.True(result);
        }
    }
}
