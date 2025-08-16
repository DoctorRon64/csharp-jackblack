using _Utils;
using System.Threading;

namespace BlackJack.gameStates;

internal class BetState : BaseState<Program> {

    public override void OnEnter() {
        Console.Clear();
        ShowBalance();

        int bet;
        do {
            C.Color(ConsoleColor.Cyan);
            bet = ReadInt("Enter your bet: ");
            if (bet <= 0 || bet > Blackboard.gameStats.Balance) {
                Console.Clear();
                ShowBalance();
                C.Color(ConsoleColor.Red);
                Console.WriteLine($"{bet}$ is an invalid bet. Try again.");
            }
        } while (bet <= 0 || bet > Blackboard.gameStats.Balance);

        Blackboard.gameStats.CurrentBet = bet;
        C.Color(ConsoleColor.Cyan);
        Console.Write("Your bet is: ");
        C.Color(ConsoleColor.Yellow);
        Console.WriteLine($"{bet}$");
        Task.Delay(3000);
        StateMachine.Switch<GameStartState>();
    }

    private int ReadInt(string prompt) {
        string input = "";
        ConsoleKey key;

        void DrawLine() {
            C.Color(ConsoleColor.Cyan);
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.WindowWidth)); // clear line
            Console.CursorLeft = 0;
            Console.Write(prompt);
            C.Color(ConsoleColor.Yellow);

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
        C.Color(ConsoleColor.Cyan);
        Console.Write($"Your balance: ");
        C.Color(ConsoleColor.Yellow);
        Console.WriteLine($"{Blackboard.gameStats.Balance}$");
    }
}
