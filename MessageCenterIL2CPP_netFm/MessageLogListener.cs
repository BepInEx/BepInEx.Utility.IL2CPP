using BepInEx.Logging;

namespace MessageCenterIL2CPP_netFm
{
    public partial class MessageCenterIL2CPP_netFm
    {
        private sealed class MessageLogListener : ILogListener
        {
            LogLevel ILogListener.LogLevelFilter => LogLevel.Message;
            public void LogEvent(object sender, LogEventArgs eventArgs) => OnEntryLogged(eventArgs);
            public void Dispose() { }
        }
    }
}
