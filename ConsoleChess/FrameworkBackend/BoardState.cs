using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBackend
{
    [JsonObject(MemberSerialization.Fields)]
    public abstract class BoardState<TBoardState> where TBoardState:BoardState<TBoardState> { 
        public Color turnOf;
        public abstract bool PositionEquals (TBoardState other);
        public virtual void SerializeBoard(string fileName)
        {
            
            //string jsonString = JsonConvert.SerializeObject (this, Formatting.Indented);
            string jsonTypeNameAuto = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            File.WriteAllText(fileName, jsonTypeNameAuto);
            //Console.WriteLine(jsonString);
        }
        public virtual TBoardState DeserializeBoard(string fileName)
        {
            TBoardState boardState = JsonConvert.DeserializeObject<TBoardState>(File.ReadAllText(fileName), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return boardState;
        }

    }
}
