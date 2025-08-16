using _Utils;
using System.Threading;

namespace BlackJack.gameStates {
    internal class GameChooseState : BaseState<Program> {
        GameCards cards => Blackboard.gameCards;
        CardPrinter printer => cards.printer;

        public override void OnEnter() {
            Console.WriteLine();
            Console.WriteLine("Choose Hit [H] or Stand [S]!");
        }

        public override void OnUpdate() {
            if (!Console.KeyAvailable) return;
            var key = Console.ReadKey(true).Key;
            switch (key) {
                case ConsoleKey.H:
                    OnHit();
                    break;
                case ConsoleKey.S:
                    OnStand();
                    break;
            }
        }

        private void OnHit() {
            cards.Hit();
            Console.Clear();

            cards.ShowHands();

            int playerValue = cards.playerHand.GetBestValue();
            if (playerValue > 21) {
                Console.WriteLine("\n💥 You busted!");
                Thread.Sleep(2000);
                StateMachine.Switch<GameOverState>();
            }
            else {
                Console.WriteLine("\nChoose Hit [H] or Stand [S]!");
            }
        }

        private void OnStand() {
            Console.Clear();
            Console.WriteLine("Dealer's turn...");
            Thread.Sleep(1000);

            // Reveal dealer's full hand
            cards.ShowHands(true);
            Thread.Sleep(1000);

            // Dealer draws until 17 or more
            while (cards.dealerHand.GetBestValue() < 17) {
                DealAnimation("Dealer");
                var drawn = cards.deck.Draw();
                cards.dealerHand.AddCard(drawn);
                printer.PrintCard(drawn);
                Thread.Sleep(700);
            }

            Console.Clear();
            cards.ShowHands(true);

            // Determine results
            int playerTotal = cards.playerHand.GetBestValue();
            int dealerTotal = cards.dealerHand.GetBestValue();
            int oldBalance = Blackboard.gameStats.Balance;

            if (dealerTotal > 21 || playerTotal > dealerTotal) {
                Console.WriteLine("\n🎉 You win!");
                Blackboard.gameStats.Balance += Blackboard.gameStats.CurrentBet;
            }
            else if (dealerTotal > playerTotal) {
                Console.WriteLine("\n😢 Dealer wins!");
                Blackboard.gameStats.Balance -= Blackboard.gameStats.CurrentBet;
            }
            else {
                Console.WriteLine("\n🤝 Push! It's a tie.");
            }

            int newBalance = Blackboard.gameStats.Balance;
            AnimateBalanceChange(oldBalance, newBalance);

            Thread.Sleep(4000);
            StateMachine.Switch<MainMenuState>();
        }

        // ===== Helper Methods =====
        private void ShowBalance() {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"Your balance: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{Blackboard.gameStats.Balance}$");
            Console.ResetColor();
        }

        private void AnimateBalanceChange(int oldBalance, int newBalance) {
            int steps = 20;
            int delta = newBalance - oldBalance;
            for (int i = 0; i <= steps; i++) {
                int val = oldBalance + delta * i / steps;
                Console.Write($"\rBalance: {val}$   ");
                Thread.Sleep(30);
            }
            Console.WriteLine();
        }

        private void DealAnimation(string who) {
            Console.Write($"{who} draws");
            for (int i = 0; i < 3; i++) {
                Thread.Sleep(300);
                Console.Write(".");
            }
            Console.WriteLine();
        }
    }
}
