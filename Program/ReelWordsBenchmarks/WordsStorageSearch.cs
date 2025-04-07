using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ReelWords;

namespace ReelWordsBenchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
public class WordsStorageSearch
{
    private readonly Trie _trie = new();
    private readonly HashTrie _hash = new();
    private readonly List<string> _words = [];

    [Params(3)]
    public int RepeatAllSearches;

    [GlobalSetup]
    public async Task Setup()
    {
        await foreach (var line in File.ReadLinesAsync(Path.Combine(Directory.GetCurrentDirectory(), "Data\\american-english-large.txt")))
        {
            _trie.Insert(line);
            _hash.Insert(line);
            _words.Add(line);
        }
    }

    [Benchmark(Baseline = true)]
    public void SearchingInTrie()
    {
        for (int i = 0; i < RepeatAllSearches; i++)
            foreach (var word in _words)
                _ = _trie.Search(word);
    }

    [Benchmark]
    public void SearchingInHash()
    {
        for(int i = 0; i < RepeatAllSearches; i++)
            foreach (var word in _words)
                _ = _hash.Search(word);
    }
}
