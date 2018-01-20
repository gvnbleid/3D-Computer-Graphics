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
        public Vertex[] Vertices { get; set; }
        public Vertex[] VerticesInWorldSpace { get; set; }
        public Vertex[] VerticesInProjectionSpace { get; set; }

        public Triangle()
        {
            Vertices = new Vertex[3];
            VerticesInProjectionSpace = new Vertex[3];
            VerticesInWorldSpace = new Vertex[3];
        }
        public Triangle(Vector v1, Vector v2, Vector v3, Vector Normal)
        {
            Vertices = new Vertex[] { new Vertex(v1.DeepClone(), Normal), new Vertex(v2.DeepClone(), Normal), new Vertex(v3.DeepClone(), Normal) };
            VerticesInProjectionSpace = new Vertex[3];
            VerticesInWorldSpace = new Vertex[3];
        }

        public void Draw(ref byte[] colorArray, int stride, int bytesPerPixel)
        {
            // tak było :)
            // Drawing.DrawLine((int)Vertices[0].Position.X, (int)Vertices[0].Position.Y, (int)Vertices[1].Position.X, (int)Vertices[1].Position.Y, Colors.Black, ref colorArray, stride, bytesPerPixel);

            for (int i = 0; i < 3; ++i)
                Drawing.DrawLine((int)Vertices[i].Position.X, (int)Vertices[i].Position.Y, 1, (int)Vertices[(i+1)%3].Position.X, (int)Vertices[(i+1)%3].Position.Y, 1, Colors.Black, ref colorArray, stride, bytesPerPixel);

        }

        public void Fill(ref byte[] colorArray, int stride, int bytesPerPixel, Color c, List<Light> lights)
        {
            //multiplyByWorldMatrix
            //flat model
            Drawing.MultiplyColor(c, 1);
            Color newObjectColor = Colors.Gray;
            foreach (Light l in lights)
            {
                Vector toLight = l.Position - Vertices[0].Position;
                toLight.Normalize();
                double d = Vector.DotProduct(toLight, Vertices[0].Normal);
                Color newLightColor = Drawing.MultiplyColor(l.LightColor, d);
                newObjectColor = Drawing.AddColor(c, newLightColor);
            }

            sortVerticesAscendingByY(out Vertex[] sorted);
            if (sorted[1].Position.Y == sorted[2].Position.Y)
                fillBottomFlatTriangle(sorted, ref colorArray, stride, bytesPerPixel, newObjectColor);
            else if (sorted[0].Position.Y == sorted[1].Position.Y)
                fillTopFlatTriangle(sorted, ref colorArray, stride, bytesPerPixel, newObjectColor);
            else
            {
                double q = (sorted[1].Position.Y - sorted[0].Position.Y) / (sorted[2].Position.Y - sorted[0].Position.Y);
                Vector newPos = new Vector((int)(sorted[0].Position.X + ((double)(sorted[1].Position.Y - sorted[0].Position.Y) / (double)(sorted[2].Position.Y - sorted[0].Position.Y)) * (sorted[2].Position.X - sorted[0].Position.X)), sorted[1].Position.Y,
                    sorted[0].Position.Z * (1 - q) + sorted[2].Position.Z * q, 1);
                Vertex v4 = new Vertex(newPos, sorted[0].Normal);
                fillBottomFlatTriangle(new Vertex[] { sorted[0], sorted[1], v4 }, ref colorArray, stride, bytesPerPixel, newObjectColor);
                fillTopFlatTriangle(new Vertex[] { sorted[1], v4, sorted[2] }, ref colorArray, stride, bytesPerPixel, newObjectColor);
            }
        }

        public bool GetOrientation()
        {
            return (VerticesInProjectionSpace[1].Position.Y - VerticesInProjectionSpace[0].Position.Y) * (VerticesInProjectionSpace[2].Position.X - VerticesInProjectionSpace[1].Position.X) - 
                (VerticesInProjectionSpace[2].Position.Y - VerticesInProjectionSpace[1].Position.Y) * (VerticesInProjectionSpace[1].Position.X - VerticesInProjectionSpace[0].Position.X) > 0;
        }

        public void MultiplyByViewAndProjectionMatrix(Camera c)
        {
            int i = 0;
            foreach(Vertex v in Vertices)
            {
                Vector newPos =  c.ViewMatrix * v.Position;
                newPos = c.ProjectionMatrix * newPos;
                Vector newNorm = c.ViewMatrix * v.Normal;
                newNorm = c.ProjectionMatrix * newNorm;
                newNorm.Normalize();
                VerticesInProjectionSpace[i++] = new Vertex(newPos, newNorm);
            }
        }

        public bool TransformToScreenCoordinates(int width, int height)
        {
            foreach(Vertex v in VerticesInProjectionSpace)
            {
                if(v.Position[3] != 1)
                {
                    v.Position[0] /= v.Position[3];
                    v.Position[1] /= v.Position[3];
                    v.Position[2] /= v.Position[3];
                    v.Position[3] /= v.Position[3];
                }

                if (v.Position[0] < -1 || v.Position[0] > 1 || v.Position[1] < -1 || v.Position[1] > 1)
                    return false;

                int x = Math.Max(0, Math.Min(width-1, (int)((v.Position[0] + 1) * 0.5 * width)));
                int y = Math.Max(Math.Min(height - 1, (int)((v.Position[1] + 1) * 0.5 * height)), 0);
                double z = (v.Position[2] + 1) * 0.5;
                v.Position[0] = x;
                v.Position[1] = y;
                v.Position[2] = z;
            }
            return true;
        }

        private void sortVerticesAscendingByY(out Vertex[] tmp)
        {
            tmp = new Vertex[3];
            for (int i = 0; i < 3; i++)
                tmp[i] = new Vertex(VerticesInProjectionSpace[i].Position, VerticesInProjectionSpace[i].Normal);
            Array.Sort(tmp, (v1, v2) =>
            {
                if (v1.Position.Y < v2.Position.Y)
                    return -1;
                if (v1.Position.Y == v2.Position.Y)
                    return v1.Position.X.CompareTo(v2.Position.X);
                return 1;
            });
        }

        private void fillBottomFlatTriangle(Vertex[] Vertices, ref byte[] colorArray, int stride, int bytesPerPixel, Color c)
        {
            double invslope1 = (Vertices[1].Position.X - Vertices[0].Position.X) / (Vertices[1].Position.Y - Vertices[0].Position.Y);
            double invslope2 = (Vertices[2].Position.X - Vertices[0].Position.X) / (Vertices[2].Position.Y - Vertices[0].Position.Y);

            double curx1 = Vertices[0].Position.X;
            double curx2 = Vertices[0].Position.X;

            for (int scanlineY = (int)Vertices[0].Position.Y; scanlineY <= Vertices[1].Position.Y; scanlineY++)
            {
                double q = (double)(scanlineY - Vertices[0].Position.Y) / (double)(Vertices[1].Position.Y - Vertices[0].Position.Y);
                double z1 = Vertices[0].Position.Z * (1 - q) + Vertices[1].Position.Z * q;
                double z2 = Vertices[0].Position.Z * (1 - q) + Vertices[2].Position.Z * q;
                Drawing.DrawLine((int)curx1, scanlineY, z1, (int)curx2, scanlineY, z2, c, ref colorArray, stride, bytesPerPixel);
                curx1 += invslope1;
                curx2 += invslope2;
            }
        }

        private void fillTopFlatTriangle(Vertex[] Vertices, ref byte[] colorArray, int stride, int bytesPerPixel, Color c)
        {
            double invslope1 = (Vertices[2].Position.X - Vertices[0].Position.X) / (Vertices[2].Position.Y - Vertices[0].Position.Y);
            double invslope2 = (Vertices[2].Position.X - Vertices[1].Position.X) / (Vertices[2].Position.Y - Vertices[1].Position.Y);

            double curx1 = Vertices[2].Position.X;
            double curx2 = Vertices[2].Position.X;

            for (int scanlineY = (int)Vertices[2].Position.Y; scanlineY >= Vertices[0].Position.Y; scanlineY--)
            {
                double q = (double)(Vertices[2].Position.Y - scanlineY) / (double)(Vertices[2].Position.Y - Vertices[0].Position.Y);
                double z1 = Vertices[2].Position.Z * (1 - q) + Vertices[0].Position.Z * q;
                double z2 = Vertices[2].Position.Z * (1 - q) + Vertices[1].Position.Z * q;
                Drawing.DrawLine((int)curx1, scanlineY, z1, (int)curx2, scanlineY, z2, c, ref colorArray, stride, bytesPerPixel);
                curx1 -= invslope1;
                curx2 -= invslope2;
            }
        }

    }
}
