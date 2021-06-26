using BattleDyzx;
using UnityEngine;

public class Texture2DImageData : IImageData
{
    Texture2D texture;

    public int width => texture.width;

    public int height => texture.height;

    public Texture2DImageData(Texture2D texture)
    {
        this.texture = texture;
    }

    public ColorRGBA GetPixel(int x, int y)
    {
        return texture.GetPixel(x, y).ToBDXColorRBGA();
    }

    public void SetPixel(int x, int y, ColorRGBA color)
    {
        texture.SetPixel(x, y, color.ToUnityColor());
    }
}
