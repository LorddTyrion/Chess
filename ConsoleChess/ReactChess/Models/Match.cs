namespace ReactBoardGame.Models
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
        public Guid Type { get; set; }
        public string Player1Id { get; set; }
        public User Player1 { get; set; } = null!;
        public string Player2Id { get; set; }
        public User Player2 { get; set; } = null!;
        public GameResult Result { get; set; }
        public string SerializedBoard { get; set; }
    }
}
