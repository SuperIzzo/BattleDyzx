using System;

namespace BattleDyzx
{
    public class NetLogger
    {
        public static void Log(object message)
        {
            Console.Write(message.ToString());
        }

        public static void LogWarning(object message)
        {
            Console.Write(message.ToString());
        }
    }
}
