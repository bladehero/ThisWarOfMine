namespace ThisWarOfMine.Common.Wrappers;

internal sealed class GuidProvider : IGuidProvider
{
    public Guid NewGuid() => Guid.NewGuid();
}
