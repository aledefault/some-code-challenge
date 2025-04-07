﻿using FluentAssertions;
using ReelWords;
using System;
using System.Collections.Generic;
using Xunit;

namespace ReelWordsTests;

public class TrieTests
{
    [Fact]
    public void Should_return_a_null_exception_if_a_null_is_searched()
    {
        Trie trie = new();
        var search = () => trie.Search(null);
        search.Should().ThrowExactly<ArgumentNullException>();
    }

    [Theory]
    [InlineData("^")]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("{")]
    public void Should_return_an_exception_if_an_invalid_character_is_searched(string invalidWord)
    {
        Trie trie = new();
        var search = () => trie.Search(invalidWord);
        search.Should().ThrowExactly<ArgumentException>().WithMessage("*is not a valid word*");
    }

    [Theory]
    [MemberData(nameof(GenerateValidWords))]
    public void Should_return_true_when_searches_a_word_and_it_exists(string word, Trie trie) =>
        trie.Search(word).Should().BeTrue();

    public static IEnumerable<object[]> GenerateValidWords()
    {

        var trie = new Trie();
        trie.Insert("d");
        yield return ["d", trie];

        var trie2 = new Trie();
        trie2.Insert("dog");
        yield return ["dog", trie2];

        var trie3 = new Trie();
        trie3.Insert("cat");
        trie3.Insert("dog");
        yield return ["dog", trie3];

        var trie4 = new Trie();
        trie4.Insert("camión");
        trie4.Insert("camion");
        yield return ["camión", trie4];
    }

    [Theory]
    [MemberData(nameof(GenerateNotFoundWords))]
    public void Should_return_false_when_a_word_does_not_exist(string word, Trie trie) => 
        trie.Search(word).Should().BeFalse();

    public static TheoryData<string, Trie> GenerateNotFoundWords()
    {
        var catTrie = new Trie();
        catTrie.Insert("cat");

        var dogTrie = new Trie();
        dogTrie.Insert("do");

        var multipleTrie = new Trie();
        multipleTrie.Insert("cat");
        multipleTrie.Insert("do");

        return new()
        {
            { "dog", catTrie },
            { "dog", dogTrie },
            { "dog", multipleTrie }
        };
    }

    [Theory]
    [InlineData("parallel")]
    [InlineData("parallel's")]
    public void Should_insert_a_word(string word)
    {
        Trie trie = new();
        trie.Insert(word);
        trie.Search(word).Should().BeTrue();
    }

    [Theory]
    [InlineData("parallel")]
    [InlineData("parallel's")]
    public void Should_delete_a_word(string word)
    {
        Trie trie = new();
        trie.Insert(word);
        trie.Search(word).Should().BeTrue();
        trie.Delete(word);
        trie.Search(word).Should().BeFalse();
    }

    [Theory]
    [InlineData("dog", "dogo")]
    [InlineData("dogo", "dog")]
    public void Should_not_delete_other_words_when_delete_a_word(string toRemove, string toKeep)
    {
        Trie trie = new();
        trie.Insert(toRemove);
        trie.Insert(toKeep);
        trie.Search(toRemove).Should().BeTrue();
        trie.Search(toKeep).Should().BeTrue();
        trie.Delete(toRemove);
        trie.Search(toKeep).Should().BeTrue();
    }

    [Theory]
    [InlineData("parallel")]
    public void Should_not_fail_deleting_a_word_that_does_not_exist(string word)
    {
        Trie trie = new();
        trie.Delete(word);
        trie.Search(word).Should().BeFalse();
    }

    [Theory]
    [InlineData("parallel")]
    [InlineData("parallel's")]
    public void Should_not_fail_if_inserting_a_word_twice(string word)
    {
        Trie trie = new();
        trie.Insert(word);
        trie.Insert(word);
        trie.Search(word).Should().BeTrue();
        trie.Delete(word);
        trie.Search(word).Should().BeFalse();
    }


    [Theory(Skip = "This should be enabled only if we need to play with accent marks.")]
    [InlineData("camión", "camion")]
    [InlineData("camion", "camión")]
    public void Should_works_with_accent_marks(string toStay, string toDelete)
    {
        Trie trie = new();
        trie.Insert(toStay);
        trie.Insert(toDelete);

        trie.Search(toStay).Should().BeTrue();
        trie.Search(toDelete).Should().BeTrue();

        trie.Delete(toDelete);
        
        trie.Search(toStay).Should().BeTrue();
        trie.Search(toDelete).Should().BeFalse();
    }
}