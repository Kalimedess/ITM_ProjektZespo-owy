using backend.Data;

public static class DeckInitializer {

    public static void Initialize(AppDbContext context) {

        //Dodawanie Decyzji
        var decks = new List<Deck> {
            new Deck { DeckId = 1, DeckName = "Podstawowy"}
        };

        foreach (var newDeck in decks)
        {
            var existingDeck = context.Decks.FirstOrDefault(d => d.DeckId == newDeck.DeckId);
            if (existingDeck != null)
            {
                // Aktualizacja danych użytkownika
                existingDeck.DeckId= newDeck.DeckId;
                existingDeck.DeckName = newDeck.DeckName;

            }
            else
            {
                // Dodanie nowego użytkownika
                context.Decks.Add(newDeck);
            }
        }
        context.SaveChanges();
        Console.WriteLine("Zaktualizowano / dodano Talie do bazy MySQL!");
    }
}