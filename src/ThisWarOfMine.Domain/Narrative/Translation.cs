﻿using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative.Events;

namespace ThisWarOfMine.Domain.Narrative;

public sealed class Translation : Abstraction.Entity<Language>
{
    private readonly List<Alternative> _alternatives = new();

    public Language Language => Id;
    public Maybe<Alternative> Original => _alternatives.TryFirst(x => x.IsOriginal);
    public bool IsEmpty => !_alternatives.Any();
    public Story Story { get; private init; }

    private Translation(Story story)
    {
        Story = story;

        Register<TranslationAlternativeAddedToBookEvent, Alternative>(Apply);
    }

    public Result<Alternative, Error> AlternativeBy(Guid guid) =>
        _alternatives
            .TryFirst(x => x.HasSame(guid))
            .ToResult(
                Error.Because($"Not defined alternative `{guid}` for translation: `{Language}` for story: {Story.Id}")
            );

    /// <summary>
    ///
    /// </summary>
    /// <param name="randomIndexSelector">Function where: first parameter is maximum value of index excluded, returns randomly selected index</param>
    /// <returns></returns>
    public Result<Alternative, Error> Random(Func<int, int> randomIndexSelector)
    {
        if (!_alternatives.Any())
        {
            return Error.Because("No alternatives defined yet");
        }

        var index = randomIndexSelector(_alternatives.Count);
        if (index < 0 || index >= _alternatives.Count)
        {
            return Error.Because("Index out of bound");
        }

        return _alternatives[index];
    }

    internal static Translation Create(Story story, Language language) => new(story) { Id = language };

    #region Event Handling

    private Result<Alternative, Error> Apply(TranslationAlternativeAddedToBookEvent @event)
    {
        return Alternative
            .Create(this, @event.AlternativeId, @event.Text)
            .Tap(alternative => _alternatives.Add(alternative));
    }

    #endregion
}
