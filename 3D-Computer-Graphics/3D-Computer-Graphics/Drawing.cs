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
        public static void DrawLine(int x0, int y0, int x1, int y1, Color color, ref byte[] colorArray, int stride, int bytesPerPixel)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            for (;;)
            { 
                int index = y0 * stride + x0 * bytesPerPixel;
                if (index >= 0 && index < colorArray.Length)
                {
                    colorArray[index] = color.B;
                    colorArray[index + 1] = color.G;
                    colorArray[index + 2] = color.R;
                    colorArray[index + 3] = 255;
                }
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
            }
        }
    }
}
