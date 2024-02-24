using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThisWarOfMine.Common;

namespace ThisWarOfMine.Infrastructure.ChatGpt
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddChatGptInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        ) => services.AddScoped<IChatGpt, ChatGptWrapper>().AddConfiguration<ChatGptConfiguration>(configuration);
    }
}
