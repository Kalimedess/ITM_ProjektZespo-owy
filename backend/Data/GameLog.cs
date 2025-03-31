namespace backend.Data
{
    public class GameLog
    {
        public int GameLog_id { get; set; }
        public int Team_id { get; set; }
        public int Game_id { get; set; }
        public int Decision_id{ get; set; }
        public int Item_id { get; set; }
        public DateTime Data { get; set; }
        public int feedback_id { get; set; }
        public int Cost { get; set; }
        public int Status_id { get; set; }
    }
}