namespace backend.Data
{
    public class Team
    {
        public int Team_id { get; set; }
        public int Game_id { get; set; }
        public string Team_name { get; set; } = string.Empty;
        public string Team_leader { get; set; } = string.Empty;
        public int Team_bud { get; set; }
    }
}