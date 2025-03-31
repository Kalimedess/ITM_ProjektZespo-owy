using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Decision> Decisions { get; set; }
        public DbSet<DecisionCost> DecisionCosts { get; set;}
        public DbSet<DecisionEnabler> DecisionEnablers { get; set; }
        public DbSet<DecisionWeight> DecisionWeights { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<GameBoard> GameBoards { get; set; }
        public DbSet<GameLog> GameLogs { get; set; }
        public DbSet<GameLogMove> GameLogMoves { get; set; }
        public DbSet<GameLogSpec> GameLogSpecs { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemsCost> ItemsCosts { get; set; }
        public DbSet<GameProcess> Processes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DecisionWeight>()
                .HasKey(dw => dw.DecisionWeightId);

            modelBuilder.Entity<DecisionEnabler>()
                .HasKey(de => de.CardEnablerId);

            modelBuilder.Entity<Game>()
                .HasKey(ga => ga.Game_id);

            modelBuilder.Entity<GameBoard>()
                .HasKey(gb => gb.GameBoard_id);

            modelBuilder.Entity<GameLog>()
                .HasKey(gl => gl.GameLog_id);

            modelBuilder.Entity<GameLogMove>()
                .HasKey(gm => gm.GameLogMove_id);

            modelBuilder.Entity<GameLogSpec>()
                .HasKey(gs => gs.GameLogSpec_id);

            modelBuilder.Entity<GameProcess>()
                .HasKey(gp => gp.Process_id);

            modelBuilder.Entity<Item>()
                .HasKey(it => it.Items_id);

            modelBuilder.Entity<Team>()
                .HasKey(tm => tm.Team_id);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=mydatabase.db");
            }
        }
    }
}
