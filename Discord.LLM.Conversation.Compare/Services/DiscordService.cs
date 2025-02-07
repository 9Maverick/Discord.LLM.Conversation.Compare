using Discord.LLM.Conversation.Compare.Models.Interfaces;
using Discord.LLM.Conversation.Compare.Service.Models;
using Discord.Net;
using Discord.Webhook;
using Discord.WebSocket;
using System.Diagnostics;

namespace Discord.LLM.Conversation.Compare.Service.Services;

public class DiscordService
{
    const string WebhookName = "LLM";
    const string querry = "querry";

    private string token;

    private IModelProvider _modelProvider;
    private DiscordSocketClient _client;

    public DiscordService(IConfiguration configuration, IModelProvider modelProvider)
    {
        var configurationSection = configuration.GetSection(nameof(DiscordSettings));
        var settings = configurationSection.Get<DiscordSettings>();

        token = settings?.Token;
        _modelProvider = modelProvider;

        StartClient();
    }

    /// <summary>
    /// Setting up the Discord client
    /// </summary>
    void StartClient()
    {
        if(string.IsNullOrEmpty(token))
            return;
        try
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.All,
                AlwaysDownloadUsers = true
            });

            _client.SlashCommandExecuted += SlashCommandsHandler;
            _client.Log += Log;
            _client.Ready += ConfigureCommands;

            _client.LoginAsync(TokenType.Bot, token);
            _client.StartAsync();

        }
        catch(Exception ex)
        {
            Trace.WriteLine(DateTime.Now.ToString() + " " + ex.Message + "\n" + ex.ToString);
        }
    }

    async Task ConfigureCommands()
    {
        var querryCommand = new SlashCommandBuilder()
            .WithName(querry)
            .WithDescription("Querries all LLM models in system")
            .AddOption("prompt", ApplicationCommandOptionType.String, "Prompt to models", isRequired: true);

        try
        {
            await _client.CreateGlobalApplicationCommandAsync(querryCommand.Build());
        }
        catch(HttpException exception)
        {
            Trace.WriteLine(exception.Message);
        }
    }

    public async Task SendToChat(SocketTextChannel chat, string message)
    {
        if(chat == null)
            return;

        await chat.SendMessageAsync(message);
    }

    /// <summary>
    /// Logging to trace
    /// </summary>
    /// <param name="msg"></param>
    private Task Log(LogMessage msg)
    {
        Debug.WriteLine(msg);
        Trace.WriteLine(msg);
        return Task.CompletedTask;
    }

    async Task SlashCommandsHandler(SocketSlashCommand command)
    {
        switch(command.Data.Name)
        {
            case querry:
                await QuerryCommandHandler(command);
                break;
        }
    }
    async Task QuerryCommandHandler(SocketSlashCommand command)
    {
        var prompt = (string?)command.Data?.Options?
        .FirstOrDefault(option => option.Type == ApplicationCommandOptionType.String)?.Value;

        if(prompt == null)
            return;

        if(command.Channel is not SocketTextChannel channel)
            return;

        var restWebhook = channel.CreateWebhookAsync(WebhookName)
            .Result;
        var webhook = new DiscordWebhookClient(restWebhook);

        _ = _modelProvider.Querry(prompt, (responce, modelSettings) =>
        {
            if(responce == null)
                return;

            webhook.SendMessageAsync(responce, username: modelSettings.ModelName, avatarUrl: modelSettings.ModelImageUrl);
        });

        var responce = $"Prompt: {prompt}";
        await command.RespondAsync(responce);
    }
}
