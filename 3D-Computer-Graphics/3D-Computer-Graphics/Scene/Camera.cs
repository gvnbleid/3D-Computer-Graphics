using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _3D_Computer_Graphics
{
    [Serializable]
    public class Camera : ObjectListElement
    { 
        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        private static int Counter { get; set; } = -1;
        public Vector Direction { get; set; }
        public double NearClippingPlane { get; set; }
        public double FarClippingPlane { get; set; }
        public double FieldOfView { get; set; }
        public double Aspect { get; set; }

        public Camera(Vector position, Vector rotation, double nearClippingPlane, double farClippingPlane, double fieldOfView, int width, int height)
        {
            if (position.Dim != 4 || rotation.Dim != 4)
                throw new FormatException("Dimension of Position and Target vectors must be 4");
            Position = position.DeepClone();
            Rotation = rotation.DeepClone();

            Matrix m = RotationMatrix();
            Direction = m * new Vector(0, 0, -1, 0);
            Counter++;
            Title = "Camera " + Counter;
            NearClippingPlane = nearClippingPlane;
            FarClippingPlane = farClippingPlane;
            FieldOfView = fieldOfView;
            Aspect = width / height;
            CalculateMatrices();
        }

        public Camera(Camera c)
        {
            Position = c.Position.DeepClone();
            Rotation = c.Rotation.DeepClone();

            Matrix m = RotationMatrix();
            Direction = m * new Vector(0, 0, -1, 0);

            Counter++;
            Title = "Camera " + Counter;
            NearClippingPlane = c.NearClippingPlane;
            FarClippingPlane = c.FarClippingPlane;
            FieldOfView = c.FieldOfView;
            Aspect = c.Aspect;
            CalculateMatrices();
        }

        public void CalculateMatrices()
        {
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
                0, 1, 0, Position.Y,
                0, 0, 1, -Position.Z,
                0, 0, 0, 1);
            ViewMatrix = tmp1 * tmp2;

            double scale = 1 / Math.Tan(FieldOfView * Math.PI / 180.0 / 2);
            ProjectionMatrix = new Matrix(4, 4);
            ProjectionMatrix[0, 0] = scale / Aspect;
            ProjectionMatrix[1, 1] = scale;
            ProjectionMatrix[2, 2] = -(FarClippingPlane + NearClippingPlane) / (FarClippingPlane - NearClippingPlane);
            ProjectionMatrix[2, 3] = -2 * FarClippingPlane * NearClippingPlane / (FarClippingPlane - NearClippingPlane);
            ProjectionMatrix[3, 2] = -1;
        }

        public override void Actualize()
        {
            Matrix m = RotationMatrix();
            Direction = m * new Vector(0, 0, -1, 0);
            CalculateMatrices();
        }

        public Camera() { }

        private Matrix RotationMatrix()
        {
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
            return rotationZMatrix * rotationYMatrix * rotationXMatrix;
        }
    }
}
