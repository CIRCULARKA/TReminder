namespace TReminder.Application.Messaging
{
    public interface IMessagesProvider
    {
        string GetMessage(string langCode, string messageName);
    }
}
