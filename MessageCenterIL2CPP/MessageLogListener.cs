using BepInEx.Logging;

namespace MessageCenterIL2CPP
{
    public partial class MessageCenter
    {
        private sealed class MessageLogListener : ILogListener
        {
            LogLevel ILogListener.LogLevelFilter => LogLevel.Message;
            public void LogEvent(object sender, LogEventArgs eventArgs) => OnEntryLogged(eventArgs);
            public void Dispose() { }
        }
    }
}
