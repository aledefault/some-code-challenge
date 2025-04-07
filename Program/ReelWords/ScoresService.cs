using System.Collections;

namespace ReelWords;

/// <summary>
/// Service that encapsulate the score for each letter.
/// </summary>
public interface IScoresService
{
    void Insert(char letter, int score);
    int HowManyPointsFor(char c);
}

public class ScoresService() : IScoresService
{
    public Hashtable _pointPerLetter = [];

    public int HowManyPointsFor(char letter)
    {
        var c = char.ToLowerInvariant(letter);
        return _pointPerLetter.ContainsKey(c)
            ? (int)_pointPerLetter[c]
            : 0;
    }

    public void Insert(char letter, int score)
    {
        var c = char.ToLowerInvariant(letter);
        if (!_pointPerLetter.ContainsKey(c))
            _pointPerLetter.Add(c, score);
    }
}
