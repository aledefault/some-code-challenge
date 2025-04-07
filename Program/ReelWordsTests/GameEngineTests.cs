using FluentAssertions;
using Moq;
using ReelWords;
using System.Collections.Generic;
using Xunit;

namespace ReelWordsTests;

public class GameEngineTests
{
    [Theory]
    [MemberData(nameof(GenerateValidGameEngine))]
    public void Should_generate_a_score_for_a_word(GameEngine gameEngine, string word, int score) =>
        gameEngine.CheckWord(word).Should().Be(score);

    public static TheoryData<GameEngine, string, int> GenerateValidGameEngine()
    {
        var wordStorage = new Mock<IWordsStorage>();
        wordStorage.Setup(x => x.Search(It.IsAny<string>())).Returns(true);

        var reelsEngine = new Mock<IReelsEngine>();
        reelsEngine.Setup(x => x.GetWord()).Returns("DKRAEK");

        var scoreService = new ScoresService();
        scoreService.Insert('a', 1);
        scoreService.Insert('d', 2);
        scoreService.Insert('e', 1);
        scoreService.Insert('k', 5);
        scoreService.Insert('r', 1);

        var gameEngine = new GameEngine(wordStorage.Object, reelsEngine.Object, scoreService);

        return new()
        {
               {
                    gameEngine,
                    "RAEK",
                    8
               }
        };
    }

    [Theory]
    [MemberData(nameof(GenerateValidGameEngineWithRotateData))]
    public void Should_rotate_a_character_when_it_scored(GameEngine gameEngine, Mock<IReelsEngine> reelsEngineMock, string word, int[] rotateIndexCalled)
    {
        gameEngine.CheckWord(word);
        foreach (var i in rotateIndexCalled)
            reelsEngineMock.Verify(x => x.Rotate(It.Is<int>(x => x == i)));
    }

    public static IEnumerable<object[]> GenerateValidGameEngineWithRotateData()
    {
        var wordStorage = new Mock<IWordsStorage>();
        wordStorage.Setup(x => x.Search(It.IsAny<string>())).Returns(true);

        var reelsEngineDKRAEK = new Mock<IReelsEngine>();
        reelsEngineDKRAEK.Setup(x => x.GetWord()).Returns("DKRAEK");

        yield return new object[]
        {
            new GameEngine(wordStorage.Object, reelsEngineDKRAEK.Object, new ScoresService()),
            reelsEngineDKRAEK,
            "RAEK",
            new int [] {2, 3, 4, 1 }
        };

        var reelsEngineKDUACK = new Mock<IReelsEngine>();
        reelsEngineKDUACK.Setup(x => x.GetWord()).Returns("KDUACK");

        yield return new object[]
        {
            new GameEngine(wordStorage.Object, reelsEngineKDUACK.Object, new ScoresService()),
            reelsEngineKDUACK,
            "DUCK",
            new int [] {1, 2, 4, 0 }
        };
    }

    [Theory]
    [MemberData(nameof(GenerateDataForValidTotalScore))]
    public void Should_generate_a_valid_total_score(GameEngine gameEngine, string[] words, int score)
    {
        foreach (var word in words)
            gameEngine.CheckWord(word);

        gameEngine.TotalScore.Should().Be(score);
    }

    public static TheoryData<GameEngine, string[], int> GenerateDataForValidTotalScore()
    {
        var wordStorage = new Mock<IWordsStorage>();
        wordStorage.Setup(x => x.Search(It.IsAny<string>())).Returns(true);
        wordStorage.Setup(x => x.Search(It.Is<string>(x => x.Equals("dog")))).Returns(false);

        var reelsEngine = new Mock<IReelsEngine>();
        reelsEngine.Setup(x => x.GetWord()).Returns("DKOHGI");

        var scoreService = new ScoresService();
        scoreService.Insert('h', 1);
        scoreService.Insert('d', 2);
        scoreService.Insert('i', 1);
        scoreService.Insert('g', 5);
        scoreService.Insert('r', 1);
        scoreService.Insert('o', 1);

        return new()
        {
               {
                    new GameEngine(wordStorage.Object, reelsEngine.Object, scoreService),
                    ["dog", "HI"],
                    2
               }
        };
    }

    [Fact]
    public void Should_not_score_if_the_word_exists_but_not_match_the_reel()
    {
        var wordStorage = new Mock<IWordsStorage>();
        wordStorage.Setup(x => x.Search(It.IsAny<string>())).Returns(true);

        var reelsEngine = new Mock<IReelsEngine>();
        reelsEngine.Setup(x => x.GetWord()).Returns("DKOHGY");

        var scoreService = new ScoresService();
        scoreService.Insert('h', 1);
        scoreService.Insert('d', 2);
        scoreService.Insert('i', 1);
        scoreService.Insert('g', 5);
        scoreService.Insert('r', 1);
        scoreService.Insert('o', 1);

        var gameEngine = new GameEngine(wordStorage.Object, reelsEngine.Object, scoreService);

        gameEngine.CheckWord("HI");
        gameEngine.TotalScore.Should().Be(0);
    }


    [Fact]
    public void Should_not_rotate_if_the_word_exists_but_not_match_the_reel()
    {
        var wordStorage = new Mock<IWordsStorage>();
        wordStorage.Setup(x => x.Search(It.IsAny<string>())).Returns(true);

        var reelsEngine = new Mock<IReelsEngine>();
        reelsEngine.Setup(x => x.GetWord()).Returns("uqliao");

        var scoreService = new ScoresService();
        scoreService.Insert('l', 1);
        scoreService.Insert('i', 2);
        scoreService.Insert('s', 1);

        var gameEngine = new GameEngine(wordStorage.Object, reelsEngine.Object, scoreService);

        gameEngine.CheckWord("lis");
        gameEngine.TotalScore.Should().Be(0);

        reelsEngine.Verify(x => x.Rotate(It.IsAny<int>()), Times.Never);
    }
}