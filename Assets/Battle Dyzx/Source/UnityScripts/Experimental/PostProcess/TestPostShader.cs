using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class TestPostShader : MonoBehaviour
{
    public Material DMmat; //datamosh material

    void Start()
    {
        this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.MotionVectors;
        //generate the motion vector texture @ '_CameraMotionVectorsTexture'
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, DMmat);
    }
}
