using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BattleDyzx;

public class NetworkManager : MonoBehaviour
{
    NetDriver netDriver;

    [SerializeField]
    public bool isHost;

    [SerializeField]
    public int localPort = -1;

    [SerializeField]
    public int remotePort = -1;

    [SerializeField]
    public string remoteIp = "127.0.0.1";    

    void Start()
    {
        netDriver = new NetDriver();

        if (isHost)
        {
            netDriver.Listen(localPort);
        }
        else
        {
            netDriver.Connect(remoteIp, remotePort);
        }
    }

    void Update()
    {
        netDriver.Update(Time.deltaTime);
    }

    void OnApplicationQuit()
    {
        netDriver.Shutdown();
    }
}
