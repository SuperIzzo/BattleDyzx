namespace BattleDyzx
{
    static class CommonStaticUtility
    {
        public static Vector2D GetSize(this IImageData imageData)
        {
            return new Vector2D(imageData.width, imageData.height);
        }
    }
}