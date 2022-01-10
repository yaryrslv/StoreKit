using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace StoreKit.Application.Services.LocalStorage
{
    public static class BitmapExtensions
    {
        public static void Rotate(this Bitmap bitmap)
        {
            var orientationProperty = bitmap.PropertyItems.FirstOrDefault(i => i.Id == 0x112);
            if (orientationProperty == null) return;

            int orientation;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Array.Reverse(orientationProperty.Value);
                orientation = BitConverter.ToInt16(orientationProperty.Value, 0);
            }
            else
            {
                orientation = BitConverter.ToInt16(orientationProperty.Value, 0);
            }

            if (orientation == 2) bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            if (orientation == 3) bitmap.RotateFlip(RotateFlipType.RotateNoneFlipXY);
            if (orientation == 4) bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            if (orientation == 5) bitmap.RotateFlip(RotateFlipType.Rotate90FlipX);
            if (orientation == 6) bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            if (orientation == 7) bitmap.RotateFlip(RotateFlipType.Rotate90FlipY);
            if (orientation == 8) bitmap.RotateFlip(RotateFlipType.Rotate90FlipXY);
        }
    }
}