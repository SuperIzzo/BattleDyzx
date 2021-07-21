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
    }
}