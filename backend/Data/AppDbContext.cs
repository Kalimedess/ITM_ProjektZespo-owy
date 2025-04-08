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
        public DbSet<Deck> Decks { get; set; }
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
        public DbSet<GameProcess> GameProcess { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Klucze Główne
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Decision>()
                .HasKey(d => d.DecisionId);

            modelBuilder.Entity<DecisionWeight>()
                .HasKey(dw => dw.DecisionWeightId);

            modelBuilder.Entity<DecisionEnabler>()
                .HasKey(de => de.CardEnablerId);

            modelBuilder.Entity<Game>()
                .HasKey(ga => ga.GameId);

            modelBuilder.Entity<GameBoard>()
                .HasKey(gb => new { gb.GameId, gb.TeamId });

            modelBuilder.Entity<GameLog>()
                .HasKey(gl => new { gl.GameId, gl.TeamId });

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

    // Klucze Obce
        //Games
            modelBuilder.Entity<Game>()
                .HasOne(g => g.User)
                .WithMany()
                .HasForeignKey(g => g.UserId);
            
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Deck)
                .WithMany()
                .HasForeignKey(g => g.DeckId);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.Board)
                .WithMany()
                .HasForeignKey(g => g.BoardId);
        //GameLogs
            modelBuilder.Entity<GameLog>()
                .HasOne(gl => gl.Game)
                .WithMany()
                .HasForeignKey(gl => gl.GameId);

            modelBuilder.Entity<GameLog>()
                .HasOne(gl => gl.Team)
                .WithMany()
                .HasForeignKey(gl => gl.TeamId);

            modelBuilder.Entity<GameLog>()
                .HasOne(gl => gl.Decision)
                .WithMany()
                .HasForeignKey(gl => gl.DecisionId);

            modelBuilder.Entity<GameLog>()
                .HasOne(gl => gl.Item)
                .WithMany()
                .HasForeignKey(gl => gl.ItemId);

            modelBuilder.Entity<GameLog>()
                .HasOne(gl => gl.Feedback)
                .WithMany()
                .HasForeignKey(gl => gl.FeedbackId);
        //Decisions
            modelBuilder.Entity<Decision>()
                .HasOne(d => d.Deck)
                .WithMany()
                .HasForeignKey(d => d.DeckId);
        //DecisionWeights
            modelBuilder.Entity<DecisionWeight>()
                .HasOne(dw => dw.Decision)
                .WithOne()
                .HasForeignKey<DecisionWeight>(dw => dw.DecisionId);
        //DecisionCost
            modelBuilder.Entity<DecisionCost>()
                .HasOne(dc => dc.Decision)
                .WithOne()
                .HasForeignKey<DecisionCost>(dc => dc.DecisionId);
        //GameLogMove
            modelBuilder.Entity<GameLogMove>()
                .HasOne(glm => glm.GameProcess)
                .WithOne()
                .HasForeignKey<GameLogMove>(glm => glm.ProcessId);

            modelBuilder.Entity<GameLogMove>()
                .HasOne(glm => glm.Board)
                .WithMany()
                .HasForeignKey(glm => glm.BoardId);

            modelBuilder.Entity<GameLogMove>()
                .HasOne(glm => glm.Team)
                .WithOne()
                .HasForeignKey<GameLogMove>(glm => glm.TeamId);
        //GameLogSpec
            modelBuilder.Entity<GameLogSpec>()
                .HasOne(gls => gls.GameLog)
                .WithMany()
                .HasForeignKey(gls => new { gls.GameLogId, gls.TeamId });

            modelBuilder.Entity<GameLogSpec>()
                .HasOne(gls => gls.GameProcess)
                .WithMany()
                .HasForeignKey(gls => gls.ProcessId);
            
            modelBuilder.Entity<GameLogSpec>()
                .HasOne(gls => gls.Team)
                .WithMany()
                .HasForeignKey(gls => gls.TeamId);
        //GameBoard
            modelBuilder.Entity<GameBoard>()
                .HasOne(gb => gb.Team)
                .WithOne()
                .HasForeignKey<GameBoard>(gb => gb.TeamId);

            modelBuilder.Entity<GameBoard>()
                .HasOne(gb => gb.Game)
                .WithOne()
                .HasForeignKey<GameBoard>(gb => gb.GameId);

            modelBuilder.Entity<GameBoard>()
                .HasOne(gb => gb.GameProcess)
                .WithMany()
                .HasForeignKey(gb => gb.ProcessId);
        //Team
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Game)
                .WithMany()
                .HasForeignKey(t => t.GameId);
        //ItemCost
            modelBuilder.Entity<ItemsCost>()
                .HasOne(ic => ic.Item)
                .WithOne()
                .HasForeignKey<ItemsCost>(ic => ic.ItemsId);
        //Feedback
            modelBuilder.Entity<Feedback>()
                .HasOne(fb => fb.Decision)
                .WithOne()
                .HasForeignKey<Feedback>(fb => fb.DecisionId);
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
