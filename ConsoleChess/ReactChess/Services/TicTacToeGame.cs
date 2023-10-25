﻿using ConsoleChess;
using TicTacToe;
namespace ReactBoardGame.Services
{
    [IdAttribute("66DF58E5-E31A-4B6A-8FC1-A4E61A1A9A75")]
    public class TicTacToeGame : Game<TicTacToeBoard, TicTacToeBoardState, TicTacToeMove>
    {
        public TicTacToeGame()
        {
            Type = GameType.TICTACTOE;
            Board = new TicTacToeBoard();
        }
        public TicTacToeGame(TicTacToeGame game)
        {
            Board = new TicTacToeBoard();
            FirstPlayer = game.FirstPlayer;
            SecondPlayer = game.SecondPlayer;
            GameID = game.GameID;
            DbID = game.DbID;
            State = game.State;
            Result = game.Result;
        }
    }
}
