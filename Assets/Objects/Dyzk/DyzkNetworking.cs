using UnityEngine;
using System.Collections;

public class DyzkNetworking : MonoBehaviour
{
    Vector3 ?targetPos;

    //--------------------------------
    Vector3 vel;

    Vector3 lastPosition;
    Vector2 lastControl;
    float minimumMovement = .01f;

    NetworkView _networkView;

    // Use this for initialization
    void Start()
    {
        _networkView = GetComponent<NetworkView>();
    }

    // Update is called once per frame
    void Update()
    {
        if( Network.isServer )
        {
            if( Vector3.Distance( transform.position, lastPosition ) > minimumMovement )
            {
                NetworkViewID viewID = _networkView.viewID;
                lastPosition = transform.position;
                _networkView.RPC( "SetPosition", RPCMode.Others, viewID, transform.position );

                lastPosition = transform.position;
            }
        }
        else if( Network.isClient )
        {
            Vector2 input = new Vector2( 
                Input.GetAxis("Horizontal"), 
                Input.GetAxis("Vertical") );

            if( Vector2.Distance( input, lastControl ) > minimumMovement )
            {
                _networkView.RPC( "Control",
                    RPCMode.Server,
                    _networkView.viewID,
                    input.x, input.y );

                if( targetPos!=null )
                {
                    transform.position = Vector3.Lerp(
                        transform.position,
                        targetPos.GetValueOrDefault(),
                        Time.deltaTime );
                }

                lastControl = input;
            }
        }

        if( !Network.isServer )
        {
            Control( _networkView.viewID,
                Input.GetAxis( "Horizontal" ), Input.GetAxis( "Vertical" ) );
        }

        transform.position += vel * Time.deltaTime;
    }

    [RPC]
    void SetPosition( NetworkViewID id, Vector3 pos )
    {
        if( id == _networkView.viewID )
            targetPos = pos;
    }

    [RPC]
    void Control( NetworkViewID id, float x, float y )
    {
        vel.x = x;
        vel.z = y;
    }
}
