using Moq;
using ThisWarOfMine.Splitter.Options;

namespace ThisWarOfMine.Splitter.UnitTests;

internal sealed class StoryCreatorTests
{
    private readonly Mock<IOptionParser> _optionParserMock;
    private readonly StoryParser _sut;

    public StoryCreatorTests()
    {
        _optionParserMock = new Mock<IOptionParser>();
        _sut = new StoryParser(_optionParserMock.Object);
    }
}