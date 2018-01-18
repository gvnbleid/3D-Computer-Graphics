using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace _3D_Computer_Graphics.Geometry
{
    public class Triangle
    {
        public Vector[] Vertices { get; set; }

        public Triangle()
        {
            Vertices = new Vector[3];
        }
        public Triangle(Vector p1, Vector p2, Vector p3)
        {
            Vertices = new Vector[] { p1, p2, p3 };
        }

        public void Draw(ref byte[] colorArray, int stride, int bytesPerPixel)
        {
            // tak było :)
            // Drawing.DrawLine((int)Vertices[0].X, (int)Vertices[0].Y, (int)Vertices[1].X, (int)Vertices[1].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);

            for (int i = 0; i < 3; ++i)
                Drawing.DrawLine((int)Vertices[i].X, (int)Vertices[i].Y, (int)Vertices[(i+1)%3].X, (int)Vertices[(i+1)%3].Y, Colors.Black, ref colorArray, stride, bytesPerPixel);

        }

        public void Fill(ref byte[] colorArray, int stride, int bytesPerPixel)
        {
            sortVerticesAscendingByY(out Vector[] sorted);
            if (sorted[1].Y == sorted[2].Y)
                fillBottomFlatTriangle(sorted, ref colorArray, stride, bytesPerPixel);
            else if (sorted[0].Y == sorted[1].Y)
                fillTopFlatTriangle(sorted, ref colorArray, stride, bytesPerPixel);
            else
            {
                Vector v4 = new Vector((int)(sorted[0].X + ((double)(sorted[1].Y - sorted[0].Y) / (double)(sorted[2].Y - sorted[0].Y)) * (sorted[2].X - sorted[0].X)), sorted[1].Y);
                fillBottomFlatTriangle(new Vector[] { sorted[0], sorted[1], v4 }, ref colorArray, stride, bytesPerPixel);
                fillTopFlatTriangle(new Vector[] { sorted[1], v4, sorted[2] }, ref colorArray, stride, bytesPerPixel);
            }
        }

        public bool GetOrientation()
        {
            if (Vertices.Contains(null))
                return true;
            return (Vertices[1].Y - Vertices[0].Y) * (Vertices[2].X - Vertices[1].X) - (Vertices[2].Y - Vertices[1].Y) * (Vertices[1].X - Vertices[0].X) > 0;
        }

        public void MultiplyByViewAndProjectionMatrix(Scene.Camera c)
        {
            foreach(Vector v in Vertices)
            {
                v.Transform(c.ViewMatrix);
                v.Transform(c.ProjectionMatrix);
            }
        }

        public void TransformToScreenCoordinates(int width, int height)
        {
            foreach(Vector v in Vertices)
            {
                if(v[3] != 1)
                {
                    v[0] /= v[3];
                    v[1] /= v[3];
                    v[2] /= v[3];
                    v[3] /= v[3];
                }

                if (v[0] < -1 || v[0] > 1 || v[1] < -1 || v[1] > 1) continue;

                int x = Math.Min(width - 1, (int)((v[0] + 1) * 0.5 * width));
                int y = Math.Max(Math.Min(height - 1, (int)((v[1] + 1) * 0.5 * height)), 0);
                double z = (v[2] + 1) * 0.5;
            }
        }

        private void sortVerticesAscendingByY(out Vector[] tmp)
        {
            tmp = new Vector[3];
            for (int i = 0; i < 3; i++)
                tmp[i] = new Vector(Vertices[i].X, Vertices[i].Y, Vertices[i].Z);
            Array.Sort(tmp, (v1, v2) =>
            {
                if (v1.Y < v2.Y)
                    return -1;
                if (v1.Y == v2.Y)
                    return v1.X.CompareTo(v2.X);
                return 1;
            });
        }

        private void fillBottomFlatTriangle(Vector[] Vertices, ref byte[] colorArray, int stride, int bytesPerPixel)
        {
            double invslope1 = (Vertices[1].X - Vertices[0].X) / (Vertices[1].Y - Vertices[0].Y);
            double invslope2 = (Vertices[2].X - Vertices[0].X) / (Vertices[2].Y - Vertices[0].Y);

            double curx1 = Vertices[0].X;
            double curx2 = Vertices[0].X;

            for (int scanlineY = (int)Vertices[0].Y; scanlineY <= Vertices[1].Y; scanlineY++)
            {
                //drawLine((int)curx1, scanlineY, (int)curx2, scanlineY);
                //do zastąpienia przez rysowanie pojedynczych pixeli
                Drawing.DrawLine((int)curx1, scanlineY, (int)curx2, scanlineY, Colors.Blue, ref colorArray, stride, bytesPerPixel);
                curx1 += invslope1;
                curx2 += invslope2;
            }
        }

        private void fillTopFlatTriangle(Vector[] Vertices, ref byte[] colorArray, int stride, int bytesPerPixel)
        {
            double invslope1 = (Vertices[2].X - Vertices[0].X) / (Vertices[2].Y - Vertices[0].Y);
            double invslope2 = (Vertices[2].X - Vertices[1].X) / (Vertices[2].Y - Vertices[1].Y);

            double curx1 = Vertices[2].X;
            double curx2 = Vertices[2].X;

            for (int scanlineY = (int)Vertices[2].Y; scanlineY >= Vertices[0].Y; scanlineY--)
            {
                //drawLine((int)curx1, scanlineY, (int)curx2, scanlineY);
                //do zastąpienia przez rysowanie pojedynczych pixeli
                Drawing.DrawLine((int)curx1, scanlineY, (int)curx2, scanlineY, Colors.Blue, ref colorArray, stride, bytesPerPixel);
                curx1 -= invslope1;
                curx2 -= invslope2;
            }
        }

    }
}
