using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace ThisWarOfMine.Application.Telegram.Abstraction
{
    [SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
    public sealed record TelegramNotification<T>(Guid Id, int UpdateId, T Payload) : INotification;
}
