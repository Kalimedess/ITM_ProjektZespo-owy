using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class UserInitializationService : IUserInitializationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UserInitializationService> _logger; // Dodajemy logger dla lepszej diagnostyki

        public UserInitializationService(IServiceProvider serviceProvider, ILogger<UserInitializationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task InitializeUserAsync(int userId)
        {
            // Stwórz nowy, dedykowany zakres dla tej operacji
            using (var scope = _serviceProvider.CreateScope())
            {
                // Pobierz NOWĄ instancję DbContext z tego zakresu
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    _logger.LogInformation("Rozpoczynanie inicjalizacji dla użytkownika {UserId}", userId);
                    
                    int templateDeckId = 1;
                    var templateDeck = await context.Decks
                        .Include(d => d.Decisions).Include(d => d.Items)
                        .Include(d => d.Feedbacks).Include(d => d.Processes)
                        .AsSplitQuery().AsNoTracking()
                        .FirstOrDefaultAsync(d => d.DeckId == templateDeckId && d.UserId == null);

                    if (templateDeck == null)
                    {
                        _logger.LogError("BŁĄD KRYTYCZNY: Szablonowy Deck o ID {TemplateDeckId} nie został znaleziony.", templateDeckId);
                        return;
                    }

                    var newDeckForUser = new Deck { DeckName = "Talia podstawowa", UserId = userId };
                    context.Decks.Add(newDeckForUser);
                    await context.SaveChangesAsync();
                    _logger.LogInformation("Utworzono nowy deck o ID {DeckId} dla użytkownika {UserId}.", newDeckForUser.DeckId, userId);

                    CopySimpleCollections(context, templateDeck, newDeckForUser.DeckId);
                    
                    var processIdMap = await CopyProcessesAndCreateIdMapAsync(context, templateDeck, newDeckForUser.DeckId);
                    _logger.LogInformation("Pomyślnie skopiowano procesy i utworzono mapę ID.");

                    var templateDecisionWeights = await context.DecisionWeights
                        .Where(dw => dw.DeckId == templateDeckId).AsNoTracking().ToListAsync();
                    CopyDecisionWeights(context, templateDecisionWeights, newDeckForUser.DeckId, processIdMap);
                    
                    await CopyTemplateBoardsAsync(context, userId);

                    await context.SaveChangesAsync();
                    _logger.LogInformation("Pomyślnie zakończono inicjalizację danych dla użytkownika {UserId}.", userId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Nieoczekiwany błąd podczas inicjalizacji użytkownika {UserId}", userId);
                }
            }
        }

        // --- METODY POMOCNICZE (teraz są niestatyczne) ---
        
        private void CopySimpleCollections(AppDbContext context, Deck templateDeck, int newDeckId)
        {
             // Kopiuj Decisions
            if (templateDeck.Decisions != null && templateDeck.Decisions.Any())
            {
                var newDecisions = templateDeck.Decisions.Select(d => new Decision
                {
                    DeckId = newDeckId,
                    CardId = d.CardId,
                    DecisionShortDesc = d.DecisionShortDesc,
                    DecisionLongDesc = d.DecisionLongDesc,
                    DecisionBaseCost = d.DecisionBaseCost,
                    DecisionCostWeight = d.DecisionCostWeight,
                });
                context.Decisions.AddRange(newDecisions);
                Console.WriteLine($"{newDecisions.Count()} Decisions prepared for Deck {newDeckId}.");
            }

            // Kopiuj Items
            if (templateDeck.Items != null && templateDeck.Items.Any())
            {
                var newItems = templateDeck.Items.Select(i => new Item
                {
                    DeckId = newDeckId,
                    CardId = i.CardId,
                    HardwareShortDesc = i.HardwareShortDesc,
                    HardwareLongDesc = i.HardwareLongDesc,
                    ItemsBaseCost = i.ItemsBaseCost,
                    ItemsCostWeight = i.ItemsCostWeight
                });
                context.Items.AddRange(newItems);
            }

            // Kopiuj Feedbacks
            if (templateDeck.Feedbacks != null && templateDeck.Feedbacks.Any())
            {
                var newFeedbacks = templateDeck.Feedbacks.Select(f => new Feedback
                {
                    DeckId = newDeckId,
                    CardId = f.CardId,
                    Status = f.Status,
                    LongDescription = f.LongDescription,
                    FeedbackPDF = f.FeedbackPDF
                });
                context.Feedbacks.AddRange(newFeedbacks);
            }
        }

        private async Task<Dictionary<int, int>> CopyProcessesAndCreateIdMapAsync(AppDbContext context, Deck templateDeck, int newDeckId)
        {
            var processIdMap = new Dictionary<int, int>();
            if (templateDeck.Processes != null && templateDeck.Processes.Any())
            {
                Console.WriteLine($"Przygotowano {templateDeck.Processes.Count} procesów do skopiowania.");
                foreach (var processTemplate in templateDeck.Processes)
                {
                    var newProcess = new Process
                    {
                        DeckId = newDeckId,
                        ProcessDesc = processTemplate.ProcessDesc,
                        ProcessLongDesc = processTemplate.ProcessLongDesc,
                        ProcessColor = processTemplate.ProcessColor
                    };
                    context.Processes.Add(newProcess);
                    await context.SaveChangesAsync(); // Czekamy na zakończenie zapisu
                    processIdMap.Add(processTemplate.ProcessId, newProcess.ProcessId);
                }
            }
            return processIdMap;
        }

        private void CopyDecisionWeights(AppDbContext context, List<DecisionWeight> templateWeights, int newDeckId, Dictionary<int, int> processIdMap)
        {
            if (templateWeights.Any())
            {
                Console.WriteLine($"Przygotowano {templateWeights.Count} wag decyzyjnych do skopiowania.");
                foreach (var weightTemplate in templateWeights)
                {
                    if (processIdMap.TryGetValue(weightTemplate.ProcessId, out int newProcessId))
                    {
                        context.DecisionWeights.Add(new DecisionWeight
                        {
                            DeckId = newDeckId,
                            CardId = weightTemplate.CardId,
                            ProcessId = newProcessId,
                            WeightX = weightTemplate.WeightX,
                            WeightY = weightTemplate.WeightY,
                            BoosterX = weightTemplate.BoosterX,
                            BoosterY = weightTemplate.BoosterY
                        });
                    }
                    else
                    {
                        Console.WriteLine($"OSTRZEŻENIE: Pominięto DecisionWeight, ponieważ nie znaleziono zmapowanego ProcessId dla starego ID: {weightTemplate.ProcessId}.");
                    }
                }
            }
        }

        private async Task CopyTemplateBoardsAsync(AppDbContext context, int userId)
        {
            int[] templateBoardIds = { 1, 2, 3 };

            foreach (var templateBoardId in templateBoardIds)
            {
                var templateBoard = await context.Boards
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.BoardId == templateBoardId && b.UserId == null);

                if (templateBoard != null)
                {
                    context.Boards.Add(new Board
                    {
                        UserId = userId,
                        Name = templateBoard.Name,
                        LabelsUp = templateBoard.LabelsUp,
                        LabelsRight = templateBoard.LabelsRight,
                        DescriptionDown = templateBoard.DescriptionDown,
                        DescriptionLeft = templateBoard.DescriptionLeft,
                        Rows = templateBoard.Rows,
                        Cols = templateBoard.Cols,
                        BorderColor = templateBoard.BorderColor,
                        CellColor = templateBoard.CellColor,
                        BorderColors = templateBoard.BorderColors
                    });
                }
            }
        }
    }
}