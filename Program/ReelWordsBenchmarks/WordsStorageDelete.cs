using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ReelWords;

namespace ReelWordsBenchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
public class WordsStorageDelete
{
    private readonly Trie _trie = new();
    private readonly HashTrie _hash = new();
    private readonly List<string> _words = [];

    [GlobalSetup]
    public async Task Setup()
    {
        await foreach (var line in File.ReadLinesAsync(Path.Combine(Directory.GetCurrentDirectory(), "Data\\american-english-large.txt")))
            _words.Add(line);
    }

    [IterationSetup]
    public void RefillTries()
    {
        foreach (var word in _words)
        {
            _trie.Insert(word);
            _hash.Insert(word);
        }
    }

    [Benchmark(Baseline = true)]
    public void DeletingInTrie()
    {
        foreach (var word in _words)
            _trie.Delete(word);
    }

    [Benchmark]
    public void DeletingInHash()
    {
        foreach (var word in _words)
            _hash.Delete(word);
    }
}
