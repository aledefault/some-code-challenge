using ReelWords;
using ReelWords.Extensions;
using Spectre.Console;
using System;
using System.Threading;

using CancellationTokenSource ct = new();

AppDomain.CurrentDomain.ProcessExit += (_, _) => ct.Cancel();
Console.CancelKeyPress += (_, _) => Environment.Exit(0);

GameEngine gameEngine = null;

try
{
    await AnsiConsole
        .Status()
        .Spinner(Spinner.Known.BoxBounce)
        .StartAsync(
        "Loading...",
        async ctx =>
        {
            gameEngine = await new FileGameEngineBuilder()
                .WithWordStorage<Trie>()
                .WithReelsEngine(Guid.NewGuid().GetHashCode())
                .WithScoresService()
                .BuildAsync(ct.Token);
        });
}
catch
{
    Console.WriteLine("It seems there's something wrong with your ReelWords installation... Bye!");
    Environment.Exit(0);
}

var initializing = true;
while (true)
{
    try
    {
        var grid = new Grid()
            .AddColumns(3)
            .AddRow(
            [
                new Text($"Last Score: {gameEngine.LastScore}", new Style(gameEngine.LastScore == 0 ? Color.Red : Color.Green)).LeftJustified(),
                new Rule($"[yellow]Welcome, stranger! What are you guessing?[/]").Centered(),
                new Text($"Total Score: {gameEngine.TotalScore}", new Style(Color.Green)).RightJustified()
            ])
            .AddRow([Text.Empty, new FigletText($"{gameEngine.GetWord().ToUpperInvariant()}").Centered(), Text.Empty])
            .AddRow();

        AnsiConsole.WriteLine();
        AnsiConsole.Write(grid);

        if (gameEngine.LastScore == 0 && !initializing)
            AnsiConsole.MarkupLine($"[red]That's an invalid word. Please, try again[/]!");

        if (initializing)
            initializing = false;

        var input = AnsiConsole.Ask<string>("[grey][[press ctrl+c to exit]][/] My [green]guess[/] is: ").NormalizeFormat();
        _ = gameEngine.CheckWord(input);
    }
    catch (ArgumentException)
    {
        AnsiConsole.MarkupLine($"[red]That's not a valid word. Please, try again[/]!");
    }

    AnsiConsole.Clear();
}
