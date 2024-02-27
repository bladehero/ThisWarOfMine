using System.Collections;
using MediatR;
using ThisWarOfMine.Domain.Abstraction;

namespace ThisWarOfMine.Domain.Narrative.Options;

public sealed class OptionGroup : Entity<Unit>, IReadOnlyList<Option>
{
    private readonly List<Option> _options = new();

    public Alternative Alternative { get; private init; }
    public int Count => _options.Count;
    public bool HasRemarks => _options.OfType<RemarkOption>().Any();

    private OptionGroup(Alternative alternative)
    {
        Alternative = alternative;
    }

    public Option this[int order] => _options[order];

    internal Option Note(Guid guid, string remark)
    {
        var remarkOption = RemarkOption.Create(this, guid, remark);
        _options.Add(remarkOption);
        return remarkOption;
    }

    internal Option WithOnlyBackToGame(Guid guid)
    {
        ThrowIfBackToGameExists();

        var option = BackToGameOption.Create(this, guid);
        _options.Add(option);
        return option;
    }

    internal Option AppendWithText(Guid guid, string text, bool withBackToGame)
    {
        ThrowIfBackToGameExists();

        var option = GetTextOption();
        _options.Add(option);
        return option;

        Option GetTextOption()
        {
            var simpleOption = SimpleOption.Create(this, guid, text);
            if (!withBackToGame)
            {
                return simpleOption;
            }

            var backToGameOption = BackToGameOption.Create(this, guid);
            var complexOption = ComplexOption.Create(this, simpleOption, backToGameOption);
            return complexOption;
        }
    }

    internal Option AppendWithRedirection(Guid guid, string? text, short storyNumber, string? appendix)
    {
        ThrowIfBackToGameExists();

        Option redirectOption = RedirectOption.Create(this, guid, storyNumber);

        if (!string.IsNullOrWhiteSpace(text))
        {
            var textOption = SimpleOption.Create(this, guid, text);
            redirectOption = redirectOption.Prepend(textOption);
        }

        if (!string.IsNullOrWhiteSpace(appendix))
        {
            var appendixOption = SimpleOption.Create(this, guid, appendix);
            redirectOption = redirectOption.Append(appendixOption);
        }

        _options.Add(redirectOption);
        return redirectOption;
    }

    internal static OptionGroup Empty(Alternative alternative) => new(alternative);

    public IEnumerator<Option> GetEnumerator() => _options.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void ThrowIfBackToGameExists()
    {
        if (_options.OfType<BackToGameOption>().Any())
        {
            throw new InvalidOperationException("Cannot add any option if there is a single back to game added");
        }
    }
}
