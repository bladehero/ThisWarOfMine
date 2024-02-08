using System.Globalization;
using Ardalis.SmartEnum;

namespace ThisWarOfMine.Contracts;

public sealed class Language : SmartEnum<Language>
{
    public static Language English = new(nameof(English), "en", 1);
    public static Language Ukrainian = new(nameof(Ukrainian), "uk", 2);
    public static Language Russian = new(nameof(Russian), "ru", 3);

    public CultureInfo Culture { get; }
    public string ShortName => Culture.IetfLanguageTag;
    
    public Language(string name, string culture, int value) : base(name, value)
    {
        Culture = CultureInfo.GetCultureInfo(culture);
    }
}