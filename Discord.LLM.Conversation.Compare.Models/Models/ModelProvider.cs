using Discord.LLM.Conversation.Compare.Interfaces;
using Discord.LLM.Conversation.Compare.Models.Mirror;
using Discord.LLM.Conversation.Compare.Models.OpenAI;
using Microsoft.Extensions.Configuration;

namespace Discord.LLM.Conversation.Compare.Models.Models;

public class ModelProvider : IModelProvider
{
    private List<IModelProvider> _modelProviders;

    public ModelProvider(IConfiguration configuration)
    {
        _modelProviders = new List<IModelProvider>
        {
            new OpenAiProvider(configuration),
            new MirrorProvider(configuration)
        };
    }

    public async Task Querry(string prompt, Action<string, IModelSettings> sendResponce)
    {
        _modelProviders.ForEach(modelProvider => modelProvider.Querry(prompt, sendResponce));
    }
}