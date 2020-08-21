using UnityEngine;
using System.Collections;

public class Spinning : MonoBehaviour
{
    public float RPM;
    private Material mat;


	// Use this for initialization
	void Start ()
    {
        mat = GetComponent<Renderer>().material;        
	}
	
	// Update is called once per frame
	void Update ()
    {
        float deltaAngle = RPM * Time.deltaTime/60 * 360;

        mat.SetFloat( "_SpinBlur", -RPM / 1000 );
        transform.Rotate( 0, deltaAngle, 0, Space.World );
    }
}
