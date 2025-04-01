using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) 
            : base(options) 
        {
            _configuration = configuration;
        }

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
                .HasKey(ga => ga.GameId);

            modelBuilder.Entity<GameBoard>()
                .HasKey(gb => gb.GameBoardId);

            modelBuilder.Entity<GameLog>()
                .HasKey(gl => gl.GameLogId);

            modelBuilder.Entity<GameLogMove>()
                .HasKey(gm => gm.GameLogMoveId);

            modelBuilder.Entity<GameLogSpec>()
                .HasKey(gs => gs.GameLogSpecId);

            modelBuilder.Entity<GameProcess>()
                .HasKey(gp => gp.ProcessId);

            modelBuilder.Entity<Item>()
                .HasKey(it => it.ItemsId);

            modelBuilder.Entity<Team>()
                .HasKey(tm => tm.TeamId);

            base.OnModelCreating(modelBuilder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}
    }
}
