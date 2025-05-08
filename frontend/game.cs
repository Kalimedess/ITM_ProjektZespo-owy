namespace backend.Data
{
    public class GameBoard
    {
        public int GameBoard_id { get; set; }
        public string GameBoard_desc { get; set; } = string.Empty;
        public string GameBoard_long_desc { get; set; } = string.Empty;
    }

    public class GameLog
    {
        public int GameLog_id { get; set; }
        public string GameLog_desc { get; set; } = string.Empty;
        public string GameLog_long_desc { get; set; } = string.Empty;
    }

    public class GameLogSpec
    {
        public int GameLogSpec_id { get; set; }
        public string GameLogSpec_desc { get; set; } = string.Empty;
        public string GameLogSpec_long_desc { get; set; } = string.Empty;
    }

    public class GameLogMove
    {
        public int GameLogMove_id { get; set; }
        public string GameLogMove_desc { get; set; } = string.Empty;
        public string GameLogMove_long_desc { get; set; } = string.Empty;
    }
}
