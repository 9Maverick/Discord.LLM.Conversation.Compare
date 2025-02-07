using Discord.LLM.Conversation.Compare.Models.Interfaces;
using Discord.LLM.Conversation.Compare.Models.Models;
using Discord.LLM.Conversation.Compare.Service.Services;

namespace Discord.LLM.Conversation.Compare.Service.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddModelProvider(this IServiceCollection services)
    {
        services.AddSingleton<IModelProvider, ModelProvider>();
        return services;
    }

    public static IServiceCollection AddDiscordService(this IServiceCollection services)
    {
        services.AddSingleton<DiscordService>();
        return services;
    }
}
