namespace ThisWarOfMine.Contracts.Narrative.Options;

public sealed class OptionGroup
{
    private readonly List<string> _remarks = new();
    private readonly List<Option> _options = new();

    public Alternative Alternative { get; private init; }
    public IReadOnlyCollection<string> Remarks => _remarks.AsReadOnly();
    public IReadOnlyCollection<Option> Options => _options.AsReadOnly();

    private OptionGroup(Alternative alternative) => Alternative = alternative;

    public OptionGroup Note(string remark)
    {
        _remarks.Add(remark);
        return this;
    }

    public OptionGroup WithOnlyBackToGame()
    {
        ThrowIfOptionsNotEmpty();

        var option = BackToGameOption.Create(this);
        _options.Add(option);
        return this;
    }

    public OptionGroup AppendWithText(string text)
    {
        ThrowIfBackToGameExists();

        var simpleOption = SimpleOption.Create(this, text);
        var backToGameOption = BackToGameOption.Create(this);
        var complexOption = ComplexOption.Create(this, simpleOption, backToGameOption);
        _options.Add(complexOption);
        return this;
    }

    public OptionGroup AppendWithRedirection(int storyNumber)
    {
        ThrowIfBackToGameExists();

        var redirectOption = RedirectOption.Create(this, storyNumber);
        _options.Add(redirectOption);
        return this;
    }

    public OptionGroup AppendWithRedirection(string text, int storyNumber)
    {
        ThrowIfBackToGameExists();

        var simpleOption = SimpleOption.Create(this, text);
        var redirectOption = RedirectOption.Create(this, storyNumber);
        var complexOption = ComplexOption.Create(this, simpleOption, redirectOption);
        _options.Add(complexOption);
        return this;
    }

    public static OptionGroup Empty(Alternative alternative) => new(alternative);
    
    private void ThrowIfOptionsNotEmpty()
    {
        if (_options.Any())
        {
            throw new InvalidOperationException(
                "Cannot use back to game option if there are already other options defined"
            );
        }
    }

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