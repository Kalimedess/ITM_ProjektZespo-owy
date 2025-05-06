using backend.Data;

public static class FeedbackInitializer {

    public static void Initialize(AppDbContext context) {

        var pdfBytes = File.ReadAllBytes("D:/Repozytoria/Projekt Zespołowy/ITM_ProjektZespo-owy/backend/Initializers/Test.pdf");
        //Dodawanie Decyzji
        var feedback = new List<Feedback> {
            new Feedback { DeckId =1, FeedbackId = 1, CardId = 23, Status = true, LongDescription = "nan", FeedbackPDF = pdfBytes},
            new Feedback { DeckId =1, FeedbackId = 2, CardId = 21, Status = false, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 3, CardId = 21, Status = false, LongDescription = "Jest zbyt wcześnie na określenie poziomu digitalizacji procesów, najpierw zastanów się nad zweryfikowaniem profilu organizacji." },
            new Feedback { DeckId =1, FeedbackId = 4, CardId = 6, Status = true, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 5, CardId = 6, Status = false, LongDescription = "Jest zbyt wcześnie na analizę konkurencji, na początku zastanów jakie obszary będą Cię interesować pod kątem własnego przedsiębiorstwa." },
            new Feedback { DeckId =1, FeedbackId = 6, CardId = 17, Status = true, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 7, CardId = 17, Status = false, LongDescription = "Jest zbyt wcześnie na przeprowadzenie analizy kompetencji. Zastanów się pod jakie umiejętności będą potrzebne dla Twoich pracowników." },
            new Feedback { DeckId =1, FeedbackId = 8, CardId = 22, Status = true, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 9, CardId = 22, Status = false, LongDescription = "brak" },
            new Feedback { DeckId =1, FeedbackId = 10, CardId = 16, Status = true, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 11, CardId = 16, Status = false, LongDescription = "Na początku zidentyfikuj jakie umiejętności będą kluczowe dla Twoich pracowników." },
            new Feedback { DeckId =1, FeedbackId = 12, CardId = 3, Status = true, LongDescription = "Prawidłowo określiłeś kluczowe procesy digitalizacij." },
            new Feedback { DeckId =1, FeedbackId = 13, CardId = 3, Status = false, LongDescription = "Jest zbyt wcześnie na określenie kluczowych procesów digitalizacji. Pomyśl na początku nad poszerzoną analizą." },
            new Feedback { DeckId =1, FeedbackId = 14, CardId = 5, Status = true, LongDescription = "Stworzyłeś plan transformacji modelu biznesowego z uwzględnieniem nowych technologii." },
            new Feedback { DeckId =1, FeedbackId = 15, CardId = 5, Status = false, LongDescription = "Jest zbyt wcześnie na stworzenie planu transformacji modelu biznesowego. Zastanów się nad identyfikacją obszarów do digitalizacji." },
            new Feedback { DeckId =1, FeedbackId = 16, CardId = 33, Status = true, LongDescription = "Zidentyfikowałeś pracowników chcących brać czynny udział w digitalizacji cyfrowej. Będą oni ambasadorami zmiany." },
            new Feedback { DeckId =1, FeedbackId = 17, CardId = 33, Status = false, LongDescription = "Jest zbyt wcześnie na zidentyfikowanie ambasadorów rewolucji cyfrowej w organizacji. Pomyśl na początku czego te rewolucja ma dotyczyć." },
            new Feedback { DeckId =1, FeedbackId = 18, CardId = 8, Status = true, LongDescription = "Stworzyłeś roadmapę digitalizacji, teraz wraz z zespołem dokładnie wiecie co po kolei zrobić aby doprowadzić do sukcesu." },
            new Feedback { DeckId =1, FeedbackId = 19, CardId = 8, Status = false, LongDescription = "Jest zbyt wcześnie na stworzenie roadmapy digitalizacji" },
            new Feedback { DeckId =1, FeedbackId = 20, CardId = 30, Status = true, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 21, CardId = 30, Status = false, LongDescription = "Zacznij od indentyfikacji procesów do któych chcesz dobrać technologie." },
            new Feedback { DeckId =1, FeedbackId = 22, CardId = 25, Status = true, LongDescription = "Zintegrowałeś prototyp z systemami które posiadałeś już wcześniej, wygląda, że całkiem nieźle się ze sobą dogadują." },
            new Feedback { DeckId =1, FeedbackId = 23, CardId = 25, Status = false, LongDescription = "Spróbowałeś zintegrować stworzony prototyp z systemami które już posiadasz, niestety wygląda, że nie do końca dobrze się ze sobą dogadują. Popracuj nad prototypem i przeprowadzeniem testów." },
            new Feedback { DeckId =1, FeedbackId = 24, CardId = 28, Status = true, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 25, CardId = 28, Status = false, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 26, CardId = 13, Status = true, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 27, CardId = 13, Status = false, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 28, CardId = 20, Status = true, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 29, CardId = 20, Status = false, LongDescription = "brak" },
            new Feedback { DeckId =1, FeedbackId = 30, CardId = 11, Status = true, LongDescription = "Opracowałeś indywidualne ścieżki szkolenia które spowodują znaczny wzrostu umiejętności pracownika w wybranym obszarze." },
            new Feedback { DeckId =1, FeedbackId = 31, CardId = 11, Status = false, LongDescription = "Opracowałeś indywidualne ścieżki szkolenia, jednak nie spowodowały one znaczącej poprawy poziomu wiedzy wybranych pracowników." },
            new Feedback { DeckId =1, FeedbackId = 32, CardId = 31, Status = true, LongDescription = "Zorganizowałeś i przeprowadziłeś szkolenie techniczne dla pracowników. Spowodowało to znaczny wzrost umiejętności w tej dziedzinie." },
            new Feedback { DeckId =1, FeedbackId = 33, CardId = 31, Status = false, LongDescription = "Zorganizowałeś szkolenie techniczne jednak żaden pracownik się na nim nie stawił." },
            new Feedback { DeckId =1, FeedbackId = 34, CardId = 18, Status = true, LongDescription = "Zorganizowałeś i przeprowadziłeś szkolenie dot. cyberbezpieczeństwa dla pracowników. Spowodowało to znaczny wzrost umiejętności w tej dziedzinie." },
            new Feedback { DeckId =1, FeedbackId = 35, CardId = 18, Status = false, LongDescription = "Zorganizowałeś szkolenie dot. cyberbezpieczeństwa jednak żaden pracownik się na nim nie stawił." },
            new Feedback { DeckId =1, FeedbackId = 36, CardId = 26, Status = true, LongDescription = "Zorganizowałeś i przeprowadziłeś szkolenie dot. zarządzenia zmianą dla pracowników. Spowodowało to znaczny wzrost umiejętności w tej dziedzinie." },
            new Feedback { DeckId =1, FeedbackId = 37, CardId = 26, Status = false, LongDescription = "Zorganizowałeś szkolenie dot. zarządzania zmianą jednak żaden pracownik się na nim nie stawił." },
            new Feedback { DeckId =1, FeedbackId = 38, CardId = 15, Status = true, LongDescription = "Zorganizowałeś i przeprowadziłeś szkolenie dot. kompetencji miękkich dla pracowników. Spowodowało to znaczny wzrost umiejętności w tej dziedzinie." },
            new Feedback { DeckId =1, FeedbackId = 39, CardId = 15, Status = false, LongDescription = "Zorganizowałeś szkolenie dot. kompetencji miękkich jednak żaden pracownik się na nim nie stawił." },
            new Feedback { DeckId =1, FeedbackId = 40, CardId = 4, Status = true, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 41, CardId = 4, Status = false, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 42, CardId = 1, Status = true, LongDescription = "Zorganizowałeś szkolenie dot. kompetencji miękkich jednak żaden pracownik się na nim nie stawił." },
            new Feedback { DeckId =1, FeedbackId = 43, CardId = 1, Status = false, LongDescription = "Stworzony przez Ciebie szczegółowy plan…. nie był wystarczająco szczegółowy a jego elementy wymagają przeszkolonych pracowników. Być może popracuj nad tym elementem" },
            new Feedback { DeckId =1, FeedbackId = 44, CardId = 10, Status = true, LongDescription = "Określiłeś budżet oraz zasoby potrzebne do przeprowadzenia wdrożenia." },
            new Feedback { DeckId =1, FeedbackId = 45, CardId = 10, Status = false, LongDescription = "Nie byłeś w stanie określić budżetu i przygotować zasobów potrzebnych do przeprowadzenia wdrożenia." },
            new Feedback { DeckId =1, FeedbackId = 46, CardId = 16, Status = true, LongDescription = "Twój zespół jest gotowy do pracy nad wdrożeniem systemu, wiedza którą zdobyli w trakcie szkoleń owocuje tym, że wiedzą co i dlaczego robią a każdy z nich ma jasno określoną role." },
            new Feedback { DeckId =1, FeedbackId = 47, CardId = 16, Status = false, LongDescription = "Twój zespół nie jest gotów do pracy nad wdrożeniem systemu. Nie posiadają wystarczającej wiedzy ani przypisanych ról w procesie." },
            new Feedback { DeckId =1, FeedbackId = 48, CardId = 32, Status = true, LongDescription = "Wielka chwila! Przeprowadzasz ostatnie testy, nerwowo zerkasz na ich wyniki, co z nich wyjdzie?" },
            new Feedback { DeckId =1, FeedbackId = 49, CardId = 32, Status = false, LongDescription = "Wielka chwila! Przeprowadzasz ostatnie testy, nerwowo zerkasz na ich wyniki, co z nich wyjdzie?" },
            new Feedback { DeckId =1, FeedbackId = 50, CardId = 38, Status = true, LongDescription = "Brawo! Skutecznie powołałeś zespół do digitalizacji." },
            new Feedback { DeckId =1, FeedbackId = 51, CardId = 38, Status = false, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 52, CardId = 9, Status = true, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 53, CardId = 9, Status = false, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 54, CardId = 19, Status = true, LongDescription = "Nerwy nie były potrzebne, test przebiegł po myśli Twojej i całego zespołu. Pamiętaj, że digitalizacji to proces który nieprzerwanie trwa, nie możesz spocząć na laurach bo konkurencja przegoni Cię przed samą linią finishu." },
            new Feedback { DeckId =1, FeedbackId = 55, CardId = 19, Status = false, LongDescription = "nan" },
            new Feedback { DeckId =1, FeedbackId = 56, CardId = 27, Status = true, LongDescription = "Udoskonalasz proces, wprowadzasz nowe rozwiązania i nieprzerwanie rozwijasz umiejętności pracowników. Skutkuje to mocną poprawą procesów i wysokim poziomem digitalizacji w przedsiębiorstwie." },
            new Feedback { DeckId =1, FeedbackId = 57, CardId = 27, Status = false, LongDescription = "Próbujesz udoskonalać proces jednak bariery wewnętrzne skutecznie to blokują. Pomyśl nad analizą danych czy zwiększeniem świadomości zespołu." },
            new Feedback { DeckId =1, FeedbackId = 58, CardId = 12, Status = true, LongDescription = "Zastosowane rozwiązania cyberbezpieczeństwa powodują, że nie masz problemu z ochroną zebranych przez Ciebie danych." },
            new Feedback { DeckId =1, FeedbackId = 59, CardId = 12, Status = false, LongDescription = "Brak rozwiniętych systemów cyberbezpieczeństwa powoduje, że zebrane dane wyciekają z firmy. Nie dość, że tracisz historię produkcyjną, potrzebujesz zatrudnić specjalistów do usunięcia awarii." },
            new Feedback { DeckId =1, FeedbackId = 60, CardId = 2, Status = true, LongDescription = "Efektywnie zarządzasz danymi a te służą Ci do optymalizacji procesów i podejmowania trafniejszych decyzji. Pamiętaj aby należycie nie chronić i nie doprowadzić do ich wycieku." },
            new Feedback { DeckId =1, FeedbackId = 61, CardId = 2, Status = false, LongDescription = "Pomimo dużej ilości danych którą zbierasz w przedsiębiorstwie nie potrafisz ich przetworzyć i wyciągnąć z nich wniosków. Jest to olbrzymia wartość i wiedza która może stać się Twoją przewagą konkurencyjną." },
            new Feedback { DeckId =1, FeedbackId = 62, CardId = 14, Status = true, LongDescription = "Dobrze przygotowane podwaliny pozwalają Ci szybko i skutecznie wprowadzać nowe technologie oraz usprawnienia w przedsiębiorstwie. Każda wprowadzona technologia powoduje, że zyskujesz przewagę nad konkurencją a Klienci postrzegają Cię jako postępową firmę." },
            new Feedback { DeckId =1, FeedbackId = 63, CardId = 14, Status = false, LongDescription = "Braki w trakcie wdrożenia zaczynają Wam coraz bardziej doskwierać. Procesy nie do końca poprawnie przygotowane nie pozwalają na szybkie i elastyczne wdrażanie nowych technologii." },
            new Feedback { DeckId =1, FeedbackId = 64, CardId = 36, Status = true, LongDescription = "Dobrze przygotowane podwaliny pozwalają Ci szybko i skutecznie wprowadzać nowe technologie oraz usprawnienia w przedsiębiorstwie. Każda wprowadzona technologia powoduje, że zyskujesz przewagę nad konkurencją a Klienci postrzegają Cię jako postępową firmę." },
            new Feedback { DeckId =1, FeedbackId = 65, CardId = 35, Status = false, LongDescription = "Braki w trakcie wdrożenia zaczynają Wam coraz bardziej doskwierać. Procesy nie do końca poprawnie przygotowane nie pozwalają na szybkie i elastyczne wdrażanie nowych technologii." },
        };
        

        foreach (var newFeedback in feedback)
        {
            var existingFeedback = context.Feedbacks
                .FirstOrDefault(f => f.FeedbackId == newFeedback.FeedbackId); // szukasz po FeedbackId
        
            if (existingFeedback != null)
            {
                // Aktualizujesz istniejący feedback
                existingFeedback.Status = newFeedback.Status;
                existingFeedback.LongDescription = newFeedback.LongDescription;
                existingFeedback.FeedbackPDF = newFeedback.FeedbackPDF;
            }
            else
            {
                context.Feedbacks.Add(newFeedback);
            }
        }
        context.SaveChanges();
        Console.WriteLine("Zaktualizowano lub dodano nowe Feedbacki!");

    }
}