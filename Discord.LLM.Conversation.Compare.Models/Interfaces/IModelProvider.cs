namespace Discord.LLM.Conversation.Compare.Interfaces;

public interface IModelProvider
{
    public Task Querry(string prompt, Action<string, IModelSettings> sendResponce);
}
