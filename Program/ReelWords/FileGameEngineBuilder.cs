using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ReelWords;

/// <summary>
/// Builder patter to simplify the game initialization.
/// </summary>
public class FileGameEngineBuilder
{
    private IWordsStorage _wordStorage;
    private IReelsEngine _reelsEngine;
    private IScoresService _scoresService;

    public FileGameEngineBuilder WithWordStorage<T>() where T : IWordsStorage
    {
        _wordStorage = (IWordsStorage)Activator.CreateInstance(typeof(T));
        return this;
    }

    public FileGameEngineBuilder WithReelsEngine(int seed)
    {
        _reelsEngine = new ReelsEngine(new(seed));
        return this;
    }

    public FileGameEngineBuilder WithScoresService()
    {
        _scoresService = new ScoresService();
        return this;
    }

    public async Task<GameEngine> BuildAsync(CancellationToken ct = default)
    {
        // Micro-optimization: We only keep in memory the words we could achieve with the reels width.
        var possibleWordSize = 0;
        if (_reelsEngine is not null)
        {
            await foreach (var line in File.ReadLinesAsync(Path.Combine(Directory.GetCurrentDirectory(), "Data\\reels.txt"), ct))
            {
                var toInsert = line.Replace(" ", "").ToCharArray();
                _reelsEngine.Insert(toInsert);
                possibleWordSize += 1;
            }
        }

        if (_wordStorage is not null)
        {
            await foreach (var line in File.ReadLinesAsync(Path.Combine(Directory.GetCurrentDirectory(), "Data\\american-english-large.txt"), ct))
                if (line.Length <= possibleWordSize)
                    _wordStorage.Insert(line);
        }

        if (_scoresService is not null)
        {
            await foreach (var line in File.ReadLinesAsync(Path.Combine(Directory.GetCurrentDirectory(), "Data\\scores.txt"), ct))
            {
                var points = line.Split(" ");
                _scoresService.Insert(char.Parse(points[0]), int.Parse(points[1]));
            }
        }

        return new GameEngine(_wordStorage, _reelsEngine, _scoresService);
    }
}
