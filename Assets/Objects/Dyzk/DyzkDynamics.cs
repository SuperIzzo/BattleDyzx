using UnityEngine;

public class DyzkDynamics : MonoBehaviour
{
    Vector3 _velocity;
    float   _angularVelocity;

    // Use this for initialization
    protected void Start ()
    {
	
	}
	
	// Update is called once per frame
	protected void FixedUpdate ()
    {
        transform.position += _velocity * Time.fixedDeltaTime;
	}
}
