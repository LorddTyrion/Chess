namespace ReactChess.Models
{
    public enum GameResult
    {
        FirstWon,
        SecondWon,
        Draw,
        InProgress
    }
    public class Match
    {
        public int Id { get; set; }
        public int Type { get; set; } //Guid
        public GameResult Result { get; set; }
    }
}
