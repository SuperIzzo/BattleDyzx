using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25001;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if( Network.peerType == NetworkPeerType.Disconnected )
        {
            GUI.Label( new Rect( 10, 10, 200, 20 ), "Status: Disconnected" );

            if( GUI.Button( new Rect( 10, 30, 120, 20 ), "Client Connect" ) )
            {
                Network.Connect( connectionIP, connectionPort );
            }

            if( GUI.Button( new Rect( 10, 50, 120, 20 ), "Initialize Server" ) )
            {
                Network.InitializeServer( 32, connectionPort, false );
            }
        }
        else if( Network.peerType == NetworkPeerType.Client )
        {
            GUI.Label( new Rect( 10, 10, 300, 20 ), "Status: Connected as Client" );
            if( GUI.Button( new Rect( 10, 30, 120, 20 ), "Disconnect" ) )
            {
                Network.Disconnect( 200 );
            }
        }
    }
}
