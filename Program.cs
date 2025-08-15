
using _Utils;
using BlackJack.gameStates;
using System.ComponentModel.Design;

internal class Program {
    private StateMachine<Program> gameStateMachine;
    internal GameStats gameStats;

    public Program()
    {
        gameStateMachine = new(this);
        gameStats = new();
        gameStats.manager.ShuffleDeck();

        gameStateMachine.Add<MainMenuState>();
        gameStateMachine.Add<BetState>();
        gameStateMachine.Switch<MainMenuState>();
    }

    static void Main(string[] args)
    {
        new Program().Run();
    }

    public void Run() {  
        while(gameStateMachine.CurrentState != null) {
            gameStateMachine.OnUpdate();
        }
    }
}

internal class GameStats {
    public DeckManager manager { get; private set; } = new();
    public int Balance { get; set; } = 1000; // starting balance
    public int CurrentBet { get; set; } = 0;

}