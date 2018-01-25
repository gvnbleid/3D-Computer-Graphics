using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace _3D_Computer_Graphics
{
    public class Triangle
    {
        public Vertex[] Vertices { get; set; }
        public Vector Center { get; set; }
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
            Center = new Vector((Vertices[0].Position.X + Vertices[1].Position.X + Vertices[2].Position.X) / 3,
                (Vertices[0].Position.Y + Vertices[1].Position.Y + Vertices[2].Position.Y) / 3,
                (Vertices[0].Position.Z + Vertices[1].Position.Z + Vertices[2].Position.Z) / 3, 1);
            VerticesInProjectionSpace = new Vertex[3];
            VerticesInWorldSpace = new Vertex[3];
        }

        public void Draw(ref byte[] colorArray, int stride, int bytesPerPixel)
        {
            for (int i = 0; i < 3; ++i)
                Drawing.DrawLine((int)VerticesInProjectionSpace[i].Position.X, (int)VerticesInProjectionSpace[i].Position.Y, 1,
                    (int)VerticesInProjectionSpace[(i+1)%3].Position.X, (int)VerticesInProjectionSpace[(i+1)%3].Position.Y, 1,
                    Colors.LightBlue, ref colorArray, stride, bytesPerPixel);

        }

        private Color CalculateColorInVertex(Vector vertexPosition, Color c, List<Light> lights, Vector cameraPosition, double shinines)
        {
            Color finalColor = Drawing.MultiplyColor(c, Light.AmbientFactor);
            foreach (Light l in lights)
            {
                Color tmp = Colors.Black;
                Vector toLight = l.Position - vertexPosition;
                Vector toObserver = cameraPosition - vertexPosition;
                toLight.Normalize();
                toObserver.Normalize();
                double d = Vector.DotProduct(toLight, VerticesInWorldSpace[0].Normal);
                Vector reflection = VerticesInWorldSpace[0].Normal * 2 * d - toLight;
                reflection.Normalize();
                tmp = Drawing.AddColor(tmp, Drawing.MultiplyColor(c, d * l.DiffuseFactor));
                tmp = Drawing.AddColor(tmp, Drawing.MultiplyColor(l.LightColor, Math.Pow(Vector.DotProduct(reflection, toObserver), shinines) * l.SpecularFactor));
                double dist = Math.Sqrt(Math.Pow(toLight.X, 2) + Math.Pow(toLight.Y, 2) + Math.Pow(toLight.Z, 2));
                double attenuation = 1 / (1 + 0.09 * dist + 0.032 * Math.Pow(dist, 2));
                finalColor = Drawing.AddColor(finalColor, Drawing.MultiplyColor(tmp, attenuation));
            }
            return finalColor;
        }

        public void Fill(ref byte[] colorArray, int stride, int bytesPerPixel, Color c, List<Light> lights, Vector cameraPosition, double shinines)
        {
            //multiplyByWorldMatrix
            //flat model
            Color finalColor = Drawing.MultiplyColor(c, Light.AmbientFactor);
            foreach (Light l in lights)
            {
                Color tmp = Colors.Black;
                Vector toLight = l.Position - Center;
                Vector toObserver = cameraPosition - Center;
                toLight.Normalize();
                toObserver.Normalize();
                double d = Vector.DotProduct(toLight, VerticesInWorldSpace[0].Normal);
                Vector reflection = VerticesInWorldSpace[0].Normal * 2 * d - toLight;
                reflection.Normalize();
                tmp = Drawing.AddColor(tmp, Drawing.MultiplyColor(c, d * l.DiffuseFactor));
                tmp = Drawing.AddColor(tmp, Drawing.MultiplyColor(l.LightColor, Math.Pow(Vector.DotProduct(reflection, toObserver), shinines) * l.SpecularFactor));
                double dist = Math.Sqrt(Math.Pow(toLight.X, 2) + Math.Pow(toLight.Y, 2) + Math.Pow(toLight.Z, 2));
                double attenuation = 1 / (1 + 0.09 * dist + 0.032 * Math.Pow(dist, 2));
                finalColor = Drawing.AddColor(finalColor, Drawing.MultiplyColor(tmp, attenuation));
            }


            sortVerticesAscendingByY(out Vertex[] sorted);
            if (sorted[1].Position.Y == sorted[2].Position.Y)
                fillBottomFlatTriangle(sorted, ref colorArray, stride, bytesPerPixel, finalColor);
            else if (sorted[0].Position.Y == sorted[1].Position.Y)
                fillTopFlatTriangle(sorted, ref colorArray, stride, bytesPerPixel, finalColor);
            else
            {
                double q = (sorted[1].Position.Y - sorted[0].Position.Y) / (sorted[2].Position.Y - sorted[0].Position.Y);
                Vector newPos = new Vector((int)(sorted[0].Position.X + ((double)(sorted[1].Position.Y - sorted[0].Position.Y) / (double)(sorted[2].Position.Y - sorted[0].Position.Y)) * (sorted[2].Position.X - sorted[0].Position.X)), sorted[1].Position.Y,
                    sorted[0].Position.Z * (1 - q) + sorted[2].Position.Z * q, 1);
                Vertex v4 = new Vertex(newPos, sorted[0].Normal);
                fillBottomFlatTriangle(new Vertex[] { sorted[0], sorted[1], v4 }, ref colorArray, stride, bytesPerPixel, finalColor);
                fillTopFlatTriangle(new Vertex[] { sorted[1], v4, sorted[2] }, ref colorArray, stride, bytesPerPixel, finalColor);
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
            foreach(Vertex v in VerticesInWorldSpace)
            {
                Vector newPos =  c.ViewMatrix * v.Position;
                newPos = c.ProjectionMatrix * newPos;
                Vector newNorm = c.ViewMatrix * v.Normal;
                newNorm = c.ProjectionMatrix * newNorm;
                newNorm.Normalize();
                VerticesInProjectionSpace[i++] = new Vertex(newPos, newNorm);
            }
        }

        public void TransformToWorld(Matrix m)
        {
            for (int i = 0; i < 3; i++)
                VerticesInWorldSpace[i] = new Vertex(m * Vertices[i].Position, m * Vertices[i].Normal);
            Center = new Vector((VerticesInWorldSpace[0].Position.X + VerticesInWorldSpace[1].Position.X + VerticesInWorldSpace[2].Position.X) / 3,
                (VerticesInWorldSpace[0].Position.Y + VerticesInWorldSpace[1].Position.Y + VerticesInWorldSpace[2].Position.Y) / 3,
                (VerticesInWorldSpace[0].Position.Z + VerticesInWorldSpace[1].Position.Z + VerticesInWorldSpace[2].Position.Z) / 3, 1);
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
