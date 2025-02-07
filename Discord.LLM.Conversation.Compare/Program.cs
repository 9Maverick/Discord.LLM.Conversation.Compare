using Discord.LLM.Conversation.Compare.Service.Extensions;
using Discord.LLM.Conversation.Compare.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddModelProvider();
builder.Services.AddDiscordService();

var app = builder.Build();
app.Services.GetService<DiscordService>();

app.Run();
