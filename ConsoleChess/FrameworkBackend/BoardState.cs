using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBackend
{
    public abstract class BoardState
    {
        public Color turnOf;
        
        public abstract bool PositionEquals(BoardState other);
        public abstract IEnumerable<Field> boardToList();
        public abstract IEnumerable<Move> GetMoves();
        public abstract string SerializeBoard();
        public abstract BoardState DeserializeBoard(string fileName);
    }
    [JsonObject(MemberSerialization.Fields)]
    public abstract class BoardState<TBoardState, TMove> : BoardState
        where TBoardState:BoardState<TBoardState, TMove>
        where TMove:Move
        {
        public List<TMove> moves;
        public override string SerializeBoard()
        {
            
            //string jsonString = JsonConvert.SerializeObject (this, Formatting.Indented);
            string jsonTypeNameAuto = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return jsonTypeNameAuto;
            
        }
        public override BoardState DeserializeBoard(string serializedBoard)
        {
            TBoardState boardState = JsonConvert.DeserializeObject<TBoardState>(serializedBoard, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return boardState;
        }
        

    }
}
