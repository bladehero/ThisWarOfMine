using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Application.Telegram.States;

public interface IRandomAlternativePicker
{
    Task<Result<Alternative, Error>> GetRandomAsync(
        short storyNumber,
        Func<Task>? onStoryNotFound = null,
        CancellationToken token = default
    );
}
