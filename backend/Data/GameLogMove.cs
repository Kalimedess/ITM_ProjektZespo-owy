namespace backend.Data
{
    public class GameLogMove
    {
        public int GameLogMove_id { get; set; }
        public int Board_id { get; set; }
        public int Process_id{ get; set; }
        public int Team_id { get; set; }
        public int Move_X { get; set; }
        public int Move_Y { get; set; }
        public int Sub_board { get; set; }
    }
}
