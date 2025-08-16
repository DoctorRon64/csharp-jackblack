using System.Reflection;

namespace BlackJack;

public class CardPrinter {
    public void DrawDivider() => Console.WriteLine(new string('-', 40));
    public void Color(ConsoleColor color) => Console.ForegroundColor = color;

    public void PrintCardsSideBySide(List<Card> cards) {
        // Each card has 7 lines (for example)
        string[][] lines = new string[cards.Count][];

        for (int i = 0; i < cards.Count; i++) {
            var c = cards[i];
            lines[i] = new string[]
            {
                "┌─────────┐",
                $"│{GetCardDisplayValue(c),-2}       │",
                "│         │",
                $"│    {GetSymbol(c.Type)}    │",
                "│         │",
                $"│       {GetCardDisplayValue(c),2}│",
                "└─────────┘"
            };
        }

        foreach(Card card in cards) {
            DealCardAnimation(card, true);
        }

        // Print line by line
        for (int line = 0; line < lines[0].Length; line++) {
            for (int cardIndex = 0; cardIndex < cards.Count; cardIndex++) {
                Console.Write(lines[cardIndex][line] + " ");
            }
            Console.WriteLine();
        }
    }

    private void DealCardAnimation(Card card, bool toPlayer) {
        string target = toPlayer ? "Player" : "Dealer";
        Console.Write($"{target} draws");
        for (int i = 0; i < 3; i++) {
            Thread.Sleep(300);
            Console.Write(".");
        }
        Console.WriteLine();
        Thread.Sleep(400);

        PrintCard(card);
        Thread.Sleep(600);
    }

    public void PrintCard(Card card) {
        Console.WriteLine("┌─────────┐");
        Console.WriteLine($"│{GetCardDisplayValue(card),-2}       │");  // top-left value
        Console.WriteLine("│         │");
        Console.WriteLine($"│    {GetSymbol(card.Type)}    │");  // center suit
        Console.WriteLine("│         │");
        Console.WriteLine($"│       {GetCardDisplayValue(card),2}│");  // bottom-right value
        Console.WriteLine("└─────────┘");
    }

    private string GetSymbol(CardType type) => type switch {
        CardType.Spades => "♠",
        CardType.Hearts => "♥",
        CardType.Clubs => "♣",
        CardType.Diamonds => "♦",
        _ => "?"
    };

    private string GetCardDisplayValue(Card card) {
        return card.Id switch {
            1 => "A",
            11 => "J",
            12 => "Q",
            13 => "K",
            _ => card.Id.ToString()
        };
    }
}
