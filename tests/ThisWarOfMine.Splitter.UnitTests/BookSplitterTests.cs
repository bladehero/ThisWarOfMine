using Moq;

namespace ThisWarOfMine.Splitter.UnitTests;

public sealed class BookSplitterTests
{
    private readonly Mock<IStoryCreator> _storyCreatorMock = new();
    private readonly BookSplitter _sut;

    public BookSplitterTests()
    {
        _sut = new BookSplitter(_storyCreatorMock.Object);
    }
}