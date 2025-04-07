using System;
using System.Collections.Generic;
using System.Text;

namespace ReelWords;

public interface IReelsEngine
{
    /// <summary>
    /// Rotate one letter and put it at the end of the array.
    /// </summary>
    /// <param name="position">Reel position to be rotated.</param>
    /// <exception cref="ArgumentException"></exception>
    void Rotate(int position);

    /// <summary>
    /// Get the actual word.
    /// </summary>
    /// <returns></returns>
    string GetWord();

    /// <summary>
    /// Insert a new reel into the structure.
    /// </summary>
    /// <param name="reel"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public void Insert(char[] reel);
}

/// <summary>
/// "Engine" in charge of keep the data structure and act as a circular matrix. 
/// </summary>
public class ReelsEngine(Random random) : IReelsEngine
{
    private readonly List<char[]> _circularMatrix = [];
    private readonly List<int> _circularIndexes = [];
    private readonly Random _random = random ?? throw new ArgumentNullException(nameof(random));

    public void Rotate(int position)
    {
        if (position < 0)
            throw new ArgumentException($"{nameof(position)} should be greater or equal than 0.");

        if (position > _circularIndexes.Count)
            throw new ArgumentException($"{nameof(position)} should be lower or equal than {_circularIndexes.Count}.");

        _circularIndexes[position] = (_circularIndexes[position] + 1) % _circularMatrix[position].Length;
    }

    public string GetWord()
    {
        StringBuilder sb = new();
        for (int i = 0; i < _circularIndexes.Count; i += 1)
            sb.Append(_circularMatrix[i][_circularIndexes[i]]);

        return sb.ToString();
    }

    public void Insert(char[] reel)
    {
        if (reel is null)
            throw new ArgumentNullException($"{nameof(reel)}.");

        if (reel.Length <= 0)
            throw new ArgumentException($"{nameof(reel)} should have atleast one element.");

        _circularMatrix.Add(reel);
        _circularIndexes.Add(_random.Next(0, reel.Length));
    }
}
