﻿using ConsoleChess;
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
            ChessBoard board = new ChessBoard();
            bool result = board.Move(generateMove(2, 3, 3, 3));
            Assert.False(result);
        }
        [Fact]
        public void CannotStepIntoOwnPiece()
        {
            ChessBoard board = new ChessBoard();
            bool result = board.Move(generateMove(0, 2, 1, 3));
            Assert.False(result);
        }
        [Fact]
        public void PawnGoodMove()
        {
            ChessBoard board = new ChessBoard();
            bool result = board.Move(generateMove(1, 3, 2, 3));
            Assert.True(result);
        }

        [Fact]
        public void PawnBadMove()
        {
            ChessBoard board = new ChessBoard();
            bool result = board.Move(generateMove(1, 3, 2, 4));
            Assert.False(result);
        }
        [Fact]
        public void KnightGoodMove()
        {
            ChessBoard board = new ChessBoard();
            bool result = board.Move(generateMove(0, 1, 2, 0));
            Assert.True(result);
        }
        [Fact]
        public void KnightBadMove()
        {
            ChessBoard board = new ChessBoard();
            bool result = board.Move(generateMove(0, 1, 2, 4));
            Assert.False(result);
        }
        [Fact]
        public void BishopGoodMove()
        {
            ChessBoard board = new ChessBoard();
            board.Move(generateMove(1, 3, 2, 3));
            board.Move(generateMove(6, 3, 5, 3));
            bool result = board.Move(generateMove(0, 2, 3, 5));
            Assert.True(result);
        }
        [Fact]
        public void BishopBadMove()
        {
            ChessBoard board = new ChessBoard();
            board.Move(generateMove(1, 2, 2, 2));
            board.Move(generateMove(6, 3, 5, 3));
            bool result = board.Move(generateMove(0, 2, 4, 2));
            Assert.False(result);
        }
        [Fact]
        public void RookGoodMove()
        {
            ChessBoard board = new ChessBoard();
            board.Move(generateMove(1, 0, 3,0));
            board.Move(generateMove(6, 3, 5, 3));
            bool result = board.Move(generateMove(0, 0, 2, 0));
            Assert.True(result);
        }
        [Fact]
        public void RookBadMove()
        {
            ChessBoard board = new ChessBoard();
            board.Move(generateMove(1, 1, 2, 1));
            board.Move(generateMove(6, 3, 5, 3));
            bool result = board.Move(generateMove(0, 0, 1, 1));
            Assert.False(result);
        }
        [Fact]
        public void QueenGoodMoveLikeRook()
        {
            ChessBoard board = new ChessBoard();
            board.Move(generateMove(1, 3, 3, 3));
            board.Move(generateMove(6, 3, 5, 3));
            bool result = board.Move(generateMove(0, 3, 2, 3));
            Assert.True(result);
        }
        [Fact]
        public void QueenGoodMoveLikeBishop()
        {
            ChessBoard board = new ChessBoard();
            board.Move(generateMove(1, 4, 3, 4));
            board.Move(generateMove(6, 3, 5, 3));
            bool result = board.Move(generateMove(0, 3, 2, 5));
            Assert.True(result);
        }
        [Fact]
        public void QueenBadMove()
        {
            ChessBoard board = new ChessBoard();
            board.Move(generateMove(1, 4, 3, 4));
            board.Move(generateMove(6, 3, 5, 3));
            board.Move(generateMove(1, 3, 3, 3));
            board.Move(generateMove(6, 4, 5, 4));
            bool result = board.Move(generateMove(0, 3, 2, 4));
            Assert.False(result);
        }
        [Fact]
        public void KingGoodMove()
        {
            ChessBoard board = new ChessBoard();
            board.Move(generateMove(1, 4, 3, 4));
            board.Move(generateMove(6, 3, 5, 3));
            bool result = board.Move(generateMove(0, 4, 1, 4));
            Assert.True(result);
        }
        [Fact]
        public void KingBadMove()
        {
            ChessBoard board = new ChessBoard();
            board.Move(generateMove(1, 4, 3, 4));
            board.Move(generateMove(6, 3, 5, 3));
            bool result = board.Move(generateMove(0, 4, 2, 4));
            Assert.False(result);
        }
        [Fact]
        public void Captures()
        {
            ChessBoard board = new ChessBoard();
            board.Move(generateMove(0, 1, 2, 2));
            board.Move(generateMove(6, 3, 4, 3));
            bool result = board.Move(generateMove(2, 2, 4, 3));
            Assert.True(result);
        }
        [Fact]
        public void PawnCaptures()
        {
            ChessBoard board = new ChessBoard();
            board.Move(generateMove(1, 4, 3, 4));
            board.Move(generateMove(6, 3, 4, 3));
            bool result=board.Move(generateMove(3, 4, 4, 3));
            Assert.True(result);
        }

        private ChessMove generateMove(int initialX, int initialY, int targetX, int targetY)
        {
            ChessMove move = new ChessMove();
            move.InitialX = initialX;
            move.InitialY = initialY;
            move.TargetX = targetX;
            move.TargetY = targetY;
            return move;
        }
    }
}
