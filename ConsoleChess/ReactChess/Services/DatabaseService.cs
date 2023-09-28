using ReactChess.Data;
using ReactChess.Models;
using FrameworkBackend;
using Microsoft.EntityFrameworkCore;

namespace ReactChess.Services
{
    public class DatabaseService
    {
        public int GameSetup(ApplicationDbContext context, GameController gameController, int gameID)
        {
            Match m = new Match(); //m.Type = 0;;
            Game game=gameController.GameById(gameID);
            IdAttribute idattr= (IdAttribute)Attribute.GetCustomAttribute(game.GetType(), typeof(IdAttribute));
            m.Type = Guid.Parse(idattr.Id);

            List<string> players = gameController.PlayersById(gameID);


            if (gameController.GameById(gameID).FirstPlayer == players[0])
            {
                m.Player1Id = context.Users.Where(p => p.UserName == players[0]).FirstOrDefault()!.Id;
                m.Player2Id = context.Users.Where(p => p.UserName == players[1]).FirstOrDefault()!.Id;
            }
            else if (gameController.GameById(gameID).SecondPlayer == players[0])
            {
                m.Player2Id = context.Users.Where(p => p.UserName == players[0]).FirstOrDefault()!.Id;
                m.Player1Id = context.Users.Where(p => p.UserName == players[1]).FirstOrDefault()!.Id;
            }
            m.SerializedBoard = "";
            context.MatchSet.Add(m);

            context.SaveChanges();
            return m.Id;
        }

        public void GameEndedNaturally(ApplicationDbContext context, GameController gameController, int gameID, string CurrentUserId, Color end)
        {
            var match = context.MatchSet.Where(m => (m.Player1Id == CurrentUserId || m.Player2Id == CurrentUserId) && m.Id == gameID).Include(m => m.Player1).Include(m => m.Player2).FirstOrDefault();
            match.Result = (GameResult)(int)end;
            match.Player1.Games++;
            match.Player2.Games++;
            if (end == Color.WHITE)
            {
                match.Player1.Wins++;
                match.Player2.Losses++;
            }
            else if ((int)end == 2)
            {
                match.Player1.Draws++;
                match.Player2.Draws++;
            }
            else
            {
                match.Player1.Losses++;
                match.Player2.Wins++;
            }
            context.SaveChanges();
        }

        public void GameEndedByResignation(ApplicationDbContext context, string CurrentUserId, Game game)
        {
            var match = context.MatchSet.Where(m => (m.Player1Id == CurrentUserId || m.Player2Id == CurrentUserId) && m.Id == game.DbID).Include(m => m.Player1).Include(m => m.Player2).FirstOrDefault();
            match.Result = CurrentUserId == match.Player1Id ? GameResult.SecondWon : GameResult.FirstWon;
            match.Player1.Games++;
            match.Player2.Games++;
            if (match.Player1Id == CurrentUserId)
            {
                match.Player1.Losses++;
                match.Player2.Wins++;
            }
            else
            {
                match.Player2.Losses++;
                match.Player1.Wins++;
            }

            context.SaveChanges();
        }
    }
}
