
using _Utils;
using BlackJack;
using BlackJack.gameStates;
using System.ComponentModel.Design;

internal class Program {
    private StateMachine<Program> gameStateMachine;
    internal GameStats gameStats;
    internal GameCards gameCards;

    public Program()
    {
        gameStateMachine = new(this);
        gameStats = new();
        gameCards = new();

        gameStateMachine.Add<MainMenuState>();
        gameStateMachine.Add<GameBetState>();
        gameStateMachine.Add<GameStartState>();
        gameStateMachine.Add<GameChooseState>();
        gameStateMachine.Add<GameOverState>();
        gameStateMachine.Switch<MainMenuState>();
    }

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        new Program().Run();
    }

    public void Run() {  
        while(gameStateMachine.CurrentState != null) {
            gameStateMachine.OnUpdate();
        }
    }
}

internal class GameStats {
    public int Balance { get; set; } = 1000; // starting balance
    public int CurrentBet { get; set; } = 0;
}

internal class GameCards {
    internal DeckManager deck;
    internal Hand playerHand;
    internal Hand dealerHand;
    internal CardPrinter printer;

    public GameCards() {
        deck = new();
        playerHand = new();
        dealerHand = new();
        printer = new();
    }

    public void DealStartingHands() {
        deck.ShuffleDeck();
        playerHand.Clear();
        dealerHand.Clear();

        // deal 2 to player, 1 to dealer
        playerHand.AddCard(deck.Draw());
        playerHand.AddCard(deck.Draw());
        dealerHand.AddCard(deck.Draw());

        Console.Clear();
        Console.WriteLine("Your hand:");
        printer.PrintCardsSideBySide(playerHand.Cards.ToList());

        Console.WriteLine("\nDealer’s hand:");
        printer.PrintCardsSideBySide(new List<Card> { dealerHand.Cards.First() });
        Console.WriteLine("?? hidden");
    }

    public void ShowHands(bool revealDealer = false) {
        Console.Clear();
        printer.DrawDivider();
        Console.WriteLine("");

        printer.Color(ConsoleColor.Cyan);
        Console.WriteLine("Your Hand:");
        printer.PrintCardsSideBySide(playerHand.Cards.ToList());
        Console.WriteLine($"Total: {playerHand.GetBestValue()}");
        printer.DrawDivider();
        Console.WriteLine("");

        printer.Color(ConsoleColor.Red);
        Console.WriteLine("Dealer's Hand:");
        if (revealDealer) {
            printer.PrintCardsSideBySide(dealerHand.Cards.ToList());
            Console.WriteLine($"Total: {dealerHand.GetBestValue()}");
        }
        else {
            printer.PrintCardsSideBySide(new List<Card> { dealerHand.Cards.First() });
            Console.WriteLine("+ ??");
        }

        printer.DrawDivider();
        Console.ResetColor();
    }

    public void Hit(bool player = true) {
        (player ? playerHand : dealerHand).AddCard(deck.Draw());
    }
}
