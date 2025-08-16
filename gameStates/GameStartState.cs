using _Utils;

namespace BlackJack.gameStates;
internal class GameStartState : BaseState<Program> {
    GameCards cards => Blackboard.gameCards;

    public override void OnEnter() {
        cards.DealStartingHands();

        Console.Clear();
        ShowHands();
    }

    public override void OnUpdate() {
        if (!Console.KeyAvailable) return;

        var key = Console.ReadKey(true).Key;
        if (key != ConsoleKey.None) {
            StateMachine.Switch<GameStartState>();
        }
    }

    private Card DrawCard() {
        return cards.deck.Draw();
    }

    private void ShowHands(bool revealDealer = false) {
        C.Color(ConsoleColor.Cyan);
        Console.WriteLine("Your Hand:");
        PrintHand(cards.playerHand, showTotal: true);

        Console.WriteLine("");
        Console.WriteLine("");

        C.Color(ConsoleColor.Red);
        Console.WriteLine("Dealer's Hand:");
        if (revealDealer) {
            PrintHand(cards.dealerHand, showTotal: true);
        }
        else {
            cards.printer.PrintCard(cards.dealerHand.Cards.First());
            Console.WriteLine($"Total: {cards.dealerHand.GetValueByCard(0)}");
            Console.WriteLine("+ ??");
        }
        Console.ResetColor();
        Console.WriteLine();

    }
    private void PrintHand(Hand hand, bool showTotal) {
        cards.printer.PrintCardsSideBySide(hand.Cards.ToList());
        
        /*foreach (Card card in hand.Cards) {
            //printer.PrintCard(card);
        }*/

        if (showTotal) {
            Console.WriteLine($"Total: {hand.GetBestValue()}");
        }
    }
}

public class CardPrinter {
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

        // Print line by line
        for (int line = 0; line < lines[0].Length; line++) {
            for (int cardIndex = 0; cardIndex < cards.Count; cardIndex++) {
                Console.Write(lines[cardIndex][line] + " ");
            }
            Console.WriteLine();
        }
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
