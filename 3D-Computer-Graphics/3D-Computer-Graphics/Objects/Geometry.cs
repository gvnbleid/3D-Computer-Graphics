using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _3D_Computer_Graphics
{
    public abstract class GeometryObject : ObjectListElement
    {
        public List<Triangle> TrianglesGrid { get; set; }
        public Color ObjectColor { get; set; }
        public double Shininess { get; set; }

        public void Draw(ref byte[] colorArray, Camera c, List<Light> l, int width, int height, int stride, int bytesPerPixel, bool fill, bool backfaceCulling, bool flat)
        {
            foreach (Triangle t in TrianglesGrid)
            {
                t.MultiplyByViewAndProjectionMatrix(c);
                if (t.TransformToScreenCoordinates(width, height))
                {
                    if (!t.GetOrientation() || !backfaceCulling)
                    {
                        if (fill)
                            t.Fill(ref colorArray, stride, bytesPerPixel, ObjectColor, l, c.Position, Shininess, flat);
                        else
                            t.Draw(ref colorArray, stride, bytesPerPixel);
                    }
                }
            }
        }

        public void TransformToWorld()
        {
            Matrix scaleMatrix = new Matrix(4, 4, Width, 0, 0, 0,
                0, Height, 0, 0,
                0, 0, Length, 0,
                0, 0, 0, 1);
            Matrix translationMatrix = new Matrix(4, 4, 1, 0, 0, Position.X,
                0, 1, 0, Position.Y,
                0, 0, 1, Position.Z,
                0, 0, 0, 1);
            double cosX = Math.Cos(Rotation.X / 180 * Math.PI);
            double sinX = Math.Sin(Rotation.X / 180 * Math.PI);
            double cosY = Math.Cos(Rotation.Y / 180 * Math.PI);
            double sinY = Math.Sin(Rotation.Y / 180 * Math.PI);
            double cosZ = Math.Cos(Rotation.Z / 180 * Math.PI);
            double sinZ = Math.Sin(Rotation.Z / 180 * Math.PI);
            Matrix rotationXMatrix = new Matrix(4, 4, 1, 0, 0, 0,
                0, cosX, -sinX, 0,
                0, sinX, cosX, 0,
                0, 0, 0, 1);
            Matrix rotationYMatrix = new Matrix(4, 4, cosY, 0, sinY, 0,
                0, 1, 0, 0,
                -sinY, 0, cosY, 0,
                0, 0, 0, 1);
            Matrix rotationZMatrix = new Matrix(4, 4, cosZ, -sinZ, 0, 0,
                sinZ, cosZ, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
            Matrix m = scaleMatrix * translationMatrix * rotationXMatrix * rotationYMatrix * rotationZMatrix;
            foreach (Triangle t in TrianglesGrid)
                t.TransformToWorld(m);
        }

        public override void Actualize()
        {
            TransformToWorld();
        }
    }
}
