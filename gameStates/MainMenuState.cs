using _Utils;
using System;

namespace BlackJack.gameStates;
internal class MainMenuState : BaseState<Program>
{
    public override void OnEnter()
    {
        Console.Clear();
        DrawMenu();
    }

    public override void OnUpdate()
    {
        if (!Console.KeyAvailable) return;

        var key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.S:
                Console.Clear();
                StateMachine?.Switch<BetState>();
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

    public override void OnExit()
    {
        C.Color(ConsoleColor.Yellow);
    }

    private void DrawMenu()
    {
        C.Color(ConsoleColor.Yellow);
        const char x = '\u2588';
        Console.WriteLine("===========================================");
        Console.WriteLine($"||| {x}{x}{x} {x}   {x}{x}{x} {x}{x}{x} {x} {x} {x}{x}{x} {x}{x}{x} {x}{x}{x} {x} {x} |||");
        Console.WriteLine($"||| {x} {x} {x}   {x} {x} {x}   {x}{x}    {x} {x} {x} {x}   {x}{x}  |||");
        Console.WriteLine($"||| {x}{x}  {x}   {x}{x}{x} {x}   {x}     {x} {x}{x}{x} {x}   {x}   |||");
        Console.WriteLine($"||| {x} {x} {x}   {x} {x} {x}   {x}{x}    {x} {x} {x} {x}   {x}{x}  |||");
        Console.WriteLine($"||| {x}{x}{x} {x}{x}{x} {x} {x} {x}{x}{x} {x} {x} {x}{x}  {x} {x} {x}{x}{x} {x} {x} |||");
        Console.WriteLine("===========================================");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine();
        Console.WriteLine();

        Console.Write($"Your Balance is: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Blackboard.gameStats.Balance}$");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"Your Current Bet is: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Blackboard.gameStats.CurrentBet}$");

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Press [S] to Start");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Press [Q] to Quit");
        Console.WriteLine();
    }
}
