using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Computer_Graphics.Geometry
{
    public class Vertex
    {
        public Vector Position { get; set; }
        public Vector Normal { get; set; }

        public Vertex() { }

        public Vertex(Vector position, Vector normal)
        {
            if (position.Dim != 4 || normal.Dim != 4)
                throw new FormatException("Dimension of both vectors must be 4");
            Position = position.DeepClone();
            Normal = normal.DeepClone();
        }

        public Vertex DeepClone()
        {
            return new Vertex(Position, Normal);
        }
    }
}
