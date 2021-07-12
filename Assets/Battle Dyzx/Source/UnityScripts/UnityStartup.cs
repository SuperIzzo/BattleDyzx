
using BattleDyzx;
using UnityEngine;

public class UnityStartup
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void SetupLoggers()
    {
        NetLog.logger = new UnityLogger();
    }
}
