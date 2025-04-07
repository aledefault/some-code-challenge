namespace ReelWords
{
    /// <summary>
    /// Saving your words in the safest place since 1990.
    /// </summary>
    public interface IWordsStorage
    {
        bool Search(string word);
        void Insert(string word);
        void Delete(string word);
    }
}