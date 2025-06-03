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
        public DbSet<Card> Cards { get; set; }
        public DbSet<Decision> Decisions { get; set; }
        public DbSet<DecisionEnabler> DecisionEnablers { get; set; }
        public DbSet<DecisionWeight> DecisionWeights { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<GameBoard> GameBoards { get; set; }
        public DbSet<GameLog> GameLogs { get; set; }
        public DbSet<GameLogSpec> GameLogSpecs { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<GameProcess> GameProcess { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Klucze Główne
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<Decision>()
                .HasKey(d => d.DecisionId);

            modelBuilder.Entity<DecisionWeight>()
                .HasKey(dw => dw.DecisionWeightId);

            modelBuilder.Entity<DecisionEnabler>()
                .HasKey(de => de.DecisionEnablerId);

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(ga => ga.GameId);

                entity.Property(g => g.GameId)
                      .ValueGeneratedOnAdd();

                entity.HasOne(g => g.User)
                      .WithMany()
                      .HasForeignKey(g => g.UserId);
            
                entity.HasOne(g => g.Deck)
                      .WithMany()
                      .HasForeignKey(g => g.DeckId);

                entity.HasOne(g => g.Board)
                      .WithMany()
                      .HasForeignKey(g => g.BoardId);

                entity.HasMany(g => g.Teams)
                      .WithOne(t => t.Game)
                      .HasForeignKey(t => t.GameId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.Property(g => g.GameStatus)
                      .HasConversion<string>()
                      .IsRequired(false);
            });   

            modelBuilder.Entity<GameBoard>()
                .HasKey(gb => new { gb.GameId, gb.TeamId });

            modelBuilder.Entity<GameLog>()
                .HasKey(gl => new { gl.GameId, gl.TeamId });

            modelBuilder.Entity<GameLogSpec>()
                .HasKey(gs => gs.GameLogSpecId);

            modelBuilder.Entity<GameProcess>()
                .HasKey(gp => gp.GameProcessId);

            modelBuilder.Entity<Item>()
                .HasKey(it => it.ItemsId);

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(tm => tm.TeamId);

                entity.Property(t => t.TeamId)
                      .ValueGeneratedOnAdd();

            });



            modelBuilder.Entity<Card>()
                .HasKey(c => c.CardId);

            base.OnModelCreating(modelBuilder);

    // Klucze Obce
        //Deck
            modelBuilder.Entity<Deck>()
                .HasOne(d => d.User)
                .WithOne()
                .HasForeignKey<Deck>(d => d.UserId);
        //Cards
            modelBuilder.Entity<Card>()
                .HasMany(c => c.DecisionEnablers)
                .WithOne(de => de.Card)
                .HasForeignKey(de => de.CardId)
                .HasPrincipalKey(c => c.CardId);
            modelBuilder.Entity<Card>()
                .HasMany(c => c.DecisionEnablerOfThis)
                .WithOne(de => de.CardEnabler)
                .HasForeignKey(de => de.EnablerId)
                .HasPrincipalKey(c => c.CardId);

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
                .HasOne(gl => gl.Card)
                .WithMany()
                .HasForeignKey(gl => gl.CardId);
            
            modelBuilder.Entity<GameLog>()
                .HasOne(gl => gl.Deck)
                .WithMany()
                .HasForeignKey(gl => gl.DeckId);

            modelBuilder.Entity<GameLog>()
                .HasOne(gl => gl.Feedback)
                .WithMany()
                .HasForeignKey(gl => gl.FeedbackId);
        //Decisions
            modelBuilder.Entity<Decision>()
                .HasOne(d => d.Deck)
                .WithMany()
                .HasForeignKey(d => d.DeckId);
            modelBuilder.Entity<Decision>()
                .HasOne(d => d.Card)
                .WithMany()
                .HasForeignKey(d => d.CardId);
        //DecisionWeights
            modelBuilder.Entity<DecisionWeight>()
                .HasOne(dw => dw.Card)
                .WithMany()
                .HasForeignKey(dw => dw.CardId)
                .HasPrincipalKey(c => c.CardId);
            modelBuilder.Entity<DecisionWeight>()
                .HasOne(dw => dw.Board)
                .WithOne()
                .HasForeignKey<DecisionWeight>(dw => dw.BoardId);
        //GameLogSpec
            modelBuilder.Entity<GameLogSpec>()
                .HasOne(gls => gls.GameLog)
                .WithMany()
                .HasForeignKey(gls => new { gls.GameLogId, gls.TeamId });

            modelBuilder.Entity<GameLogSpec>()
                .HasOne(gls => gls.Team)
                .WithMany()
                .HasForeignKey(gls => gls.TeamId);

            modelBuilder.Entity<GameLogSpec>()
                .HasOne(gls => gls.Board)
                .WithMany()
                .HasForeignKey(gls => gls.BoardId);

            modelBuilder.Entity<GameLogSpec>()
                .HasOne(gls => gls.GameProcess)
                .WithMany()
                .HasForeignKey(gls => gls.GameProcessId);

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
                .HasForeignKey(gb => gb.GameProcessId);
            
            modelBuilder.Entity<GameBoard>()
                .HasOne(gb => gb.Board)
                .WithMany()
                .HasForeignKey(gb => gb.BoardId);
        //Feedback
            modelBuilder.Entity<Feedback>()
                .HasOne(fb => fb.Card)
                .WithMany()
                .HasForeignKey(fb => fb.CardId);
            modelBuilder.Entity<Feedback>()
                .HasOne(fb => fb.Deck)
                .WithMany()
                .HasForeignKey(fb => fb.DeckId);

        // Board
            modelBuilder.Entity<Board>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);

        //Item
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Card)
                .WithMany()
                .HasForeignKey(i =>i.CardId);
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Deck)
                .WithMany()
                .HasForeignKey(i =>i.DeckId);

    //Inne
            modelBuilder.Entity<Card>()
                .Property(c => c.CardType)
                .HasConversion<string>();

            modelBuilder.Entity<Game>()
                .Property(g => g.GameStatus)
                .HasConversion<string>()
                .IsRequired(false);

            modelBuilder.Entity<DecisionWeight>()
                .HasIndex(dw => dw.BoardId)
                .IsUnique(false);
        
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
