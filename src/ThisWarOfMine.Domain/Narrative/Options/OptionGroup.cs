using System.Collections;

namespace ThisWarOfMine.Domain.Narrative.Options;

public sealed class OptionGroup : IReadOnlyCollection<Option>
{
    private readonly List<Option> _options = new();

    public Alternative Alternative { get; private init; }
    public int Count => _options.Count;
    public bool HasRemarks => _options.OfType<RemarkOption>().Any();

    private OptionGroup(Alternative alternative) => Alternative = alternative;

    public Option Note(string remark)
    {
        var remarkOption = RemarkOption.Create(this, remark);
        _options.Add(remarkOption);
        return remarkOption;
    }

    public Option WithOnlyBackToGame()
    {
        ThrowIfBackToGameExists();

        var option = BackToGameOption.Create(this);
        _options.Add(option);
        return option;
    }

    public Option AppendWithText(string text, bool withBackToGame)
    {
        ThrowIfBackToGameExists();

        var option = GetTextOption();
        _options.Add(option);
        return option;

        Option GetTextOption()
        {
            var simpleOption = SimpleOption.Create(this, text);
            if (!withBackToGame)
            {
                return simpleOption;
            }

            var backToGameOption = BackToGameOption.Create(this);
            var complexOption = ComplexOption.Create(this, simpleOption, backToGameOption);
            return complexOption;
        }
    }

    public Option AppendWithTextAndBackToGame(string text)
    {
        ThrowIfBackToGameExists();

        var simpleOption = SimpleOption.Create(this, text);
        var backToGameOption = BackToGameOption.Create(this);
        var complexOption = ComplexOption.Create(this, simpleOption, backToGameOption);
        _options.Add(complexOption);
        return complexOption;
    }

    public Option AppendWithRedirection(int storyNumber, string? text = null, string? appendix = null)
    {
        ThrowIfBackToGameExists();

        Option redirectOption = RedirectOption.Create(this, storyNumber);

        if (!string.IsNullOrWhiteSpace(text))
        {
            var textOption = SimpleOption.Create(this, text);
            redirectOption = redirectOption.Prepend(textOption);
        }

        if (!string.IsNullOrWhiteSpace(appendix))
        {
            var appendixOption = SimpleOption.Create(this, appendix);
            redirectOption = redirectOption.Append(appendixOption);
        }

        _options.Add(redirectOption);
        return redirectOption;
    }

    public static OptionGroup Empty(Alternative alternative) => new(alternative);

    public IEnumerator<Option> GetEnumerator() => _options.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void ThrowIfBackToGameExists()
    {
        if (_options.OfType<BackToGameOption>().Any())
        {
            throw new InvalidOperationException(
                "Cannot add any option if there is a single back to game added"
            );
        }
    }
}