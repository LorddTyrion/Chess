using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBackend
{
    public enum Color
    {
        WHITE = 0, BLACK = 1, DRAW = 2, NONE = 3
    }
    public abstract class Board<TBoardState, TMove> :Board
        where TBoardState : BoardState<TBoardState, TMove>
        where TMove:Move
    {
        public TBoardState boardState;
        public override Color GetTurnOf()
        {
            return boardState.turnOf;
        }
        public override IEnumerable<Field> BoardToList()
        {
            return boardState.boardToList();
        }

        public override IEnumerable<Move> GetMoves()
        {
            return boardState.GetMoves();
        }
        public override string SerializeBoard()
        {
            return boardState.SerializeBoard();
        }
        public override BoardState DeserializeBoard(string serializedBoard)
        {
            return boardState.DeserializeBoard(serializedBoard);
        }
    }
    public abstract class Board
    {
        public abstract bool Move<TMove>(TMove move) where TMove : Move;
        public abstract IEnumerable<Move> getPossibleMoves(int x, int y);
        public abstract Color CheckEndGame();
        public abstract int GetSumValue(Color color);
        public abstract Color GetTurnOf();
        public abstract IEnumerable<Field> BoardToList();
        public abstract IEnumerable<Move> GetMoves();
        public abstract string SerializeBoard();
        public abstract BoardState DeserializeBoard(string serializedBoard);
    }
}
