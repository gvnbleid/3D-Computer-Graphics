using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Computer_Graphics.Scene
{
    public class Camera : ObjectListElement
    { 
        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }

        public Camera(Vector Position, Vector Target, double nearClippingPlane, double farClippingPlane, double fieldOfView, int width, int height)
        {
            if (Position.Dim != 3 || Target.Dim != 3)
                throw new FormatException("Dimension of Position and Target vectors must be 3");
            Counter++;
            Title = "Camera " + Counter;
            Vector Direction = Position - Target;
            Direction.Normalize();
            Vector UpWorld = new Vector(0, 1, 0);
            Vector Right = Vector.CrossProduct(UpWorld, Direction);
            Right.Normalize();
            Vector Up = Vector.CrossProduct(Direction, Right);
            Matrix tmp1 = new Matrix(4, 4, Right.X, Right.Y, Right.Z, 0,
                Up.X, Up.Y, Up.Z, 0,
                Direction.X, Direction.Y, Direction.Z, 0,
                0, 0, 0, 1);
            Matrix tmp2 = new Matrix(4, 4, 1, 0, 0, -Position.X,
                0, 1, 0, -Position.Y,
                0, 0, 1, -Position.Z,
                0, 0, 0, 1);
            ViewMatrix = tmp1 * tmp2;

            double scale = 1 / Math.Tan(fieldOfView / 2);
            double aspect = (double)width / height;
            ProjectionMatrix = new Matrix(4, 4);
            ProjectionMatrix[0, 0] = scale / aspect;
            ProjectionMatrix[1, 1] = scale;
            ProjectionMatrix[2, 2] = -(farClippingPlane + nearClippingPlane) / (farClippingPlane - nearClippingPlane);
            ProjectionMatrix[2, 3] = -2 * farClippingPlane * nearClippingPlane / (farClippingPlane - nearClippingPlane);
            ProjectionMatrix[3, 2] = -1;
        }

        public Camera() { }
    }
}
