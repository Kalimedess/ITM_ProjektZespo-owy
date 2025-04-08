using backend.Data;

public static class FeedbackInitializer {

    public static void Initialize(AppDbContext context) {

        //Dodawanie Decyzji
        var feedback = new List<Feedback> {
            new Feedback { FeedbackId = 1, DecisionId = 23, Status = "P", LongDescription = "nan" },
            new Feedback { FeedbackId = 2, DecisionId = 21, Status = "P", LongDescription = "nan" },
            new Feedback { FeedbackId = 3, DecisionId = 21, Status = "N", LongDescription = "Jest zbyt wcześnie na określenie poziomu digitalizacji procesów, najpierw zastanów się nad zweryfikowaniem profilu organizacji." },
            new Feedback { FeedbackId = 4, DecisionId = 6, Status = "P", LongDescription = "nan" },
            new Feedback { FeedbackId = 5, DecisionId = 6, Status = "N", LongDescription = "Jest zbyt wcześnie na analizę konkurencji, na początku zastanów jakie obszary będą Cię interesować pod kątem własnego przedsiębiorstwa." },
            new Feedback { FeedbackId = 6, DecisionId = 17, Status = "P", LongDescription = "nan" },
            new Feedback { FeedbackId = 7, DecisionId = 17, Status = "N", LongDescription = "Jest zbyt wcześnie na przeprowadzenie analizy kompetencji. Zastanów się pod jakie umiejętności będą potrzebne dla Twoich pracowników." },
            new Feedback { FeedbackId = 8, DecisionId = 22, Status = "P", LongDescription = "nan" },
            new Feedback { FeedbackId = 9, DecisionId = 22, Status = "N", LongDescription = "brak" },
            new Feedback { FeedbackId = 10, DecisionId = 16, Status = "P", LongDescription = "nan" },
            new Feedback { FeedbackId = 11, DecisionId = 16, Status = "N", LongDescription = "Na początku zidentyfikuj jakie umiejętności będą kluczowe dla Twoich pracowników." },
            new Feedback { FeedbackId = 12, DecisionId = 3, Status = "P", LongDescription = "Prawidłowo określiłeś kluczowe procesy digitalizacij." },
            new Feedback { FeedbackId = 13, DecisionId = 3, Status = "N", LongDescription = "Jest zbyt wcześnie na określenie kluczowych procesów digitalizacji. Pomyśl na początku nad poszerzoną analizą." },
            new Feedback { FeedbackId = 14, DecisionId = 5, Status = "P", LongDescription = "Stworzyłeś plan transformacji modelu biznesowego z uwzględnieniem nowych technologii." },
            new Feedback { FeedbackId = 15, DecisionId = 5, Status = "N", LongDescription = "Jest zbyt wcześnie na stworzenie planu transformacji modelu biznesowego. Zastanów się nad identyfikacją obszarów do digitalizacji." },
            new Feedback { FeedbackId = 16, DecisionId = 33, Status = "P", LongDescription = "Zidentyfikowałeś pracowników chcących brać czynny udział w digitalizacji cyfrowej. Będą oni ambasadorami zmiany." },
            new Feedback { FeedbackId = 17, DecisionId = 33, Status = "N", LongDescription = "Jest zbyt wcześnie na zidentyfikowanie ambasadorów rewolucji cyfrowej w organizacji. Pomyśl na początku czego te rewolucja ma dotyczyć." },
            new Feedback { FeedbackId = 18, DecisionId = 8, Status = "P", LongDescription = "Stworzyłeś roadmapę digitalizacji, teraz wraz z zespołem dokładnie wiecie co po kolei zrobić aby doprowadzić do sukcesu." },
            new Feedback { FeedbackId = 19, DecisionId = 8, Status = "N", LongDescription = "Jest zbyt wcześnie na stworzenie roadmapy digitalizacji" },
            new Feedback { FeedbackId = 20, DecisionId = 30, Status = "P", LongDescription = "nan" },
            new Feedback { FeedbackId = 21, DecisionId = 30, Status = "N", LongDescription = "Zacznij od indentyfikacji procesów do któych chcesz dobrać technologie." },
            new Feedback { FeedbackId = 22, DecisionId = 25, Status = "P", LongDescription = "Zintegrowałeś prototyp z systemami które posiadałeś już wcześniej, wygląda, że całkiem nieźle się ze sobą dogadują." },
            new Feedback { FeedbackId = 23, DecisionId = 25, Status = "N", LongDescription = "Spróbowałeś zintegrować stworzony prototyp z systemami które już posiadasz, niestety wygląda, że nie do końca dobrze się ze sobą dogadują. Popracuj nad prototypem i przeprowadzeniem testów." },
            new Feedback { FeedbackId = 24, DecisionId = 28, Status = "P", LongDescription = "nan" },
            new Feedback { FeedbackId = 25, DecisionId = 28, Status = "N", LongDescription = "nan" },
            new Feedback { FeedbackId = 26, DecisionId = 13, Status = "P", LongDescription = "nan" },
            new Feedback { FeedbackId = 27, DecisionId = 13, Status = "N", LongDescription = "nan" },
            new Feedback { FeedbackId = 28, DecisionId = 20, Status = "P", LongDescription = "nan" },
            new Feedback { FeedbackId = 29, DecisionId = 20, Status = "N", LongDescription = "brak" },
            new Feedback { FeedbackId = 30, DecisionId = 11, Status = "P", LongDescription = "Opracowałeś indywidualne ścieżki szkolenia które spowodują znaczny wzrostu umiejętności pracownika w wybranym obszarze." },
            new Feedback { FeedbackId = 31, DecisionId = 11, Status = "N", LongDescription = "Opracowałeś indywidualne ścieżki szkolenia, jednak nie spowodowały one znaczącej poprawy poziomu wiedzy wybranych pracowników." },
            new Feedback { FeedbackId = 32, DecisionId = 31, Status = "P", LongDescription = "Zorganizowałeś i przeprowadziłeś szkolenie techniczne dla pracowników. Spowodowało to znaczny wzrost umiejętności w tej dziedzinie." },
            new Feedback { FeedbackId = 33, DecisionId = 31, Status = "N", LongDescription = "Zorganizowałeś szkolenie techniczne jednak żaden pracownik się na nim nie stawił." },
            new Feedback { FeedbackId = 34, DecisionId = 18, Status = "P", LongDescription = "Zorganizowałeś i przeprowadziłeś szkolenie dot. cyberbezpieczeństwa dla pracowników. Spowodowało to znaczny wzrost umiejętności w tej dziedzinie." },
            new Feedback { FeedbackId = 35, DecisionId = 18, Status = "N", LongDescription = "Zorganizowałeś szkolenie dot. cyberbezpieczeństwa jednak żaden pracownik się na nim nie stawił." },
            new Feedback { FeedbackId = 36, DecisionId = 26, Status = "P", LongDescription = "Zorganizowałeś i przeprowadziłeś szkolenie dot. zarządzenia zmianą dla pracowników. Spowodowało to znaczny wzrost umiejętności w tej dziedzinie." },
            new Feedback { FeedbackId = 37, DecisionId = 26, Status = "N", LongDescription = "Zorganizowałeś szkolenie dot. zarządzania zmianą jednak żaden pracownik się na nim nie stawił." },
            new Feedback { FeedbackId = 38, DecisionId = 15, Status = "P", LongDescription = "Zorganizowałeś i przeprowadziłeś szkolenie dot. kompetencji miękkich dla pracowników. Spowodowało to znaczny wzrost umiejętności w tej dziedzinie." },
            new Feedback { FeedbackId = 39, DecisionId = 15, Status = "N", LongDescription = "Zorganizowałeś szkolenie dot. kompetencji miękkich jednak żaden pracownik się na nim nie stawił." },
            new Feedback { FeedbackId = 40, DecisionId = 4, Status = "P", LongDescription = "nan" },
            new Feedback { FeedbackId = 41, DecisionId = 4, Status = "N", LongDescription = "nan" },
            new Feedback { FeedbackId = 42, DecisionId = 1, Status = "P", LongDescription = "Zorganizowałeś szkolenie dot. kompetencji miękkich jednak żaden pracownik się na nim nie stawił." },
            new Feedback { FeedbackId = 43, DecisionId = 1, Status = "N", LongDescription = "Stworzony przez Ciebie szczegółowy plan…. nie był wystarczająco szczegółowy a jego elementy wymagają przeszkolonych pracowników. Być może popracuj nad tym elementem" },
            new Feedback { FeedbackId = 44, DecisionId = 10, Status = "P", LongDescription = "Określiłeś budżet oraz zasoby potrzebne do przeprowadzenia wdrożenia." },
            new Feedback { FeedbackId = 45, DecisionId = 10, Status = "N", LongDescription = "Nie byłeś w stanie określić budżetu i przygotować zasobów potrzebnych do przeprowadzenia wdrożenia." },
            new Feedback { FeedbackId = 46, DecisionId = 16, Status = "P", LongDescription = "Twój zespół jest gotowy do pracy nad wdrożeniem systemu, wiedza którą zdobyli w trakcie szkoleń owocuje tym, że wiedzą co i dlaczego robią a każdy z nich ma jasno określoną role." },
            new Feedback { FeedbackId = 47, DecisionId = 16, Status = "N", LongDescription = "Twój zespół nie jest gotów do pracy nad wdrożeniem systemu. Nie posiadają wystarczającej wiedzy ani przypisanych ról w procesie." },
            new Feedback { FeedbackId = 48, DecisionId = 32, Status = "P", LongDescription = "Wielka chwila! Przeprowadzasz ostatnie testy, nerwowo zerkasz na ich wyniki, co z nich wyjdzie?" },
            new Feedback { FeedbackId = 49, DecisionId = 32, Status = "N", LongDescription = "Wielka chwila! Przeprowadzasz ostatnie testy, nerwowo zerkasz na ich wyniki, co z nich wyjdzie?" },
            new Feedback { FeedbackId = 50, DecisionId = 38, Status = "P", LongDescription = "Brawo! Skutecznie powołałeś zespół do digitalizacji." },
            new Feedback { FeedbackId = 51, DecisionId = 38, Status = "N", LongDescription = "nan" },
            new Feedback { FeedbackId = 52, DecisionId = 9, Status = "P", LongDescription = "nan" },
            new Feedback { FeedbackId = 53, DecisionId = 9, Status = "N", LongDescription = "nan" },
            new Feedback { FeedbackId = 54, DecisionId = 19, Status = "P", LongDescription = "Nerwy nie były potrzebne, test przebiegł po myśli Twojej i całego zespołu. Pamiętaj, że digitalizacji to proces który nieprzerwanie trwa, nie możesz spocząć na laurach bo konkurencja przegoni Cię przed samą linią finishu." },
            new Feedback { FeedbackId = 55, DecisionId = 19, Status = "N", LongDescription = "nan" },
            new Feedback { FeedbackId = 56, DecisionId = 27, Status = "P", LongDescription = "Udoskonalasz proces, wprowadzasz nowe rozwiązania i nieprzerwanie rozwijasz umiejętności pracowników. Skutkuje to mocną poprawą procesów i wysokim poziomem digitalizacji w przedsiębiorstwie." },
            new Feedback { FeedbackId = 57, DecisionId = 27, Status = "N", LongDescription = "Próbujesz udoskonalać proces jednak bariery wewnętrzne skutecznie to blokują. Pomyśl nad analizą danych czy zwiększeniem świadomości zespołu." },
            new Feedback { FeedbackId = 58, DecisionId = 12, Status = "P", LongDescription = "Zastosowane rozwiązania cyberbezpieczeństwa powodują, że nie masz problemu z ochroną zebranych przez Ciebie danych." },
            new Feedback { FeedbackId = 59, DecisionId = 12, Status = "N", LongDescription = "Brak rozwiniętych systemów cyberbezpieczeństwa powoduje, że zebrane dane wyciekają z firmy. Nie dość, że tracisz historię produkcyjną, potrzebujesz zatrudnić specjalistów do usunięcia awarii." },
            new Feedback { FeedbackId = 60, DecisionId = 2, Status = "P", LongDescription = "Efektywnie zarządzasz danymi a te służą Ci do optymalizacji procesów i podejmowania trafniejszych decyzji. Pamiętaj aby należycie nie chronić i nie doprowadzić do ich wycieku." },
            new Feedback { FeedbackId = 61, DecisionId = 2, Status = "N", LongDescription = "Pomimo dużej ilości danych którą zbierasz w przedsiębiorstwie nie potrafisz ich przetworzyć i wyciągnąć z nich wniosków. Jest to olbrzymia wartość i wiedza która może stać się Twoją przewagą konkurencyjną." },
            new Feedback { FeedbackId = 62, DecisionId = 14, Status = "P", LongDescription = "Dobrze przygotowane podwaliny pozwalają Ci szybko i skutecznie wprowadzać nowe technologie oraz usprawnienia w przedsiębiorstwie. Każda wprowadzona technologia powoduje, że zyskujesz przewagę nad konkurencją a Klienci postrzegają Cię jako postępową firmę." },
            new Feedback { FeedbackId = 63, DecisionId = 14, Status = "N", LongDescription = "Braki w trakcie wdrożenia zaczynają Wam coraz bardziej doskwierać. Procesy nie do końca poprawnie przygotowane nie pozwalają na szybkie i elastyczne wdrażanie nowych technologii." },
            new Feedback { FeedbackId = 64, DecisionId = 36, Status = "P", LongDescription = "Dobrze przygotowane podwaliny pozwalają Ci szybko i skutecznie wprowadzać nowe technologie oraz usprawnienia w przedsiębiorstwie. Każda wprowadzona technologia powoduje, że zyskujesz przewagę nad konkurencją a Klienci postrzegają Cię jako postępową firmę." },
            new Feedback { FeedbackId = 65, DecisionId = 35, Status = "N", LongDescription = "Braki w trakcie wdrożenia zaczynają Wam coraz bardziej doskwierać. Procesy nie do końca poprawnie przygotowane nie pozwalają na szybkie i elastyczne wdrażanie nowych technologii." },
        };

        foreach (var newFeedback in feedback)
        {
            var existingFeedback= context.Feedbacks.FirstOrDefault(f => f.DecisionId == newFeedback.DecisionId);
            if (existingFeedback != null)
            {
                // Aktualizacja danych użytkownika
                existingFeedback.FeedbackId = newFeedback.FeedbackId;
                existingFeedback.DecisionId = newFeedback.DecisionId;
                existingFeedback.Status = newFeedback.Status;
                existingFeedback.LongDescription = newFeedback.LongDescription;
            }
            else
            {
                // Dodanie nowego użytkownika
                context.Feedbacks.Add(newFeedback);
            }
        }
        context.SaveChanges();
        Console.WriteLine("Zaktualizowano / dodano Odpowiedzi Decyzji do bazy MySQL!");
    }
}