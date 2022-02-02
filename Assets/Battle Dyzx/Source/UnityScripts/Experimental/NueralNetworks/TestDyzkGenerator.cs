using BattleDyzx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDyzkGenerator : MonoBehaviour
{
    public DyzkDatabase dyzkData;
    public UnityEngine.UI.Image image;

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

        //ganTest.TrainReal(imageDatas);
        //ganTest.TrainFake(5);

        var texture = new Texture2D(256, 256,TextureFormat.RGBA32, false, false);
        var imageData = new Texture2DImageData(texture);        

        ganTest.GenerateImage(imageData);
        texture.Apply();

        image.sprite = Sprite.Create(texture, new Rect(0, 0, 256, 256), Vector2.one / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
