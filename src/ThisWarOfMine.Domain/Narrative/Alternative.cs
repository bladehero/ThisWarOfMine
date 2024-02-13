using ThisWarOfMine.Domain.Narrative.Options;

namespace ThisWarOfMine.Domain.Narrative;

public sealed class Alternative
{
    public string Text { get; private init; } = null!;
    public bool IsOriginal { get; private init; }
    public OptionGroup Options { get; private set; } = null!;
    public Translation Translation { get; private init; }

    private Alternative(Translation translation) => Translation = translation;

    public static Alternative Create(Translation translation, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(text));
        }

        var alternative = new Alternative(translation)
            { Text = text, IsOriginal = IfTheVeryFirstAlternative(translation) };
        alternative.Options = OptionGroup.Empty(alternative);
        return alternative;
    }

    private static bool IfTheVeryFirstAlternative(Translation translation) =>
        translation.Story.Translations.Count == 1 && !translation.Alternatives.Any();
}