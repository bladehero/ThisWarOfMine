using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Narrative;

public sealed class StoryNumber : ValueObject
{
    public short Value { get; }
    public Maybe<Story> Story { get; private set; }
    public bool Assigned => Story.HasValue;

    public StoryNumber(short value) => Value = value;

    public override string ToString() => Value.ToString();

    internal void Assign(Story story)
    {
        if (Story.HasValue)
        {
            throw new InvalidOperationException(
                "Cannot assign story to story number that has already been assigned to story"
            );
        }

        Story = story;
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    public static bool operator >(StoryNumber? left, StoryNumber? right) => left?.Value > right?.Value;

    public static bool operator <(StoryNumber? left, StoryNumber? right) => left?.Value < right?.Value;

    public static bool operator >=(StoryNumber? left, StoryNumber? right) => left?.Value >= right?.Value;

    public static bool operator <=(StoryNumber? left, StoryNumber? right) => left?.Value <= right?.Value;

    public static implicit operator StoryNumber(short value) => new(value);

    public static implicit operator short(StoryNumber storyNumber) => storyNumber.Value;
}
