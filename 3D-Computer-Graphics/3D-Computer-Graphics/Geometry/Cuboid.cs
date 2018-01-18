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
    public class Cuboid : ObjectListElement, IGeometry
    {
        public Vector Position { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public Vector[] Vertices { get; set; }
        public Vector[] Pixels { get; set; }

        public Cuboid()
        {
            Counter++;
            Title = "Cuboid " + Counter;
            Position = new Vector(0, 0, 0, 1);
            X = 100;
            Y = 100;
            Z = 100;

            //Vertices = new Vector[4];
            //Vertices[0] = new Vector(4, 0, 0, 0, 1);
            //Vertices[1] = new Vector(4, 100, 0, 0, 1);
            //Vertices[2] = new Vector(4, 0, 100, 0, 1);
            //Vertices[3] = new Vector(4, 0, 0, 100, 1);

            Vertices = new Vector[8];
            Vertices[0] = new Vector(Position.X - X / 2, Position.Y - Y / 2, Position.Z - Z / 2, 1);
            Vertices[1] = new Vector(Position.X + X / 2, Position.Y - Y / 2, Position.Z - Z / 2, 1);
            Vertices[2] = new Vector(Position.X + X / 2, Position.Y + Y / 2, Position.Z - Z / 2, 1);
            Vertices[3] = new Vector(Position.X - X / 2, Position.Y + Y / 2, Position.Z - Z / 2, 1);
            Vertices[4] = new Vector(Position.X - X / 2, Position.Y + Y / 2, Position.Z + Z / 2, 1);
            Vertices[5] = new Vector(Position.X + X / 2, Position.Y + Y / 2, Position.Z + Z / 2, 1);
            Vertices[6] = new Vector(Position.X + X / 2, Position.Y - Y / 2, Position.Z + Z / 2, 1);
            Vertices[7] = new Vector(Position.X - X / 2, Position.Y - Y / 2, Position.Z + Z / 2, 1);
            Pixels = new Vector[Vertices.Length];
        }

        public void Draw(ref byte[] colorArray, Matrix view, int width, int height, int stride, int bytesPerPixel)
        {
            double fov = Math.PI / 2;
            double scale = 1 / Math.Tan(fov / 2);
            double aspect = (double)width / height;
            Matrix MProj = new Matrix(4, 4);
            double near = 0.1;
            double far = 1000;
            MProj[0, 0] = scale / aspect;
            MProj[1, 1] = scale;
            MProj[2, 2] = -(far + near) / (far - near);
            MProj[2, 3] = -2 * far * near / (far - near);
            MProj[3, 2] = -1;

            int index = 0;

            foreach (Vector v in Vertices)
            {
                Vector vv = view * v;
                Vector vvv = MProj * vv;
                if (vvv[3] != 1)
                {
                    vvv[0] /= vvv[3];
                    vvv[1] /= vvv[3];
                    vvv[2] /= vvv[3];
                    vvv[3] /= vvv[3];
                }

                if (vvv[0] < -1 || vvv[0] > 1 || vvv[1] < -1 || vvv[1] > 1) continue;

                int x = Math.Min(width - 1, (int)((vvv[0] + 1) * 0.5 * width));
                int y = Math.Max(Math.Min(height - 1, (int)((vvv[1] + 1) * 0.5 * height)), 0);
                double z = (vvv[2] + 1) * 0.5;
                Pixels[index++] = new Vector(x, y, z);
            }
            InitTriangles(out Triangle[] triangles);
            foreach (Triangle t in triangles)
            {
                if (!t.GetOrientation())
                {
                    t.Draw(ref colorArray, stride, bytesPerPixel);
                    t.Fill(ref colorArray, stride, bytesPerPixel);
                }
            }
            //Point[] p = Pixels;
            //Drawing.DrawLine((int)p[0].X, (int)p[0].Y, (int)p[1].X, (int)p[1].Y, Colors.Blue, ref colorArray, stride, bytesPerPixel);
            //Drawing.DrawLine((int)p[0].X, (int)p[0].Y, (int)p[2].X, (int)p[2].Y, Colors.Blue, ref colorArray, stride, bytesPerPixel);
            //Drawing.DrawLine((int)p[0].X, (int)p[0].Y, (int)p[3].X, (int)p[3].Y, Colors.Blue, ref colorArray, stride, bytesPerPixel);
            //Drawing.DrawLine((int)p[3].X, (int)p[3].Y, (int)p[4].X, (int)p[4].Y, Colors.Blue, ref colorArray, stride, bytesPerPixel);
            //Drawing.DrawLine((int)p[3].X, (int)p[3].Y, (int)p[6].X, (int)p[6].Y, Colors.Blue, ref colorArray, stride, bytesPerPixel);
            //Drawing.DrawLine((int)p[4].X, (int)p[4].Y, (int)p[7].X, (int)p[7].Y, Colors.Blue, ref colorArray, stride, bytesPerPixel);
            //Drawing.DrawLine((int)p[6].X, (int)p[6].Y, (int)p[7].X, (int)p[7].Y, Colors.Blue, ref colorArray, stride, bytesPerPixel);
            //Drawing.DrawLine((int)p[1].X, (int)p[1].Y, (int)p[5].X, (int)p[5].Y, Colors.Blue, ref colorArray, stride, bytesPerPixel);
            //Drawing.DrawLine((int)p[1].X, (int)p[1].Y, (int)p[6].X, (int)p[6].Y, Colors.Blue, ref colorArray, stride, bytesPerPixel);
            //Drawing.DrawLine((int)p[2].X, (int)p[2].Y, (int)p[5].X, (int)p[5].Y, Colors.Blue, ref colorArray, stride, bytesPerPixel);
            //Drawing.DrawLine((int)p[2].X, (int)p[2].Y, (int)p[4].X, (int)p[4].Y, Colors.Blue, ref colorArray, stride, bytesPerPixel);
            //Drawing.DrawLine((int)p[5].X, (int)p[5].Y, (int)p[7].X, (int)p[7].Y, Colors.Blue, ref colorArray, stride, bytesPerPixel);
        }

        private void InitTriangles(out Triangle[] triangles)
        {
            //triangles = new Triangle[3];
            //triangles[0] = new Triangle(Pixels[0], Pixels[1], Pixels[2]);
            //triangles[1] = new Triangle(Pixels[0], Pixels[2], Pixels[3]);
            //triangles[2] = new Triangle(Pixels[1], Pixels[3], Pixels[0]);

            triangles = new Triangle[12];
            triangles[0] = new Triangle(Pixels[0], Pixels[2], Pixels[1]);
            triangles[1] = new Triangle(Pixels[0], Pixels[3], Pixels[2]);
            triangles[2] = new Triangle(Pixels[2], Pixels[3], Pixels[4]);
            triangles[3] = new Triangle(Pixels[2], Pixels[4], Pixels[5]);
            triangles[4] = new Triangle(Pixels[1], Pixels[2], Pixels[5]);
            triangles[5] = new Triangle(Pixels[1], Pixels[5], Pixels[6]);
            triangles[6] = new Triangle(Pixels[0], Pixels[7], Pixels[4]);
            triangles[7] = new Triangle(Pixels[0], Pixels[4], Pixels[3]);
            triangles[8] = new Triangle(Pixels[5], Pixels[4], Pixels[7]);
            triangles[9] = new Triangle(Pixels[5], Pixels[7], Pixels[6]);
            triangles[10] = new Triangle(Pixels[0], Pixels[6], Pixels[7]);
            triangles[11] = new Triangle(Pixels[0], Pixels[1], Pixels[6]);
        }
    }
}
