using Microsoft.EntityFrameworkCore;
// Upewnij się, że masz tutaj odpowiednie usingi do swoich modeli, np.:
using backend.Data;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets
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
        public DbSet<Item> Items { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<GameProcess> GameProcesses { get; set; }
        public DbSet<GameLogSpec> GameLogSpecs { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                  base.OnModelCreating(modelBuilder);

                  // User Configuration
                  modelBuilder.Entity<User>(entity =>
                  {
                        entity.HasKey(u => u.UserId);
                  });

                  // Board Configuration
                  modelBuilder.Entity<Board>(entity =>
                  {
                        entity.HasKey(b => b.BoardId);

                        entity.HasOne(b => b.User)
                        .WithMany()
                        .HasForeignKey(b => b.UserId);
                  });

                  // Deck Configuration
                  modelBuilder.Entity<Deck>(entity =>
                  {
                        entity.HasKey(d => d.DeckId);

                        entity.HasOne(d => d.User)
                              .WithMany()
                              .HasForeignKey(d => d.UserId)
                              .OnDelete(DeleteBehavior.Cascade);
                  });

                  // Card Configuration
                  modelBuilder.Entity<Card>(entity =>
                  {
                        entity.HasKey(c => c.CardId);
                        entity.Property(c => c.CardType).HasConversion<string>();

                        entity.HasMany(c => c.DecisionEnablers)
                        .WithOne(de => de.Card)
                        .HasForeignKey(de => de.CardId);

                        entity.HasMany(c => c.DecisionEnablerOfThis)
                        .WithOne(de => de.CardEnabler)
                        .HasForeignKey(de => de.EnablerId);
                  });

                  // Game Configuration
                  modelBuilder.Entity<Game>(entity =>
                  {
                        entity.HasKey(g => g.GameId);
                        entity.Property(g => g.GameId).ValueGeneratedOnAdd();

                        entity.Property(g => g.GameStatus)
                        .HasConversion<string>()
                        .IsRequired(false);

                        // Foreign Keys
                        entity.HasOne(g => g.User)
                        .WithMany()
                        .HasForeignKey(g => g.UserId);

                        entity.HasOne(g => g.Deck)
                        .WithMany()
                        .HasForeignKey(g => g.DeckId);

                        // --- POPRAWKA: Poprawna konfiguracja dwóch relacji do plansz (Board) ---
                        entity.HasOne(g => g.TeamBoard)
                        .WithMany() // Zakładając, że jedna plansza może być użyta w wielu grach
                        .HasForeignKey(g => g.TeamBoardId);

                        entity.HasOne(g => g.RivalBoard)
                        .WithMany()
                        .HasForeignKey(g => g.RivalBoardId);

                        // Relationships to other entities that reference Game
                        entity.HasMany(g => g.Teams)
                        .WithOne(t => t.Game)
                        .HasForeignKey(t => t.GameId)
                        .OnDelete(DeleteBehavior.Cascade);

                        entity.HasMany(g => g.GameProcesses)
                        .WithOne(p => p.Game)
                        .HasForeignKey(p => p.GameId)
                        .OnDelete(DeleteBehavior.Restrict);

                        entity.HasMany(g => g.GameBoards)
                        .WithOne(gb => gb.Game)
                        .HasForeignKey(gb => gb.GameId)
                        .OnDelete(DeleteBehavior.Cascade);

                        entity.HasMany(g => g.GameLogs)
                        .WithOne(gl => gl.Game)
                        .HasForeignKey(gl => gl.GameId)
                        .OnDelete(DeleteBehavior.Cascade);
                  });

                  // Team Configuration
                  modelBuilder.Entity<Team>(entity =>
                  {
                        entity.HasKey(t => t.TeamId);
                        entity.Property(t => t.TeamId).ValueGeneratedOnAdd();
                        entity.HasMany(t => t.GameProcesses)
                            .WithOne(p => p.Team)
                            .HasForeignKey(p => p.TeamId)
                            .OnDelete(DeleteBehavior.Cascade);
                  });

                  // GameBoard Configuration
                  modelBuilder.Entity<GameBoard>(entity =>
                  {
                        entity.HasKey(gb => gb.GameBoardId);

                        entity.HasOne(gb => gb.Team)
                        .WithMany()
                        .HasForeignKey(gb => gb.TeamId);

                        entity.HasOne(gb => gb.GameProcess)
                        .WithMany()
                        .HasForeignKey(gb => gb.GameProcessId);

                        entity.HasOne(gb => gb.Board)
                        .WithMany()
                        .HasForeignKey(gb => gb.BoardId);
                  });

                  // GameLog Configuration
                  modelBuilder.Entity<GameLog>(entity =>
                  {
                        entity.HasKey(gl => gl.GameLogId);

                        entity.HasOne(gl => gl.Team)
                        .WithMany()
                        .HasForeignKey(gl => gl.TeamId);

                        entity.HasOne(gl => gl.Card)
                        .WithMany()
                        .HasForeignKey(gl => gl.CardId);

                        entity.HasOne(gl => gl.Deck)
                        .WithMany()
                        .HasForeignKey(gl => gl.DeckId);

                        entity.HasOne(gl => gl.Feedback)
                        .WithMany()
                        .HasForeignKey(gl => gl.FeedbackId);

                        entity.HasOne(gl => gl.Board)
                        .WithMany()
                        .HasForeignKey(gl => gl.BoardId);
                  });

                  // Decision Configuration
                  modelBuilder.Entity<Decision>(entity =>
                  {
                        entity.HasKey(d => d.DecisionId);

                        entity.HasOne(d => d.Deck)
                        .WithMany(deck => deck.Decisions)
                        .HasForeignKey(d => d.DeckId)
                        .OnDelete(DeleteBehavior.Cascade);

                        entity.HasOne(d => d.Card)
                        .WithMany()
                        .HasForeignKey(d => d.CardId);
                  });

                  // DecisionWeight Configuration
                  modelBuilder.Entity<DecisionWeight>(entity =>
                  {
                        entity.HasKey(dw => dw.DecisionWeightId);

                        entity.HasOne(dw => dw.Card)
                              .WithMany()
                              .HasForeignKey(dw => dw.CardId);

                        entity.HasOne(dw => dw.Deck)
                              .WithMany()
                              .HasForeignKey(dw => dw.DeckId);

                        entity.HasOne(dw => dw.Process)
                              .WithMany()
                              .HasForeignKey(dw => dw.ProcessId);
                  });

                  // DecisionEnabler Configuration
                  modelBuilder.Entity<DecisionEnabler>(entity =>
                  {
                        entity.HasKey(de => de.DecisionEnablerId);

                        // --- POPRAWKA: Dodano konfigurację relacji, które nie są częścią Card ---
                        entity.HasOne(de => de.Game)
                        .WithMany()
                        .HasForeignKey(de => de.GameId);

                        entity.HasOne(de => de.Team)
                        .WithMany()
                        .HasForeignKey(de => de.TeamId);
                  });

                  // Feedback Configuration
                  modelBuilder.Entity<Feedback>(entity =>
                  {
                        entity.HasKey(f => f.FeedbackId);

                        entity.HasOne(f => f.Deck)
                        .WithMany(deck => deck.Feedbacks)
                        .HasForeignKey(f => f.DeckId)
                        .OnDelete(DeleteBehavior.Cascade);

                        entity.HasOne(f => f.Card)
                        .WithMany()
                        .HasForeignKey(f => f.CardId);
                  });

                  // Item Configuration
                  modelBuilder.Entity<Item>(entity =>
                  {
                        entity.HasKey(i => i.ItemsId);

                        entity.HasOne(i => i.Deck)
                        .WithMany(deck => deck.Items)
                        .HasForeignKey(i => i.DeckId)
                        .OnDelete(DeleteBehavior.Cascade);

                        entity.HasOne(i => i.Card)
                        .WithMany()
                        .HasForeignKey(i => i.CardId);
                  });
                  // GameProcess Configuration
                  modelBuilder.Entity<GameProcess>(entity =>
                  {
                        entity.HasKey(gp => gp.GameProcessId);

                        // Relacja z Process
                        entity.HasOne(gp => gp.Process)
                              .WithMany() // Process nie ma kolekcji GameProcess
                              .HasForeignKey(gp => gp.ProcessId)
                              .OnDelete(DeleteBehavior.Restrict);

                        entity.HasOne(gp => gp.Game)
                              .WithMany(g => g.GameProcesses)
                              .HasForeignKey(gp => gp.GameId);

                        entity.HasOne(gp => gp.Team)
                              .WithMany(t => t.GameProcesses)
                              .HasForeignKey(gp => gp.TeamId);
                  });
                  
                  // GameLogSpec Configuration
                  modelBuilder.Entity<GameLogSpec>(entity =>
                  {
                        entity.HasKey(gls => gls.GameLogSpecId);

                        entity.HasOne<GameLog>(gls => gls.GameLog)
                              .WithMany(gl => gl.GameLogSpecs)
                              .HasForeignKey(gls => gls.GameLogId)
                              .OnDelete(DeleteBehavior.Cascade);

                        entity.HasOne(gls => gls.GameProcess)
                              .WithMany()
                              .HasForeignKey(gls => gls.GameProcessId)
                              .OnDelete(DeleteBehavior.SetNull);
                  });

            }
    }
}