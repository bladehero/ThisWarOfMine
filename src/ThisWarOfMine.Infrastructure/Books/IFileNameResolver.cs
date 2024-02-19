using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Infrastructure.Books;

internal interface IFileNameResolver
{
    Maybe<string> IfNotExistsGetFileNameFor(Book aggregate);
}
