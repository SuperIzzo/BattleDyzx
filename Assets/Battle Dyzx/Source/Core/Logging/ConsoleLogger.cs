namespace BattleDyzx
{
    public class ConsoleLogger : ILogger
    {
        public void Log(object log)
        {
            Log(LogLevel.Info, log);
        }

        public void Log(LogLevel logLevel, object log)
        {
            System.Console.WriteLine("[" + logLevel.ToString() +"] " + log.ToString());
        }
    }
}