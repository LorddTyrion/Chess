using ConsoleChess;
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
                    game.WhitePlayer = name;
                }
                else game.BlackPlayer = name;
                return false;
            }
            else
            {
                if(games[games.Count - 1].BlackPlayer == null)
                {
                    games[games.Count - 1].BlackPlayer = name;
                }
                else if(games[games.Count - 1].WhitePlayer == null)
                {
                    games[games.Count - 1].WhitePlayer = name;
                }
                games[games.Count - 1].State = GameState.STARTED;
                return true;
            }
        }
        public int IdByName(string name)
        {
            foreach(Game game in games)
            {
                if(game.WhitePlayer == name || game.BlackPlayer == name)
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
                    if(game.BlackPlayer!=null) list.Add(game.BlackPlayer);
                    if(game.WhitePlayer!=null) list.Add(game.WhitePlayer);
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
        public bool IsValid(string name, int gameid)
        {         
            int id= IdByName(name);
            if(id!=gameid) return false;
            Game game=GameById(id);
            if (game.State != GameState.STARTED) return false;
            if (game.BlackPlayer == name && game.Board.boardState.turnOf == Color.BLACK) return true;
            if(game.WhitePlayer==name && game.Board.boardState.turnOf == Color.WHITE) return true;
            return false;
        }
        public void DeleteGame(Game game)
        {
            games.Remove(game);
        }
    }
}
