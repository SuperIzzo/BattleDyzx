using UnityEngine;

/*
public class DyzkPhysics : MonoBehaviour
{
    Vector3 _velocity;
    Vector3 _acceleration;


    //IArena arena;

    // Use this for initialization
    void Start ()
    {
        arena = FindObjectOfType<Arena>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        UpdateOnArena();
        return;        
    }

    private void UpdateOnArena()
    {
        _velocity += Physics.gravity * Time.fixedDeltaTime;

        transform.position += _velocity * Time.fixedDeltaTime;

        /*
        Vector3 position = transform.position;
        float terrainY = arena.SampleHeight( position ) + 1;

        if( position.y < terrainY )
        {
            position.y = terrainY;
            _velocity.y = 0;

            transform.position = position;
        }
        * /
    }
    

    private static Terrain GetTerrain()
    {
        return Terrain.activeTerrain;
    }
}
*/