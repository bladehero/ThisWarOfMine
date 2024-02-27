namespace ThisWarOfMine.Common.Wrappers;

internal sealed class RandomWrapper : IRandom
{
    public int Next(int maximum) => Random.Shared.Next(maximum);
}
