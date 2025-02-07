using Discord.LLM.Conversation.Compare.Models.Interfaces;
using Discord.LLM.Conversation.Compare.Models.Models.Base;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;

namespace Discord.LLM.Conversation.Compare.Models;

public class OpenAiProvider : BaseModelSettings, IModelProvider
{
    private IChatClient _chatClient;

    public OpenAiProvider(IConfiguration configuration)
        : base(configuration, typeof(OpenAiSettings))
    {
        _chatClient = new OpenAIClient(ModelKey)
            .AsChatClient(ModelVersion);
    }

    public async Task Querry(string prompt, Action<string, IModelSettings> sendResponce)
    {
        var responce = await _chatClient.CompleteAsync(prompt, new ChatOptions { MaxOutputTokens = 400 });

        sendResponce.Invoke(responce.ToString(), this);
    }
}