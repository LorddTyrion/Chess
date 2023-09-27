using ConsoleChess;
using FrameworkBackend;
namespace ReactChess.Services
{
    public class GameController
    {
        List<Game> games=new List<Game>();
        public bool AddPlayer(string name)
        {
            if (games.Count==0 || games[games.Count - 1].State != GameState.PREPARING)
            {
                Game game=new Game();
                
                games.Add(game);
                Random vel=new Random();
                if (vel.Next(1, 3) == 1)
                {
                    game.FirstPlayer = name;
                }
                else game.SecondPlayer = name;
                return false;
            }
            else
            {
                if(games[games.Count - 1].SecondPlayer == null)
                {
                    games[games.Count - 1].SecondPlayer = name;
                }
                else if(games[games.Count - 1].FirstPlayer == null)
                {
                    games[games.Count - 1].FirstPlayer = name;
                }
                games[games.Count - 1].State = GameState.STARTED;
                return true;
            }
        }
        public int IdByName(string name)
        {
            foreach(Game game in games)
            {
                if(game.FirstPlayer == name || game.SecondPlayer == name)
                {
                    return game.GameID;
                }    
            }
            return -1;
        }
        public List<string> PlayersById(int id)
        {
            List<string> list = new List<string>();
            foreach (Game game in games)
            {
                if (game.GameID==id)
                {
                    if(game.SecondPlayer!=null) list.Add(game.SecondPlayer);
                    if(game.FirstPlayer!=null) list.Add(game.FirstPlayer);
                }
            }
            return list;
        }
        public Game GameById(int id)
        {
            foreach (Game game in games)
            {
                if (game.GameID == id) return game;
            }
            return null;
        }
        public bool IsValid(string name, int gameid, Color turnOf) 
        {         
            int id= IdByName(name);
            if(id!=gameid) return false;
            Game game=GameById(id);
            if (game.State != GameState.STARTED) return false;
            if (game.SecondPlayer == name && turnOf == Color.BLACK) return true;
            if(game.FirstPlayer==name && turnOf == Color.WHITE) return true;
            return false;
        }
        public void DeleteGame(Game game)
        {
            games.Remove(game);
        }
    }
}
