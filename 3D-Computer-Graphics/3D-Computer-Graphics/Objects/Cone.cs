using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _3D_Computer_Graphics
{
    public class Cone : GeometryObject
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

            Vector[] vertices = new Vector[37];
            double angle = 0.0;
            for(int i=0; i<36;i++)
            {
                vertices[i] = new Vector(Position.X + Math.Cos(angle)/2,
                    Position.Y + 0.5, Position.Z + Math.Sin(angle)/2, 1);
                angle += 1 / 36.0 * Math.PI * 2;
            }
            vertices[36] = new Vector(Position.X, Position.Y - 0.5, Position.Z, 1);

            InitTriangles(vertices);
            TransformToWorld();
        }

        private void InitTriangles(Vector[] vertices)
        {
            TrianglesGrid = new List<Triangle>();
            Vector center = new Vector(Position.X, Position.Y + 0.5, Position.Z, 1);
            for(int i=0;i<36;i++)
            {
                Vector tmp = Vector.CrossProduct(vertices[36] - vertices[i], vertices[36] - vertices[i+1]);
                tmp.Normalize();
                Vector normal = new Vector(-tmp.X, -tmp.Y, -tmp.Z, 0);
                TrianglesGrid.Add(new Triangle(vertices[(i+1)%36], vertices[i], center, new Vector(0, 1, 0, 0)));
                TrianglesGrid.Add(new Triangle(vertices[i], vertices[(i + 1) % 36], vertices[36], normal));
            }
        }
    }
}
