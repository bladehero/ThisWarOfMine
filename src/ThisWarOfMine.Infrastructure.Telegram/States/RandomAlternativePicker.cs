using CSharpFunctionalExtensions;
using ThisWarOfMine.Application.Telegram.States;
using ThisWarOfMine.Common.Wrappers;
using ThisWarOfMine.Domain;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Infrastructure.Telegram.States;

internal sealed class RandomAlternativePicker : IRandomAlternativePicker
{
    private readonly IBookRepository _bookRepository;
    private readonly ITelegramSettingsState _telegramSettingsState;
    private readonly IRandom _random;

    public RandomAlternativePicker(
        IBookRepository bookRepository,
        ITelegramSettingsState telegramSettingsState,
        IRandom random
    )
    {
        _bookRepository = bookRepository;
        _telegramSettingsState = telegramSettingsState;
        _random = random;
    }

    public Task<Result<Alternative, Error>> GetRandomAsync(
        short storyNumber,
        Func<Task>? onStoryNotFound = null,
        CancellationToken token = default
    )
    {
        return _bookRepository
            .FindByNameAsync(Constants.BookOfScripts, token)
            .TapError(error => throw new InvalidOperationException($"Cannot find book by name: {error}"))
            .Bind(x => x.StoryBy(storyNumber))
            .TapError(() => onStoryNotFound?.Invoke() ?? Task.CompletedTask)
            .Bind(GettingTranslationOrOriginal)
            .TapError(error => throw new InvalidOperationException($"Cannot find any alternative: {error}"));

        Result<Alternative, Error> GettingTranslationOrOriginal(Story story)
        {
            var settings = _telegramSettingsState.Get();
            return story.TranslationBy(settings.Language).Bind(translation => translation.Random(_random.Next));
        }
    }
}
