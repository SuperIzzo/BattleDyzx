using UnityEngine;

namespace BattleDyzx
{
    public class UnityLogger : ILogger
    {
        public void Log(object log)
        {
            Debug.Log(log);
        }

        public void Log(LogLevel logLevel, object log)
        {
            switch(logLevel)
            {
                case LogLevel.Detail:
                case LogLevel.Info:
                    Debug.Log(log);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(log);
                    break;
                case LogLevel.Error:
                    Debug.LogError(log);
                    break;
            }
        }
    }
}