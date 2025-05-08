
using backend.Data;


public static class BoardInitializer {

    public static void Initialize(AppDbContext context){
        
        // Dodawanie Przedmiotów
        var boards = new List<Board>
        {
            new Board { BoardId = 1, Name = "Plansza podstawowa", LabelsUp = "Podstawowa kordynacja;Standaryzacja procesów;Zintegrowane działania;Pełna integracja strategiczna", LabelsRight = "Nowicjusz;Naśladowca;Innowator;Lider cyfrowy", DescriptionDown = "Poziom integracji wew/zew", DescriptionLeft = "Zawansowanie Cyfrowe", Rows = 8, Cols = 8, BorderColor = "#595959", CellColor = "#fefae0", BorderColors = "#008000;#FFFF00;#FFA500;#FF0000" },
            new Board { BoardId = 2, Name = "Plansza zaawansowana", LabelsUp = "Początkowy;Rozwinięty;Zaawansowany;Ekspercki;Mistrzowski", LabelsRight = "Poziom 1;Poziom 22;Poziom 3;Poziom 4", DescriptionDown = "Etapy rozwoju kompetencji", DescriptionLeft = "Poziomy umiejętności", Rows = 8, Cols = 10, BorderColor = "#333333", CellColor = "#f5f5f5", BorderColors = "#3498db;#2ecc71;#f1c40f;#e74c3c;#9b59b6" },
            new Board { BoardId = 3, Name = "Mapa strategiczna", LabelsUp = "Mapa strategiczna;Planowanie;Implementacja;Kontrola", LabelsRight = "Strategia;Taktyka;Operacje", DescriptionDown = "Etapy zarządzania", DescriptionLeft = "Poziomy zarządzania", Rows = 6, Cols = 8, BorderColor = "#444444", CellColor = "#e0f7fa", BorderColors = "#1abc9c;#3498db;#f39c12;#e74c3c" },
        };
        foreach (var newBoard in boards)
        {
            var existingBoard = context.Boards.FirstOrDefault(b => b.BoardId == newBoard.BoardId);
            if (existingBoard != null)
            {
                // Aktualizacja danych przedmiotów
                existingBoard.BoardId = newBoard.BoardId;
                existingBoard.Name = newBoard.Name;
                existingBoard.LabelsUp = newBoard.LabelsUp;
                existingBoard.LabelsRight = newBoard.LabelsRight;
                existingBoard.DescriptionDown = newBoard.DescriptionDown;
                existingBoard.DescriptionLeft = newBoard.DescriptionLeft;
                existingBoard.Rows = newBoard.Rows;
                existingBoard.Cols = newBoard.Cols;
                existingBoard.BorderColor = newBoard.BorderColor;
                existingBoard.CellColor = newBoard.CellColor;
                existingBoard.BorderColors = newBoard.BorderColors;
            }
            else
            {
                // Dodanie nowego przedmiotu
                context.Boards.Add(newBoard);
            }
        }
        context.SaveChanges();
        Console.WriteLine("Zaktualizowano / dodano Plansze do bazy MySQL!");
    }
}
