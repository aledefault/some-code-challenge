using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ReelWords;

/// <summary>
/// Composite class that encapsulate logic and control the overall game.
/// </summary>
public class GameEngine(IWordsStorage wordsStorage, IReelsEngine reelsEngine, IScoresService pointsService)
{
    private readonly IWordsStorage _wordsStorage = wordsStorage;
    private readonly IReelsEngine _reelsEngine = reelsEngine;
    private readonly IScoresService _pointsService = pointsService;

    public int TotalScore { get; private set; }
    public int LastScore { get; private set; }

    /// <summary>
    /// Get the actual word to play against.
    /// </summary>
    /// <returns></returns>
    public string GetWord() => _reelsEngine.GetWord();

    /// <summary>
    /// Check if the word exist in the storage. 
    /// If exists it will return the partial score.
    /// If not exists it will return 0.
    /// In any other case it will calculate the word score.
    /// </summary>
    /// <param name="word"></param>
    /// <returns>Partial score.</returns>
    public int CheckWord(string word)
    {
        var score = 0;
        if (_wordsStorage.Search(word))
        {
            var toRotate = CalculateRotations(word);
            if (toRotate is not null)
            {
                RotateReels(toRotate);
                score = CalculateScore(toRotate);
            }
            else
            {
                score = 0;
            }
        }

        LastScore = score;
        TotalScore += score;
        return LastScore;
    }

    private List<int> CalculateRotations(string wordToCheck)
    {
        var indexes = new Dictionary<char, Queue<int>>();
        int index = 0;
        foreach (var letter in _reelsEngine.GetWord())
        {
            if (!indexes.TryGetValue(letter, out var queue))
            {
                queue = new Queue<int>();
                indexes.Add(letter, queue);
            }
            
            queue.Enqueue(index++);
        }

        var toRotate = new List<int>();
        foreach (var letter in wordToCheck)
        {
            if (!indexes.TryGetValue(letter, out var queue))
                return null;

            toRotate.Add(queue.Dequeue());

            if (queue.Count == 0)
                indexes.Remove(letter);
        }

        return toRotate;
    }

    private int CalculateScore(List<int> toRotate)
    {
        var result = 0;
        var reelWord = _reelsEngine.GetWord();
        foreach (var i in toRotate)
            result += _pointsService.HowManyPointsFor(reelWord[i]);

        return result;
    }

    private void RotateReels(List<int> toRotate)
    {
        foreach (var i in toRotate)
            _reelsEngine.Rotate(i);
    }
}
