using UnityEngine;

public class Spinning : MonoBehaviour
{
    public float RPM;
    private Material mat;

	void Start ()
    {
        mat = GetComponent<Renderer>().material;        
	}
	
	void Update ()
    {
        float deltaAngle = RPM * Time.deltaTime/60 * 360;

        mat.SetFloat( "_SpinBlur", -RPM / 1000 );
        transform.localRotation = Quaternion.Euler(0, deltaAngle, 0) * transform.localRotation;
    }
}
