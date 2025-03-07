﻿using Discord.LLM.Conversation.Compare.Interfaces;
using Discord.LLM.Conversation.Compare.Models.Models.Base;
using Microsoft.Extensions.Configuration;

namespace Discord.LLM.Conversation.Compare.Models.Mirror;

public class MirrorProvider : BaseModelSettings, IModelProvider
{
    public MirrorProvider(IConfiguration configuration)
        : base(configuration, typeof(MirrorSettings)) { }

    public async Task Querry(string prompt, Action<string, IModelSettings> sendResponce)
    {
        sendResponce.Invoke(prompt, this);
    }
}