namespace Discord.LLM.Conversation.Compare.Models.Interfaces;

public interface IModelProvider
{
    public Task Querry(string prompt, Action<string, IModelSettings> sendResponce);
}
