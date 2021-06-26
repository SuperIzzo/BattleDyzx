namespace BattleDyzx
{
    public interface IImageData
    {
        int width { get; }
        int height { get; }        

        ColorRGBA GetPixel(int x, int y);
        void SetPixel(int x, int y, ColorRGBA color);
    }
}
