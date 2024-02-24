using System.Diagnostics.CodeAnalysis;

namespace ThisWarOfMine.Infrastructure.ChatGpt
{
    public sealed class ChatGptConfiguration
    {
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string? SecretKey { get; set; }
    }
}
