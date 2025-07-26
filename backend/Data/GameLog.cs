namespace backend.Data
{
    public class GameLog
    {
        public int GameLogId { get; set; }
        public DateTime Data { get; set; }
        public int? TeamId { get; set; }
        public Team? Team { get; set; } = null!;
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;
        public int? GameEventId { get; set; }
        public GameEvent? GameEvent { get; set; }
        public int? CardId { get; set; }
        public Card? Card { get; set; } = null!;
        public int? DeckId { get; set; }
        public Deck? Deck { get; set; } = null!;
        public int? BoardId { get; set; }
        public Board? Board { get; set; } = null!;
        public int? FeedbackId { get; set; }
        public Feedback? Feedback { get; set; }
        public double? Cost { get; set; }
        public bool? Status { get; set; }

        public bool? IsApproved { get; set; }

        public virtual ICollection<GameLogSpec> GameLogSpecs { get; set; } = new HashSet<GameLogSpec>();
    }
}