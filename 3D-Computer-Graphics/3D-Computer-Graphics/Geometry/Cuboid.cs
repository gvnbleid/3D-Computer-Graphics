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
    }
}
