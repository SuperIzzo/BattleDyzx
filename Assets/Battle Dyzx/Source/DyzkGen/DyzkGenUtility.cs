using ConvNetSharp.Volume;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BattleDyzx
{
    static class DyzkGenUtility 
    {
        private static T ToT<T>(object o) where T : struct, IEquatable<T>, IFormattable
        {
            return (T)Convert.ChangeType(o, typeof(T));
        }

        private static float ToFloat(object o)
        {
            return (float)Convert.ChangeType(o, typeof(float));
        }

        public static void SetImageData<T>(this Volume<T> volume, int batchIndex, IImageData imageData) where T : struct, IEquatable<T>, IFormattable
        {
            for (int x = 0; x < imageData.width; x++)
            {
                for (int y = 0; y < imageData.height; y++)
                {
                    ColorRGBA col = imageData.GetPixel(x, y);

                    volume.Set(x, y, 0, batchIndex, ToT<T>(col.r));
                    volume.Set(x, y, 1, batchIndex, ToT<T>(col.g));
                    volume.Set(x, y, 2, batchIndex, ToT<T>(col.b));
                    volume.Set(x, y, 3, batchIndex, ToT<T>(col.a));
                }
            }
        }

        public static void SetImageData<T>(this Volume<T> volume, IImageData imageData) where T : struct, IEquatable<T>, IFormattable
        {
            SetImageData(volume, 0, imageData);
        }

        public static void ToImageData<T>(this Volume<T> volume, int batchIndex, IImageData imageData) where T : struct, IEquatable<T>, IFormattable
        {
            for (int x = 0; x < imageData.width; x++)
            {
                for (int y = 0; y < imageData.height; y++)
                {
                    ColorRGBA col = imageData.GetPixel(x, y);

                    col.r = Math.Clamp01(ToFloat(volume.Get(x, y, 0, batchIndex)));
                    col.g = Math.Clamp01(ToFloat(volume.Get(x, y, 1, batchIndex)));
                    col.b = Math.Clamp01(ToFloat(volume.Get(x, y, 2, batchIndex)));
                    col.a = Math.Clamp01(ToFloat(volume.Get(x, y, 3, batchIndex)));

                    imageData.SetPixel(x, y, col);
                }
            }
        }

        public static void ToImageData<T>(this Volume<T> volume, IImageData imageData) where T : struct, IEquatable<T>, IFormattable
        {
            ToImageData(volume, 0, imageData);
        }
    }
}