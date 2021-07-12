namespace BattleDyzx
{
    public enum LogLevel
    {
        Detail,
        Info,
        Warning,
        Error
    }

    public interface ILogger
    {
        void Log(object log);
        void Log(LogLevel logLevel, object log);
    }
}