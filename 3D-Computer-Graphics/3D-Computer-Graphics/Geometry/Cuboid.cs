using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            double scale = 1 / Math.Tan(45 * 0.5 * Math.PI / 180);
            Matrix MProj = new Matrix(4, 4);
            MProj[0, 0] = scale;
            MProj[1, 1] = scale;
            MProj[2, 2] = -100 / (100 - 0.1);
            MProj[3, 2] = -100 * 0.1 / (100 - 0.1);
            MProj[2, 3] = -1;

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
                int y = Math.Min(height - 1, (int)(1 - (vvv[1] + 1) * 0.5 * height));
                colorArray[y * stride + x * bytesPerPixel] = 255;
            }

            colorArray[100 * stride + 3 * bytesPerPixel] = 0;
            colorArray[100 * stride + 3 * bytesPerPixel + 1] = 0;
            colorArray[100 * stride + 3 * bytesPerPixel + 2] = 0;
            colorArray[100 * stride + 3 * bytesPerPixel + 3] = 0;
            colorArray[100 * stride + 4 * bytesPerPixel] = 0;
            colorArray[100 * stride + 5 * bytesPerPixel] = 0;
            colorArray[100 * stride + 6 * bytesPerPixel] = 0;
            colorArray[100 * stride + 7 * bytesPerPixel] = 0;

        }
    }
}
