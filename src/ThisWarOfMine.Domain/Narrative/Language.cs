using System.Globalization;
using Ardalis.SmartEnum;
using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Narrative;

public sealed class Language : SmartEnum<Language>
{
    public static readonly Language English = new(nameof(English), "en", 1);
    public static readonly Language Ukrainian = new(nameof(Ukrainian), "uk", 2);
    public static readonly Language Russian = new(nameof(Russian), "ru", 3);

    public CultureInfo Culture { get; }
    public string ShortName => Culture.IetfLanguageTag;

    private Language(string name, string culture, int value)
        : base(name, value) => Culture = CultureInfo.GetCultureInfo(culture);

    public static Language FromShortName(string shortName) =>
        List.TryFirst(x => x.ShortName == shortName)
            .GetValueOrThrow($"Cannot find language by short name: `{shortName}`");
}
