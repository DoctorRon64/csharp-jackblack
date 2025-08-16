using _Utils;
using System;

namespace BlackJack.gameStates;
internal class MainMenuState : BaseState<Program> {
    CardPrinter printer => Blackboard.gameCards.printer;

    public override void OnEnter() {
        Console.Clear();
        DrawMenu();
    }

    public override void OnUpdate() {
        if (!Console.KeyAvailable) return;

        var key = Console.ReadKey(true).Key;
        switch (key) {
            case ConsoleKey.S:
                Console.Clear();
                StateMachine?.Switch<GameBetState>();
                break;
            case ConsoleKey.Q:
                StateMachine?.Quit();
                break;
            default:
                Console.Clear();
                DrawMenu();
                break;
        }
    }

    public override void OnExit() {
       printer.Color(ConsoleColor.Yellow);
    }

    private void DrawMenu() {
        printer.Color(ConsoleColor.Yellow);
        const char x = '\u2588';
        Console.WriteLine("===========================================");
        Console.WriteLine($"||| {x}{x}{x} {x}   {x}{x}{x} {x}{x}{x} {x} {x} {x}{x}{x} {x}{x}{x} {x}{x}{x} {x} {x} |||");
        Console.WriteLine($"||| {x} {x} {x}   {x} {x} {x}   {x}{x}    {x} {x} {x} {x}   {x}{x}  |||");
        Console.WriteLine($"||| {x}{x}  {x}   {x}{x}{x} {x}   {x}     {x} {x}{x}{x} {x}   {x}   |||");
        Console.WriteLine($"||| {x} {x} {x}   {x} {x} {x}   {x}{x}    {x} {x} {x} {x}   {x}{x}  |||");
        Console.WriteLine($"||| {x}{x}{x} {x}{x}{x} {x} {x} {x}{x}{x} {x} {x} {x}{x}  {x} {x} {x}{x}{x} {x} {x} |||");
        Console.WriteLine("===========================================");

        printer.Color(ConsoleColor.Cyan);
        Console.WriteLine();
        Console.WriteLine();

        Console.Write($"Your Balance is: ");
        printer.Color(ConsoleColor.Yellow);
        Console.WriteLine($"{Blackboard.gameStats.Balance}$");

        printer.Color(ConsoleColor.Cyan);
        Console.Write($"Your Current Bet is: ");
        printer.Color(ConsoleColor.Yellow);
        Console.WriteLine($"{Blackboard.gameStats.CurrentBet}$");

        Console.WriteLine();
        printer.Color(ConsoleColor.Green);
        Console.WriteLine("Press [S] to Start");
        printer.Color(ConsoleColor.DarkRed);
        Console.WriteLine("Press [Q] to Quit");
        Console.WriteLine();
    }
}
