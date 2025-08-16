using _Utils;
using System.Threading;

namespace BlackJack.gameStates;

internal class GameBetState : BaseState<Program> {
    CardPrinter printer => Blackboard.gameCards.printer;

    public override void OnEnter() {
        Console.Clear();
        ShowBalance();

        int bet;
        do {
            printer.Color(ConsoleColor.Cyan);
            bet = ReadInt("Enter your bet: ");
            if (bet <= 0 || bet > Blackboard.gameStats.Balance) {
                Console.Clear();
                ShowBalance();
                printer.Color(ConsoleColor.Red);
                Console.WriteLine($"{bet}$ is an invalid bet. Try again.");
            }
        } while (bet <= 0 || bet > Blackboard.gameStats.Balance);

        Blackboard.gameStats.CurrentBet = bet;
        printer.Color(ConsoleColor.Cyan);
        Console.Write("Your bet is: ");
        printer.Color(ConsoleColor.Yellow);
        Console.WriteLine($"{bet}$");
        Task.Delay(3000);
        StateMachine.Switch<GameStartState>();
    }

    private int ReadInt(string prompt) {
        string input = "";
        ConsoleKey key;

        void DrawLine() {
            printer.Color(ConsoleColor.Cyan);
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.WindowWidth)); // clear line
            Console.CursorLeft = 0;
            Console.Write(prompt);
            printer.Color(ConsoleColor.Yellow);

            Console.Write(input);
            Console.Write("$"); // always show $ after input
            Console.CursorLeft -= 1;
        }
        DrawLine();

        do {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            if (char.IsDigit(keyInfo.KeyChar)) {
                input += keyInfo.KeyChar;
                DrawLine();
            }
            else if (key == ConsoleKey.Backspace && input.Length > 0) {
                input = input[0..^1];
                DrawLine();
            }
            // ignore all other keys
        } while (key != ConsoleKey.Enter);

        Console.WriteLine(); // move to next line
        return int.TryParse(input, out int value) ? value : 0;
    }

    private void ShowBalance() {
        printer.Color(ConsoleColor.Cyan);
        Console.Write($"Your balance: ");
        printer.Color(ConsoleColor.Yellow);
        Console.WriteLine($"{Blackboard.gameStats.Balance}$");
    }
}
