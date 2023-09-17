using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBackend
{
    [JsonObject(MemberSerialization.Fields)]
    public abstract class BoardState
    {
        public abstract bool PositionEquals (BoardState other);
        public virtual void SerializeBoard(string fileName)
        {
            
            string jsonString = JsonConvert.SerializeObject (this, Formatting.Indented);
            File.WriteAllText(fileName, jsonString);
            //Console.WriteLine(jsonString);
        }
        public abstract BoardState DeserializeBoard(string fileName);
      
    }
}
