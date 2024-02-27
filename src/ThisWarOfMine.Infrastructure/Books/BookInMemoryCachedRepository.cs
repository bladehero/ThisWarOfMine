using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Memory;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Infrastructure.Books;

internal sealed class BookInMemoryCachedRepository : IBookRepository
{
    private readonly IBookRepository _bookRepository;
    private readonly IMemoryCache _memoryCache;

    public BookInMemoryCachedRepository(IBookRepository bookRepository, IMemoryCache memoryCache)
    {
        _bookRepository = bookRepository;
        _memoryCache = memoryCache;
    }

    public Task SaveAsync(Book aggregate, CancellationToken cancellationToken) =>
        _bookRepository.SaveAsync(aggregate, cancellationToken);

    public Task<Result<Book, Error>> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        BookEntryKey<Guid> key = id;
        Task<Result<Book, Error>> ById() => _bookRepository.LoadAsync(id, cancellationToken);
        return TryGetFromCacheFirst(key, ById);
    }

    public Task<Result<Book, Error>> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        BookEntryKey<string> key = name;
        Task<Result<Book, Error>> ByName() => _bookRepository.FindByNameAsync(name, cancellationToken);
        return TryGetFromCacheFirst(key, ByName);
    }

    #region Helpers

    private async Task<Result<Book, Error>> TryGetFromCacheFirst<TKey>(
        [DisallowNull] TKey key,
        Func<Task<Result<Book, Error>>> bookFactory
    )
        where TKey : BookEntryKey
    {
        var exists = _memoryCache.TryGetValue(key, out var value);
        if (exists && value is Book cached)
        {
            return cached;
        }

        return await bookFactory().Tap(SaveInCache);
    }

    private void SaveInCache(Book book)
    {
        var options = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);
        _memoryCache.Set(BookEntryKey.AsGuidKey(book), book, options);
        _memoryCache.Set(BookEntryKey.AsStringKey(book), book, options);
    }

    #endregion

    #region Keys

    private sealed record BookEntryKey<T>(T Key) : BookEntryKey
    {
        public static implicit operator BookEntryKey<T>(T value) => new(value);
    }

    private record BookEntryKey
    {
        public static BookEntryKey<Guid> AsGuidKey(Book book) => new(book.Id);

        public static BookEntryKey<string> AsStringKey(Book book) => new(book.Name);
    }

    #endregion
}
