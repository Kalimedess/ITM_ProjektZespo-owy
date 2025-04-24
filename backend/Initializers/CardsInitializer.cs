using backend.Data;

public static class CardInitializer {

    public static void Initialize(AppDbContext context) {

        //Dodawanie Decyzji
        var cards = new List<Card> {
            new Card { DeckId = 1, CardId = 23, CardName = "Stworzenie profilu organizacji", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 21, CardName = "Samodzielna inwentaryzacjia poziomu digitalizacji", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 6, CardName = "Analiza konkurencji pod kątem poziomu digitalizacji", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 17, CardName = "Ocena kompetencji cyfrowych", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 22, CardName = "Analiza danych operacyjnych", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 3, CardName = "Określenie kluczowych procesów digitalizacji", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 29, CardName = "Ocena procesów wewnętrznych", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 5, CardName = "Stwórz strategię transformacji modelu biznesowego", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 33, CardName = "Poszukiwanie ambasadorów zmiany wewnątrz organizacji", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 8, CardName = "Stworzenie roadmapy digitalizacji", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 30, CardName = "Dobór technologii do procesu", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 7, CardName = "Testowanie i prototypowanie", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 25, CardName = "Małoskalowa integracja systemów", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 28, CardName = "Zebranie uwag z instalacji pilotażowej", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 13, CardName = "Przygotowanie rekomendacji do wdrożenia", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 20, CardName = "Analiza potrzeb szkoleniowych - ocena umiejętności", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 11, CardName = "Opracuj indywidualne ścieżki rozwoju", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 31, CardName = "Przeprowadź szkolenie techniczne", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 18, CardName = "Przeprowadź szkolenie z zakresu cyberbezpieczeńśtwa", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 26, CardName = "Przeprowadź szkolenie z zakresu zarządzania zmianą", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 15, CardName = "Przeprowadź szkolenie z kompetencji miękkich", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 4, CardName = "Ocena efektywności szkoleń", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 1, CardName = "Stworzenie szczegółowego planu wdrożenia", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 10, CardName = "Przygotowanie budżetu i zasobów", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 16, CardName = "Przygotowanie zespołu", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 9, CardName = "Przeprowadzenie szkoleń", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 32, CardName = "Przeprowadzenie testów końcowych", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 19, CardName = "Zbieranie feedbacku", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 27, CardName = "Iteracyjne doskonalenie", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 12, CardName = "Monitorowanie bezpieczeństwa", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 2, CardName = "Zarządzanie danymi", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 14, CardName = "Rozwój technologii", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 36, CardName = "Integracja z nowymi technologiami:", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 35, CardName = "Budowanie kultury innowacyjności", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 34, CardName = "Kontynuacja edukacji.", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 24, CardName = "Stworzenie spójnego ekosystemu firmy", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 38, CardName = "Powołanie zespołu ds. digitalizacjoi", CardType = CardType.Decision},
            new Card { DeckId = 1, CardId = 37, CardName = "Stworzenie zespołu wdrożeniowego", CardType = CardType.Decision},
        };

        foreach (var newCard in cards)
        {
            var existingCard = context.Cards.FirstOrDefault(c => c.CardId == newCard.CardId);
            if (existingCard != null)
            {
                // Aktualizacja danych użytkownika
                existingCard.CardId= newCard.CardId;
                existingCard.CardName = newCard.CardName;
                existingCard.CardType = newCard.CardType;

            }
            else
            {
                // Dodanie nowego użytkownika
                context.Cards.Add(newCard);
            }
        }
        context.SaveChanges();
        Console.WriteLine("Zaktualizowano / dodano Karty do bazy MySQL!");
    }
}