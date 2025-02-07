namespace Discord.LLM.Conversation.Compare.Models.Interfaces;

public interface IModelSettings
{
    public string ModelName { get; set; }
    public string ModelVersion { get; set; }
    public string ModelImageUrl { get; set; }
    public string ModelKey { get; set; }
}