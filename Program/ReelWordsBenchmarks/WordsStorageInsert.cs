using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ReelWords;

namespace ReelWordsBenchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
public class WordsStorageInsert
{
    private readonly List<string> _words = [];
    private Trie _trie = new();
    private HashTrie _hash = new();

    [GlobalSetup]
    public async Task Setup()
    {
        await foreach (var line in File.ReadLinesAsync(Path.Combine(Directory.GetCurrentDirectory(), "Data\\american-english-large.txt")))
            _words.Add(line);
    }

    [IterationSetup]
    public void InitializeTries()
    {
        _trie = new();
        _hash = new();
    }

    [Benchmark(Baseline = true)]
    public void InsertingInTrie()
    {
        foreach (var word in _words)
            _trie.Insert(word);
    }

    [Benchmark]
    public void InsertingInHash()
    {
        foreach (var word in _words)
            _hash.Insert(word);
    }
}
