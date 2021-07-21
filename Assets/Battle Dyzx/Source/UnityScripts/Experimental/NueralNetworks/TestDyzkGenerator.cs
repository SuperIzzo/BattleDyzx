using BattleDyzx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDyzkGenerator : MonoBehaviour
{
    public DyzkDatabase dyzkData;

    DyzkGAN ganTest;    

    // Start is called before the first frame update
    void Start()
    {
        ganTest = new DyzkGAN();
        ganTest.Build();

        IImageData[] imageDatas = new IImageData[dyzkData.dyzkTextures.Count];
        for( int i=0; i<imageDatas.Length; i++)
        {
            imageDatas[i] = new Texture2DImageData(dyzkData.dyzkTextures[i]);
        }

        ganTest.TrainReal(imageDatas);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
