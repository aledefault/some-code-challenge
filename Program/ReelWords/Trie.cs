using ReelWords.Extensions;
using System;
using System.Collections.Generic;

namespace ReelWords;

public class Trie : IWordsStorage
{
    private const char Init = '^';
    private const char End = '=';

    private readonly Dictionary<char, Trie> _characters;

    public Trie() => _characters = []; 

    public bool Search(string word)
    {
        ValidateWord(word);
        return SearchChild(Init + word.NormalizeFormat() + End);
    }

    private bool SearchChild(string word)
    {
        if (word.Length == 0)
            return true;

        return _characters.TryGetValue(word[0], out var trie) && trie.SearchChild(word[1..]);
    }

    public void Insert(string word)
    {
        ValidateWord(word);
        InsertChild(Init + word.NormalizeFormat() + End);
    }

    private void InsertChild(string word)
    {
        if (word.Length == 0)
            return;

        if (!_characters.TryGetValue(word[0], out var node))
        {
            node = new Trie();
            _characters.Add(word[0], node);
        }

        node.InsertChild(word[1..]);
    }

    public void Delete(string word)
    {
        ValidateWord(word);
        DeleteChild(Init + word.NormalizeFormat() + End);
    }

    private bool DeleteChild(string word)
    {
        if (word.Length == 0)
            return true;

        if (!_characters.TryGetValue(word[0], out var node))
            return false;

        return node.DeleteChild(word[1..]) && (node._characters.Count != 0 || _characters.Remove(word[0]));
    }

    private static void ValidateWord(string word)
    {
        ArgumentNullException.ThrowIfNull(word);

        if (!WordsValidator.ValidationRegex().IsMatch(word))
            throw new ArgumentException($"{word} is not a valid word.");
    }
}
