using _Utils;

namespace BlackJack.gameStates {
    internal class GameOverState : BaseState<Program> {
        CardPrinter printer => Blackboard.gameCards.printer;

        public override void OnEnter() {
            Console.Clear();
            printer.Color(ConsoleColor.Green);
            Console.WriteLine(@"
                   _____                         ____                 
                  / ____|                       / __ \                
                 | |  __  __ _ _ __ ___   ___  | |  | |_   _____ _ __ 
                 | | |_ |/ _` | '_ ` _ \ / _ \ | |  | \ \ / / _ \ '__|
                 | |__| | (_| | | | | | |  __/ | |__| |\ V /  __/ |   
                  \_____|\__,_|_| |_| |_|\___|  \____/  \_/ \___|_|   
            ");

            Console.WriteLine();
            printer.Color(ConsoleColor.Cyan);
            Console.Write("Your final balance: ");
            printer.Color(ConsoleColor.Yellow);
            Console.WriteLine($"{Blackboard.gameStats.Balance}$");
            Console.WriteLine();

            // reset current bet for safety
            Blackboard.gameStats.CurrentBet = 0;

            printer.Color(ConsoleColor.Green);
            Console.WriteLine("Press [M] to return to Main Menu");
            printer.Color(ConsoleColor.DarkRed);
            Console.WriteLine("Press [Q] to Quit");
            Console.ResetColor();
        }

        public override void OnUpdate() {
            if (!Console.KeyAvailable) return;

            var key = Console.ReadKey(true).Key;
            switch (key) {
                case ConsoleKey.M:
                    StateMachine.Switch<MainMenuState>();
                    break;
                case ConsoleKey.Q:
                    StateMachine.Quit();
                    break;
            }
        }

        public override void OnExit() {
            Console.ResetColor();
        }
    }
}
