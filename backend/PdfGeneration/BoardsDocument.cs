using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using backend.Data;

namespace backend.PdfGeneration
{
    public class BoardDrawingConfig
    {
        public float CellSize { get; } = 30;
        public float RowLabelWidth { get; } = 30;
        public float ColumnLabelHeight { get; } = 20;
        public float TopLabelHeight { get; } = 20;
        public float RightLabelWidth { get; } = 30;
        public float BorderWidth { get; } = 4;
        public int Rows { get; }
        public int Cols { get; }
        public string BorderColors { get; } = string.Empty;
        public string BorderColor { get; }
        public string CellColor { get; }

        public BoardDrawingConfig(Board board)
        {
            Rows = board.Rows;
            Cols = board.Cols;
            BorderColor = board.BorderColor;
            CellColor = board.CellColor;
            BorderColors = board.BorderColors;
        }
    }

    public class BoardsDocument : IDocument
    {
        private readonly List<Board> _boardsToRender;

        public BoardsDocument(List<Board> allBoards, int teamBoardId, int rivalBoardId)
        {
            _boardsToRender = new List<Board>();

            var rivalBoard = allBoards.FirstOrDefault(b => b.BoardId == rivalBoardId);
            if (rivalBoard != null) _boardsToRender.Add(rivalBoard);

            var teamBoard = allBoards.FirstOrDefault(b => b.BoardId == teamBoardId);
            if (teamBoard != null) _boardsToRender.Add(teamBoard);

            var otherBoards = allBoards.Where(b => b.BoardId != teamBoardId && b.BoardId != rivalBoardId);
            _boardsToRender.AddRange(otherBoards);
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            // Przetwarzaj plansze w parach, aby umieścić dwie na jednej stronie
            for (int i = 0; i < _boardsToRender.Count; i += 2)
            {
                var firstBoardInPair = _boardsToRender[i];
                // Sprawdź, czy istnieje druga plansza w parze
                var secondBoardInPair = (i + 1 < _boardsToRender.Count) ? _boardsToRender[i + 1] : null;

                container.Page(page =>
                {
                    page.Size(2000, 2000, Unit.Point);
                    page.Margin(50);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial));

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        // --- WIERSZ 1 ---

                        // Komórka (1,1) - lewy górny róg -> pusta
                        table.Cell();

                        // Komórka (1,2) - prawy górny róg -> pierwsza plansza
                        // DODANO .AlignCenter() i .AlignMiddle() aby wyśrodkować planszę w komórce
                        table.Cell()
                            .AlignCenter()
                            .AlignMiddle()
                            .PaddingLeft(-150)
                            .PaddingBottom(-90)
                            .Element(c => ComposeBoard(c, firstBoardInPair));

                        // --- WIERSZ 2 ---

                        // Komórka (2,1) - lewy dolny róg -> druga plansza (jeśli istnieje)
                        // DODANO .AlignCenter() i .AlignMiddle()
                        table.Cell()
                            .AlignCenter()
                            .AlignMiddle()
                            .PaddingRight(-150)
                            .PaddingTop(-90)
                            .Element(c =>
                            {
                                if (secondBoardInPair != null)
                                {
                                    ComposeBoard(c, secondBoardInPair);
                                }
                            });

                        // Komórka (2,2) - prawy dolny róg -> pusta
                        table.Cell();
                    });
                });
            }
        }

        private void ComposeBoard(IContainer container, Board boardData)
        {
            var config = new BoardDrawingConfig(boardData);
            
            container.Scale(3.2f).Column(mainColumn =>
            {
                mainColumn.Spacing(5);

                // Item 1: Cała plansza z lewym opisem
                mainColumn.Item()
                .AlignCenter()
                .Row(boardRow =>
                {
                    boardRow.Spacing(5);

                    // Lewy, pionowy opis osi
                    if (!string.IsNullOrEmpty(boardData.DescriptionLeft))
                    {
                        boardRow.AutoItem()
                            .RotateLeft()
                            .AlignCenter()
                            .Text(boardData.DescriptionLeft)
                            .FontSize(14).Bold();
                    }

                    // Kontener na całą wizualną część planszy
                    boardRow.AutoItem().Column(boardColumn =>
                    {
                        // Etykiety górne
                        if (!string.IsNullOrEmpty(boardData.LabelsUp))
                        {
                            boardColumn.Item().Element(c => DrawTopLabels(c, config, boardData.LabelsUp));
                        }

                        // Górna ramka
                        boardColumn.Item().AlignCenter().Element(c => DrawHorizontalBorder(c, config));

                        // *** KLUCZOWA POPRAWKA: JEDEN WIERSZ DLA CAŁEJ ŚRODKOWEJ CZĘŚCI ***
                        boardColumn.Item().Row(coreRow =>
                        {
                            

                            // Numery wierszy (1-8)
                            coreRow.ConstantItem(config.RowLabelWidth).Column(numbers =>
                            {
                                for (int r = 0; r < config.Rows; r++)
                                {
                                    numbers.Item().Height(config.CellSize).AlignCenter().AlignMiddle()
                                           .Text((config.Rows - r).ToString());
                                }
                            });

                            // Lewa ramka pionowa
                            coreRow.AutoItem().Element(c => DrawVerticalBorder(c, config));

                            // Główna siatka
                            coreRow.ConstantItem(config.Cols * config.CellSize).Table(table =>
                            {
                                table.ColumnsDefinition(cols =>
                                {
                                    for (int i = 0; i < config.Cols; i++)
                                        cols.ConstantColumn(config.CellSize);
                                });
                                for (int r = 0; r < config.Rows; r++)
                                for (int c = 0; c < config.Cols; c++)
                                    table.Cell().Row((uint)r + 1).Column((uint)c + 1)
                                         .Border(0.5f).BorderColor(config.BorderColor)
                                         .Background(config.CellColor).Height(config.CellSize);
                            });

                            // Prawa ramka pionowa
                            coreRow.AutoItem().Element(c => DrawVerticalBorder(c, config));

                            // Prawe etykiety (Nowicjusz, etc.)
                            if (!string.IsNullOrEmpty(boardData.LabelsRight))
                            {
                                coreRow.ConstantItem(config.RightLabelWidth).Column(rightLabels =>
                                {
                                    var labels = boardData.LabelsRight.Split(';');
                                    for (int i = 0; i < config.Rows; i += 2)
                                    {
                                        var labelIndex = i / 2;
                                        var item = rightLabels.Item().Height(config.CellSize * 2);
                                        if (labelIndex < labels.Length)
                                        {
                                            item.AlignCenter().AlignMiddle().RotateLeft()
                                                .Text(labels[labelIndex].Trim()).FontSize(8);
                                        }
                                    }
                                });
                            }
                            
                            
                        });

                        // Dolna ramka
                        boardColumn.Item().AlignCenter().Element(c => DrawHorizontalBorder(c, config));

                        // Dolne etykiety (A, B, C...)
                        boardColumn.Item().Element(c => DrawBottomLabels(c, config));
                    });
                });

                // Item 2: Dolny opis osi
                if (!string.IsNullOrEmpty(boardData.DescriptionDown))
                {
                    mainColumn.Item().AlignCenter().Text(boardData.DescriptionDown).FontSize(14).Bold();
                }
            });
        }
        
        // --- UPROSZCZONE METODY POMOCNICZE ---

        private void DrawTopLabels(IContainer container, BoardDrawingConfig config, string labelsUp)
        {
            container.Row(row =>
            {
                // Puste miejsce na lewą ramkę i numery wierszy
                row.ConstantItem(config.BorderWidth + config.RowLabelWidth);
                
                var labels = labelsUp.Split(';');
                for (int i = 0; i < config.Cols; i += 2)
                {
                    var labelIndex = i / 2;
                    var item = row.ConstantItem(config.CellSize * 2).Height(config.TopLabelHeight);
                    if (labelIndex < labels.Length)
                    {
                        item.AlignCenter().Text(labels[labelIndex].Trim()).FontSize(8);
                    }
                }
            });
        }
        
        private void DrawBottomLabels(IContainer container, BoardDrawingConfig config)
        {
            container.Row(row =>
            {
                // Puste miejsce na lewą ramkę i numery wierszy
                row.ConstantItem(config.BorderWidth + config.RowLabelWidth);
                
                for (int i = 0; i < config.Cols; i++)
                {
                    row.ConstantItem(config.CellSize).Height(config.ColumnLabelHeight)
                       .AlignCenter().Text(((char)('A' + i)).ToString());
                }
            });
        }

        private void DrawHorizontalBorder(IContainer container, BoardDrawingConfig config)
        {
            // ZMIANA: Parsujemy string z kolorami na tablicę
            if (string.IsNullOrEmpty(config.BorderColors)) return;
            var colors = config.BorderColors.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (colors.Length == 0) return;

            container.Height(config.BorderWidth).Row(row =>
            {
                row.ConstantItem(config.RowLabelWidth);
                for (int i = 0; i < config.Cols; i += 2)
                {
                    // Używamy sparsowanej tablicy 'colors'
                    var colorIndex = (i / 2) % colors.Length;
                    row.ConstantItem(config.CellSize * 2).Background(colors[colorIndex]);
                }
                row.ConstantItem(config.RightLabelWidth);
            });
        }
        
        private void DrawVerticalBorder(IContainer container, BoardDrawingConfig config)
        {
            // ZMIANA: Parsujemy string z kolorami na tablicę
            if (string.IsNullOrEmpty(config.BorderColors)) return;
            var colors = config.BorderColors.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (colors.Length == 0) return;

            container.Width(config.BorderWidth).Column(column =>
            {
                for (int i = config.Rows - 2; i >= 0; i -= 2)
                {
                    // Używamy sparsowanej tablicy 'colors'
                    var colorIndex = (i / 2) % colors.Length;
                    column.Item().Height(config.CellSize * 2).Background(colors[colorIndex]);
                }
            });
        }
    }
}