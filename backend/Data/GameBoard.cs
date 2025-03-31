namespace backend.Data
{
    public class GameBoard
    {
        public int GameBoard_id { get; set; }
        public int Team_id { get; set; }
        public int Game_id { get; set; }
        public int Process_id{ get; set; }
        public int Poz_X { get; set; }
        public int Poz_Y { get; set; }
        public int Subboard_id { get; set; }
    }
}