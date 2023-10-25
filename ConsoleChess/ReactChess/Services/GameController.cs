using ConsoleChess;
using FrameworkBackend;
namespace ReactBoardGame.Services
{
    public class GameController
    {
        List<Game> chessgames=new List<Game>();
        List<Game> tictactoegames=new List<Game>();
        Dictionary<GameType,List<Game>> games=new Dictionary<GameType,List<Game>>();
        public GameController()
        {
            games.Add(GameType.CHESS, chessgames);
            games.Add(GameType.TICTACTOE, tictactoegames);
        }
        public bool AddPlayer(string name, int gametype)
        {
            if (games[(GameType)gametype].Count==0 || games[(GameType)gametype][games[(GameType)gametype].Count - 1].State != GameState.PREPARING)
            {
                Game game = GameFactory.Make((GameType)gametype);

                games[(GameType)gametype].Add(game);
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
                if(games[(GameType)gametype][games[(GameType)gametype].Count - 1].SecondPlayer == null)
                {
                    games[(GameType)gametype][games[(GameType)gametype].Count - 1].SecondPlayer = name;
                }
                else if(games[(GameType)gametype][games[(GameType)gametype].Count - 1].FirstPlayer == null)
                {
                    games[(GameType)gametype][games[(GameType)gametype].Count - 1].FirstPlayer = name;
                }
                games[(GameType)gametype][games[(GameType)gametype].Count - 1].State = GameState.STARTED;
                return true;
            }
        }
        public int IdByName(string name)
        {
            foreach(List<Game> gamelist in games.Values)
            {
                foreach (Game game in gamelist)
                {
                    if (game.FirstPlayer == name || game.SecondPlayer == name)
                    {
                        return game.GameID;
                    }
                }
            }
            return -1;
        }
        public List<string> PlayersById(int id)
        {
            List<string> list = new List<string>();
            foreach (List<Game> gamelist in games.Values)
            {
                foreach (Game game in gamelist)
                {
                    if (game.GameID == id)
                    {
                        if (game.SecondPlayer != null) list.Add(game.SecondPlayer);
                        if (game.FirstPlayer != null) list.Add(game.FirstPlayer);
                    }
                }
            }
            return list;
        }
        public Game GameById(int id)
        {
            foreach (List<Game> gamelist in games.Values)
            {
                foreach (Game game in gamelist)
                {
                    if (game.GameID == id) return game;
                }
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
        public void DeleteGame(Game game,int gametype)
        {
            games[(GameType)gametype].Remove(game);
        }
    }
}
