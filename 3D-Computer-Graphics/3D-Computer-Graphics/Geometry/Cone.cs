using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _3D_Computer_Graphics.Geometry
{
    public class Cone : Geometry, IGeometry
    {
        private static int Counter { get; set; } = -1;

        public Cone() { }

        public Cone(Vector position, Vector rotation, int width, int height, int length)
        {
            Counter++;
            Title = "Cone " + Counter;
            Position = position.DeepClone();
            Position.Y = Position.Y;
            Rotation = position.DeepClone();
            Width = width;
            Height = height;
            Length = length;
            ObjectColor = Colors.Gray;

            Vector[] vertices = new Vector[22];
            double angle = 0.0;
            for(int i=0; i<20;i++)
            {
                vertices[i] = new Vector(Position.X + Math.Cos(angle) * width,
                    0, Position.Z + Math.Sin(angle) * length, 1);
                angle += 1 / 20.0 * Math.PI * 2;
            }
            vertices[20] = new Vector(Position.X, Position.Y - height, Position.Z, 1);

            InitTriangles(vertices);
            TransformToWorld();
        }

        private void InitTriangles(Vector[] vertices)
        {
            TrianglesGrid = new List<Triangle>();
            for(int i=0;i<19;i++)
            {
                Vector tmp = Vector.CrossProduct(vertices[20] - vertices[i + 1], vertices[20] - vertices[i]);
                tmp.Normalize();
                Vector normal = new Vector(tmp.X, tmp.Y, tmp.Z, 0);
                TrianglesGrid.Add(new Triangle(vertices[i], vertices[i + 1], Position, new Vector(0, -1, 0, 0)));
                TrianglesGrid.Add(new Triangle(vertices[i], vertices[i + 1], vertices[20], normal));
            }
        }
    }
}
