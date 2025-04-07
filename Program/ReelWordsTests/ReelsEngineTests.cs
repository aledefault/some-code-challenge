using FluentAssertions;
using Moq;
using ReelWords;
using System;
using System.Collections.Generic;
using Xunit;

namespace ReelWordsTests;

public class ReelsEngineTests
{
    private readonly Mock<Random> _randomMock;

    public ReelsEngineTests()
    {
        _randomMock = new Mock<Random>();
        _randomMock.Setup(x => x.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(0);
    }

    [Fact]
    public void Should_throw_an_argument_null_exception_when_data_inserted_is_null()
    {
        var engine = new ReelsEngine(_randomMock.Object);
        var contruct = () => engine.Insert(null);
        contruct.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Should_throw_an_argument_exception_when_data_inserted_is_empty()
    {
        var engine = new ReelsEngine(_randomMock.Object);
        var contruct = () => engine.Insert([]);
        contruct.Should().ThrowExactly<ArgumentException>().WithMessage("*should have atleast one element*");
    }

    [Theory]
    [MemberData(nameof(GenerateValidDataEngines))]
    public void Should_rotate_a_letter(ReelsEngine engine, IList<Action<ReelsEngine>> actions, string result)
    {
        foreach (var a in actions)
            a(engine);

        engine.GetWord().Should().Be(result);
    }

    public static TheoryData<ReelsEngine, IList<Action<ReelsEngine>>, string> GenerateValidDataEngines()
    {
        var randomMock = new Mock<Random>();
        randomMock.Setup(x => x.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(0);

        var engine1 = new ReelsEngine(randomMock.Object);
        engine1.Insert(['a', 'b']);
        engine1.Insert(['c', 'd']);

        var engine2 = new ReelsEngine(randomMock.Object);
        engine2.Insert(['a', 'b']);
        engine2.Insert(['c', 'd']);
        engine2.Insert(['x']);
        engine2.Insert(['r', 'f', 'k']);

        var engine3 = new ReelsEngine(randomMock.Object);
        engine3.Insert(['a', 'b']);
        engine3.Insert(['c', 'd']);
        engine3.Insert(['x']);
        engine3.Insert(['r', 'f', 'k']);

        var engine4 = new ReelsEngine(randomMock.Object);
        engine4.Insert(['a', 'b']);
        engine4.Insert(['c', 'd']);
        engine4.Insert(['x']);
        engine4.Insert(['r', 'f', 'k']);

        return new()
        {
           {
                engine1,
                new List<Action<ReelsEngine>>
                {
                    engine => engine.Rotate(0)
                },
                "bc"
           },
           {
                engine2,
                new List<Action<ReelsEngine>>
                {
                    engine => engine.Rotate(0),
                    engine => engine.Rotate(1),
                    engine => engine.Rotate(2),
                    engine => engine.Rotate(3)
                },
                "bdxf"
           },
           {
                engine3,
                new List<Action<ReelsEngine>>
                {
                    engine => engine.Rotate(2),
                    engine => engine.Rotate(2)
                },
                "acxr"
           },
           {
                engine4,
                new List<Action<ReelsEngine>>
                {
                    engine => engine.Rotate(2),
                    engine => engine.Rotate(3),
                    engine => engine.Rotate(3),
                    engine => engine.Rotate(3)
                },
                "acxr"
           }
        };
    }
}
