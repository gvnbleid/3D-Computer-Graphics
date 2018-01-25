using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _3D_Computer_Graphics.Geometry;

namespace _3D_Computer_Graphics
{
    public static class Drawing
    {
        public static void DrawLine(int x0, int y0, double z0, int x1, int y1, double z1, Color color1, Color color2, ref byte[] colorArray, int stride, int bytesPerPixel)
        {
            int width = x1 - x0;
            int begining = x0;
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            for (;;)
            {
                double q = (double)(x0 - begining) / (double)width;
                int index = y0 * stride + x0 * bytesPerPixel;
                double z = z0 * (1 - q) + z1 * q;
                Color color = Color.FromArgb(255, (byte)(color1.R * (1 - q) + color2.R * q),
                    (byte)(color1.G * (1 - q) + color2.G * q),
                    (byte)(color1.B * (1 - q) + color2.B * q));
                if (index >= 0 && index < colorArray.Length)
                {
                    colorArray[index] = (byte)Math.Min(255,(int)color.B*z);
                    colorArray[index + 1] = (byte)Math.Min(255, (int)color.G * z);
                    colorArray[index + 2] = (byte)Math.Min(255, (int)color.R * z);
                    colorArray[index + 3] = 255;
                }
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
            }
        }

        public static Color MultiplyColor(Color c, double multiplier)
        {
            c.R = (byte)Math.Max(0,(c.R * multiplier));
            c.G = (byte)Math.Max(0, (c.G * multiplier));
            c.B = (byte)Math.Max(0, (c.B * multiplier));
            return Color.FromArgb(255, c.R, c.G, c.B);
        }

        public static Color AddColor(Color c1, Color c2)
        {
            c1.R = (byte)Math.Min(255, c1.R + c2.R);
            c1.G = (byte)Math.Min(255, c1.G + c2.G);
            c1.B = (byte)Math.Min(255, c1.B + c2.B);
            return Color.FromArgb(255, c1.R, c1.G, c1.B);
        }
    }
}
