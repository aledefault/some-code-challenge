using FluentAssertions;
using ReelWords;
using Xunit;

namespace ReelWordsTests;

public class PointsServiceTests
{
    [Fact]
    public void Should_return_0_if_letter_does_not_exist()
    {
        var scoresService = new ScoresService();
        scoresService.Insert('a', 1);
        scoresService.HowManyPointsFor('b').Should().Be(0);
    }

    [Theory]
    [MemberData(nameof(GenerateValidPoints))]
    public void Should_return_the_valid_point_for_a_letter(ScoresService scoresService, char c, int points) =>
        scoresService.HowManyPointsFor(c).Should().Be(points);

    public static TheoryData<ScoresService, char, int> GenerateValidPoints()
    {
        var score1 = new ScoresService();
        score1.Insert('a', 1);

        var score2 = new ScoresService();
        score2.Insert('a', 1);
        score2.Insert('b', 2);

        return new()
        {
           { score1, 'a', 1 },
           { score2, 'b', 2 }
        };
    }
}
