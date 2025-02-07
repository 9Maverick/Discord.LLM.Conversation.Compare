using Discord.LLM.Conversation.Compare.Models.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Discord.LLM.Conversation.Compare.Models.Models.Base;

public class BaseModelSettings : IModelSettings
{
    public string ModelName { get; set; }
    public string ModelVersion { get; set; }
    public string ModelImageUrl { get; set; }
    public string ModelKey { get; set; }

    public BaseModelSettings(IConfiguration configuration, Type configurationSectionType)
    {
        var section = configuration.GetSection(configurationSectionType.Name);
        var t = section.Get(configurationSectionType, null);

        if(t is not IModelSettings modelSettings)
            return;

        ModelName = modelSettings.ModelName;
        ModelVersion = modelSettings.ModelVersion;
        ModelImageUrl = modelSettings.ModelImageUrl;
        ModelKey = modelSettings.ModelKey;
    }

    internal BaseModelSettings() { }
}