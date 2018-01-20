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
    public class Cuboid : Geometry, IGeometry
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Length { get; set; }
        private static int Counter { get; set; } = -1;

        public Cuboid(Vector position, Vector rotation, int width, int height, int length)
        {
            Counter++;
            Title = "Cuboid " + Counter;
            Position = position.DeepClone();
            Rotation = position.DeepClone();
            Width = width;
            Height = height;
            Length = length;
            ObjectColor = Colors.Gray;

            Vector[] vertices = new Vector[8];
            vertices[0] = new Vector(Position.X - Width, Position.Y - Height, Position.Z - Length, 1);
            vertices[1] = new Vector(Position.X + Width, Position.Y - Height, Position.Z - Length, 1);
            vertices[2] = new Vector(Position.X + Width, Position.Y + Height, Position.Z - Length, 1);
            vertices[3] = new Vector(Position.X - Width, Position.Y + Height, Position.Z - Length, 1);
            vertices[4] = new Vector(Position.X - Width, Position.Y + Height, Position.Z + Length, 1);
            vertices[5] = new Vector(Position.X + Width, Position.Y + Height, Position.Z + Length, 1);
            vertices[6] = new Vector(Position.X + Width, Position.Y - Height, Position.Z + Length, 1);
            vertices[7] = new Vector(Position.X - Width, Position.Y - Height, Position.Z + Length, 1);
            InitTriangles(vertices);
            TransformToWorld();
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

        public void Draw(ref byte[] colorArray, Camera c, List<Light> l, int width, int height, int stride, int bytesPerPixel)
        {
            foreach (Triangle t in TrianglesGrid)
            { 
                t.MultiplyByViewAndProjectionMatrix(c);
                if (t.TransformToScreenCoordinates(width, height))
                {
                    if (!t.GetOrientation())
                        t.Fill(ref colorArray, stride, bytesPerPixel, ObjectColor, l);
                }
            }
        }

        private void InitTriangles(Vector[] vertices)
        {
            TrianglesGrid = new List<Triangle>();
            TrianglesGrid.Add(new Triangle(vertices[0], vertices[2], vertices[1], new Vector(0, 0, -1, 0)));
            TrianglesGrid.Add(new Triangle(vertices[0], vertices[3], vertices[2], new Vector(0, 0, -1, 0)));
            TrianglesGrid.Add(new Triangle(vertices[2], vertices[3], vertices[4], new Vector(0, 1, 0, 0)));
            TrianglesGrid.Add(new Triangle(vertices[2], vertices[4], vertices[5], new Vector(0, 1, 0, 0)));
            TrianglesGrid.Add(new Triangle(vertices[1], vertices[2], vertices[5], new Vector(1, 0, 0, 0)));
            TrianglesGrid.Add(new Triangle(vertices[1], vertices[5], vertices[6], new Vector(1, 0, 0, 0)));
            TrianglesGrid.Add(new Triangle(vertices[0], vertices[7], vertices[4], new Vector(-1, 0, 0, 0)));
            TrianglesGrid.Add(new Triangle(vertices[0], vertices[4], vertices[3], new Vector(-1, 0, 0, 0)));
            TrianglesGrid.Add(new Triangle(vertices[5], vertices[4], vertices[7], new Vector(0, 0, 1, 0)));
            TrianglesGrid.Add(new Triangle(vertices[5], vertices[7], vertices[6], new Vector(0, 0, 1, 0)));
            TrianglesGrid.Add(new Triangle(vertices[0], vertices[6], vertices[7], new Vector(0, -1, 0, 0)));
            TrianglesGrid.Add(new Triangle(vertices[0], vertices[1], vertices[6], new Vector(0, -1, 0, 0)));
        }

        public override void Actualize()
        {
            TransformToWorld();
        }
    }
}
