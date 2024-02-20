namespace ThisWarOfMine.Common.Wrappers;

public interface IGuidProvider : IWrapper
{
    Guid NewGuid();
}
