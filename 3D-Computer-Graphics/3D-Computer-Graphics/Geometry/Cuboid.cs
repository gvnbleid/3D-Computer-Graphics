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

namespace _3D_Computer_Graphics.Geometry
{
    public class Cuboid : IGeometry
    {
        public Vector Position { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public Vector[] Vertices { get; set; }

        public Cuboid()
        {
            Position = new Vector(4, 0, 0, 0, 1);
            X = 100;
            Y = 100;
            Z = 100;
            Vertices = new Vector[8];
            Vertices[0] = new Vector(4, Position.X - X / 2, Position.Y - Y / 2, Position.Z - Z / 2, 1);
            Vertices[1] = new Vector(4, Position.X - X / 2, Position.Y - Y / 2, Position.Z + Z / 2, 1);
            Vertices[2] = new Vector(4, Position.X - X / 2, Position.Y + Y / 2, Position.Z - Z / 2, 1);
            Vertices[3] = new Vector(4, Position.X + X / 2, Position.Y - Y / 2, Position.Z - Z / 2, 1);
            Vertices[4] = new Vector(4, Position.X + X / 2, Position.Y + Y / 2, Position.Z - Z / 2, 1);
            Vertices[5] = new Vector(4, Position.X - X / 2, Position.Y + Y / 2, Position.Z + Z / 2, 1);
            Vertices[6] = new Vector(4, Position.X + X / 2, Position.Y - Y / 2, Position.Z + Z / 2, 1);
            Vertices[7] = new Vector(4, Position.X + X / 2, Position.Y + Y / 2, Position.Z + Z / 2, 1);
        }

        public void Draw(ref byte[] colorArray, Matrix view, int width, int height, int stride, int bytesPerPixel)
        {
            double fov = Math.PI / 2;
            double scale = 1 / Math.Tan(fov / 2);
            double aspect = (double)width / height;
            Matrix MProj = new Matrix(4, 4);
            double near = 0.1;
            double far = 1000;
            MProj[0, 0] = scale/aspect;
            MProj[1, 1] = scale;
            MProj[2, 2] = -(far + near) / (far - near);
            MProj[2, 3] = -2 * far * near / (far - near);
            MProj[3, 2] = -1;

            Point[] p = new Point[8];
            int index = 0;

            foreach(Vector v in Vertices)
            {
                Vector vv = view * v;
                Vector vvv = MProj * vv;
                if(vvv[3]!=1)
                {
                    vvv[0] /= vvv[3];
                    vvv[1] /= vvv[3];
                    vvv[2] /= vvv[3];
                    vvv[3] /= vvv[3];
                }

                if (vvv[0] < -1 || vvv[0] > 1 || vvv[1] < -1 || vvv[1] > 1) continue;

                int x = Math.Min(width - 1, (int)((vvv[0] + 1) * 0.5 * width));
                int y = Math.Max(Math.Min(height - 1, (int)((vvv[1] + 1) * 0.5 * height)),0);
                p[index++] = new Point(x, y);
                colorArray[y * stride + x * bytesPerPixel] = 255;
                colorArray[y * stride + x * bytesPerPixel + 3] = 255;
            }
            Drawing.DrawLine((int)p[0].X, (int)p[0].Y, (int)p[1].X, (int)p[1].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);
            Drawing.DrawLine((int)p[0].X, (int)p[0].Y, (int)p[2].X, (int)p[2].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);
            Drawing.DrawLine((int)p[0].X, (int)p[0].Y, (int)p[3].X, (int)p[3].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);
            Drawing.DrawLine((int)p[3].X, (int)p[3].Y, (int)p[4].X, (int)p[4].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);
            Drawing.DrawLine((int)p[3].X, (int)p[3].Y, (int)p[6].X, (int)p[6].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);
            Drawing.DrawLine((int)p[4].X, (int)p[4].Y, (int)p[7].X, (int)p[7].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);
            Drawing.DrawLine((int)p[6].X, (int)p[6].Y, (int)p[7].X, (int)p[7].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);
            Drawing.DrawLine((int)p[1].X, (int)p[1].Y, (int)p[5].X, (int)p[5].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);
            Drawing.DrawLine((int)p[1].X, (int)p[1].Y, (int)p[6].X, (int)p[6].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);
            Drawing.DrawLine((int)p[2].X, (int)p[2].Y, (int)p[5].X, (int)p[5].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);
            Drawing.DrawLine((int)p[2].X, (int)p[2].Y, (int)p[4].X, (int)p[4].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);
            Drawing.DrawLine((int)p[5].X, (int)p[5].Y, (int)p[7].X, (int)p[7].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);



        }
    }
}
