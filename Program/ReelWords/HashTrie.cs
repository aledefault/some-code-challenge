using ReelWords.Extensions;
using System;
using System.Collections.Generic;

namespace ReelWords;

/// <summary>
/// Hash implementation of the Trie general tree.
/// </summary>
public class HashTrie : IWordsStorage
{
    private readonly HashSet<string> _set;

    public HashTrie() => _set = [];

    public bool Search(string word)
    {
        ValidateWord(word);
        return _set.Contains(word.NormalizeFormat());
    }

    public void Insert(string word)
    {
        ValidateWord(word);
        _set.Add(word.NormalizeFormat());
    }

    public void Delete(string word)
    {
        ValidateWord(word);
        _set.Remove(word.NormalizeFormat());
    }

    private void ValidateWord(string word)
    {
        ArgumentNullException.ThrowIfNull(word);

        if (!WordsValidator.ValidationRegex().IsMatch(word))
            throw new ArgumentException($"{word} is not a valid word.");
    }
}