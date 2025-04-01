namespace backend.Data
{
    public class Team
    {
        public int TeamId { get; set; }
        public int GameId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string TeamLeader { get; set; } = string.Empty;
        public int TeamBud { get; set; }
        public string TeamToken { get; set; } = string.Empty;
    }
}