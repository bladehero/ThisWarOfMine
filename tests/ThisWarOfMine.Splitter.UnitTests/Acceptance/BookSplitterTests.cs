using FluentAssertions;
using FluentAssertions.Execution;
using ThisWarOfMine.Contracts;

namespace ThisWarOfMine.Splitter.UnitTests.Acceptance;

public sealed class BookSplitterTests
{
    private const string Folder = "Acceptance/Content";
    private const string File = "book.txt";
    private static readonly int[] ExcludedParts =
    {
        10, 100, 101, 110, 150, 155, 200, 220, 300, 351, 400, 500, 534, 600, 610, 700, 740, 800, 810, 900, 901, 905, 909,
        1001
    };

    private readonly BookSplitter _sut;

    public BookSplitterTests()
    {
        _sut = new BookSplitter(new StoryCreator());
    }

    [Fact]
    public async Task SplitAsync_WhenBookWith1947StoriesProvided_ShouldParseAllOfThemInCorrectOrder()
    {
        // Arrange
        const int expectedCount = 1923;
        var expectedNumbers = Enumerable.Range(1, 1947).Except(ExcludedParts);
        var path = Path.Combine(Folder, File);


        // Act
        var actual = await _sut.SplitAsync(path, Language.Russian).ToListAsync();

        // Assert
        using (new AssertionScope())
        {
            actual.Should().HaveCount(expectedCount);
            actual.Should().BeInAscendingOrder(x => x.Number);
            expectedNumbers.Except(actual.Select(x => x.Number)).Should().BeEmpty();
        }
    }
}