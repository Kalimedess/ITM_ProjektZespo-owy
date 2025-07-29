using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using System.Collections.Generic;

namespace backend.PdfGeneration
{
    public class CardPdfModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Application { get; set; } = string.Empty;
        public string CardType { get; set; } = "Unknown";
    }

    public class CardsDocument : IDocument
    {
        private readonly List<CardPdfModel> _cards;

        public CardsDocument(List<CardPdfModel> cards)
        {
            _cards = cards;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            foreach (var card in _cards)
            {
                container.Page(page =>
                {
                    page.Size(1676, 792, Unit.Point);
                    page.Margin(0);
                    page.Content().Element(c => ComposeCardContentSimple(c, card));
                });
            }
        }

        // Simpler alternative using absolute positioning
        private void ComposeCardContentSimple(IContainer container, CardPdfModel card)
        {
            container
                .Background(Colors.White)
                .Element(element =>
                {
                    // Create a container that will hold both circle and text
                    element.Column(mainColumn =>
                    {
                        // Create a row to position elements
                        mainColumn.Item()
                            .Row(row =>
                            {
                                // Circle section (partially hidden)
                                row.ConstantItem(475) // Only show 350px of the 750px circle
                                    .AlignMiddle()
                                    .Element(circleContainer =>
                                    {
                                        // Use padding to shift circle left (creating partial visibility)
                                        circleContainer
                                            .PaddingLeft(-392)
                                            .Width(792)
                                            .Height(792)
                                            .Svg(GenerateCircleSvg(card.Id, GetCardColor(card.CardType)));
                                    });

                                // Text section
                                row.RelativeItem()
                                    .PaddingVertical(150)
                                    .PaddingHorizontal(100)
                                    .Column(textColumn =>
                                    {
                                        textColumn.Item().DefaultTextStyle(x => x
                                            .FontColor(Colors.Black)
                                            .FontFamily(Fonts.Arial));

                                        textColumn.Spacing(35);

                                        textColumn.Item().Text(card.Title)
                                            .FontSize(50)
                                            .Bold();

                                        textColumn.Item().Text(card.Description)
                                            .FontSize(28)
                                            .LineHeight(1.4f);

                                        if (!string.IsNullOrWhiteSpace(card.Application))
                                        {
                                            textColumn.Item()
                                                .PaddingTop(-10)
                                                .Text(text =>
                                                {
                                                    text.DefaultTextStyle(x => x.FontSize(28).LineHeight(1.4f));
                                                    text.Span("Zastosowanie: ").Bold();
                                                    text.Span(card.Application);
                                                });
                                        }
                                    });
                            });
                    });
                });
        }

        private string GenerateCircleSvg(int id, string color)
        {
            var textContent = id.ToString();
            var xPosition = 75;

            return $@"
                <svg viewBox='0 0 100 100' xmlns='http://www.w3.org/2000/svg'>
                    <circle cx='50' cy='50' r='50' fill='{color}' />
                    <text x='{xPosition}%' y='60%' 
                          font-family='Arial, sans-serif' 
                          font-size='20' 
                          font-weight='bold' 
                          fill='white' 
                          text-anchor='middle' 
                          dominant-baseline='middle'>
                        {textContent}
                    </text>
                </svg>";
        }

        private string GetCardColor(string cardType)
        {
            return cardType switch
            {
                "Decision" => "#00b1eb", // Blue
                "Software" => "#009641", // Green
                "Hardware" => "#ef7d00", // Orange
                _ => "#95A5A6"           // Gray
            };
        }
    }
}