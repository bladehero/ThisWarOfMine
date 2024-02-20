using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Infrastructure.Books;

internal interface IBookNameResolver
{
    string GetFileNameFor(Guid bookId);
    Maybe<string> IfNotExistsGetFileNameFor(Book aggregate);
}
