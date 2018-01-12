using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace _3D_Computer_Graphics.Geometry
{
    public class Triangle : IGeometry
    {
        public Point[] Vertices;

        public Triangle()
        {
            Vertices = new Point[3];
        }
        public Triangle(Point p1, Point p2, Point p3)
        {
            Vertices = new Point[] { p1, p2, p3 };
        }

        public void Draw(ref byte[] colorArray, Matrix View, int width, int height, int stride, int bytesPerPixel)
        {
            // tak było :)
            // Drawing.DrawLine((int)Vertices[0].X, (int)Vertices[0].Y, (int)Vertices[1].X, (int)Vertices[1].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);

            for (int i = 0; i < 3; ++i)
                Drawing.DrawLine((int)Vertices[i].X, (int)Vertices[i].Y, (int)Vertices[(i+1)%3].X, (int)Vertices[(i+1)%3].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);

        }
    }
}
