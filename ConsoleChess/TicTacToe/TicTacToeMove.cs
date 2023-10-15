using FrameworkBackend;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class TicTacToeMove:Move
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Move Generate(string stringifiedMove)
        {
            TicTacToeMove move = new TicTacToeMove();
            var jObj = JObject.Parse(stringifiedMove);
            move.X = (int)jObj["X"];
            move.Y = (int)jObj["Y"];
            return move;
        }
    }
}
