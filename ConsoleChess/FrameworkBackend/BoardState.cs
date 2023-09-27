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
        public abstract void SerializeBoard(string fileName);
        public abstract BoardState DeserializeBoard(string fileName);
    }
    [JsonObject(MemberSerialization.Fields)]
    public abstract class BoardState<TBoardState, TMove> : BoardState
        where TBoardState:BoardState<TBoardState, TMove>
        where TMove:Move
        {
        public List<TMove> moves;
        public override void SerializeBoard(string fileName)
        {
            
            //string jsonString = JsonConvert.SerializeObject (this, Formatting.Indented);
            string jsonTypeNameAuto = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            File.WriteAllText(fileName, jsonTypeNameAuto);
            //Console.WriteLine(jsonString);
        }
        public override BoardState DeserializeBoard(string fileName)
        {
            TBoardState boardState = JsonConvert.DeserializeObject<TBoardState>(File.ReadAllText(fileName), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return boardState;
        }
        

    }
}
