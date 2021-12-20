namespace TReminder.Application.Messaging
{
    public interface IMessagesProvider
    {
        /// <summary>
        /// Default is "en"
        /// </summary>
        void ChangeLanguage(string languageCode);

        string GetMessage(string messageName);
    }
}
