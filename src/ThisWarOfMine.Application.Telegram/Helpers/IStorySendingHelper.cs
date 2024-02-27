using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Application.Telegram.Helpers;

internal interface IStorySendingHelper
{
    Task<Result<Alternative, Error>> SendAsync(short storyNumber, long chatId, CancellationToken token);
}
