namespace ReactChess.Models
{
    public class PlayedMatch
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string UserID { get; set; }
        public Match Match { get; set; }
        public int MatchId { get; set; }
        public int Index { get; set; }
        
    }
}
