namespace ThisWarOfMine.Infrastructure.Books
{
    public sealed class BookAccessorConfiguration
    {
        private IBookAccessingStrategy _accessingStrategy = new AutoDisposeBookAccessingStrategy();

        public IBookAccessingStrategy AccessingStrategy
        {
            get => _accessingStrategy;
            set =>
                _accessingStrategy = value ?? throw new InvalidOperationException("Strategy should be always defined");
        }
    }
}
