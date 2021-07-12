using System;

namespace BattleDyzx
{
    public class NetLog
    {
        private static ILogger _logger;
        public static ILogger logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new ConsoleLogger();
                }

                return _logger;
            }

            set { _logger = value; }
        }

        public static void Log(object message)
        {
            logger.Log(message);
        }

        public static void Log(LogLevel level, object message)
        {
            logger.Log(level, message);
        }
    }
}
