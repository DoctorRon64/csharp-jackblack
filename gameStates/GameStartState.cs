using _Utils;

namespace BlackJack.gameStates;
internal class GameStartState : BaseState<Program> {
    GameCards cards => Blackboard.gameCards;

    public override void OnEnter() {
        cards.DealStartingHands();

        Console.Clear();
        cards.ShowHands();
    }

    public override void OnUpdate() {
        if (!Console.KeyAvailable) return;

        var key = Console.ReadKey(true).Key;
        if (key != ConsoleKey.None) {
            StateMachine.Switch<GameChooseState>();
        }
    }

    
}