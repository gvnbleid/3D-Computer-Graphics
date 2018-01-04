using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Computer_Graphics.Scene
{
    public class Camera
    {
        public Vector Position { get; set; }
        public Vector Target { get; set; }
        public Vector Direction { get; set; }
        public Vector Right { get; set; }
        public Vector UpWorld { get; set; }
        public Vector Up { get; set; }
        public Matrix View { get; set; }

        public Camera()
        {
            Position = new Vector(3, new double[] { 0, 0, 100 });
            Target = new Vector(3, new double[] { 0, 0, 0 });
            Direction = Position - Target;
            Direction.Normalize();
            UpWorld = new Vector(3, 0, 1, 0);
            Right = Vector.CrossProduct(UpWorld, Direction);
            Up = Vector.CrossProduct(Direction, Right);
            Matrix tmp1 = new Matrix(4, 4, Right.X, Right.Y, Right.Z, 0,
                Up.X, Up.Y, Up.Z, 0,
                Direction.X, Direction.Y, Direction.Z, 0,
                0, 0, 0, 1);
            Matrix tmp2 = new Matrix(4, 4, 1, 0, 0, -Position.X,
                0, 1, 0, -Position.Y,
                0, 0, 1, -Position.Z,
                0, 0, 0, 1);
            View = tmp1 * tmp2;
        }
    }
}
