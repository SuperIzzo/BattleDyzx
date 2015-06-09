using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CenterTerrain : MonoBehaviour {

	// Use this for initialization
	void Start()
    {
        Vector3 TS = GetComponent<Terrain>().terrainData.size;
        transform.position = new Vector3( -TS.x/2, 0, -TS.z/2 );
    }
	
	// Update is called once per frame
	//void Update () {
	
	//}
}
