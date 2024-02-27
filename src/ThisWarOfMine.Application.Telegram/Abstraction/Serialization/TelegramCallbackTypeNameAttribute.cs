namespace ThisWarOfMine.Application.Telegram.Abstraction.Serialization;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal sealed class TelegramCallbackTypeNameAttribute : Attribute
{
    public string Name { get; }

    public TelegramCallbackTypeNameAttribute(string name)
    {
        Name = name;
    }
}
