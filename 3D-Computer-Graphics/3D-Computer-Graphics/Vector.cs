using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Computer_Graphics
{
    public class Vector
    {
        public Matrix Values { get; set; }
        public int Dim { get; set; }

        public Vector()
        {
            Values = new Matrix(1, 3, new double[] { 0, 0, 0 });
            Dim = 3;
        }

        public Vector(int n)
        {
            Values = new Matrix(1, n, new double[n]);
            Dim = n;
        }

        public Vector(Matrix m)
        {
            Dim = m.Cols;
            Values = new Matrix(1, Dim);
            for (int i = 0; i < Dim; i++)
                Values[0, i] = m[0, i];
        }

        public Vector(params double[] values)
        {
            Values = new Matrix(1, values.Length, values);
            Dim = values.Length;
        }

        public static Vector CrossProduct(Vector a, Vector b)
        {
            if (a.Dim < 3 || b.Dim < 3) throw new MException("Vectors must be at least 3-dimensional");
            return new Vector(new double[] { a.Y*b.Z-a.Z*b.Y,
            a.Z*b.X-a.X*b.Z, a.X*b.Y-a.Y*b.X});
        }

        public static double DotProduct(Vector a, Vector b)
        {
            if (a.Dim != b.Dim)
                throw new FormatException("Dimensions of vectors must be the same");
            double sum = 0;
            for (int i = 0; i < a.Dim; i++)
                sum += a.Values[0, i] * b.Values[0, i];
            return sum;
        }

        public void Normalize()
        {
            double length = Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2) + Math.Pow(this.Z, 2));
            if (length == 0)
                return;
            X /= length;
            Y /= length;
            Z /= length;
        }

        public Vector DeepClone()
        {
            return new Vector(this.Values);
        }

        public double this[int i]
        {
            get { return Values[0,i]; }
            set { Values[0,i] = value; }
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            if (v1.Dim != v2.Dim) throw new MException("Dimensions must be the same");
            double[] tmp = new double[v1.Dim];
            for (int i = 0; i < tmp.Length; i++)
                tmp[i] = v1[i] - v2[i];
            return new Vector(tmp);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            if (v1.Dim != v2.Dim) throw new MException("Dimensions must be the same");
            double[] tmp = new double[v1.Dim];
            for (int i = 0; i < tmp.Length; i++)
                tmp[i] = v1[i] + v2[i];
            return new Vector(tmp);
        }
        public static Vector operator *(Vector v1, double d)
        {
            double[] tmp = new double[v1.Dim];
            for (int i = 0; i < tmp.Length; i++)
                tmp[i] = v1[i] * d;
            return new Vector(tmp);
        }

        public double X
        {
            get { return Values[0, 0]; }

            set { Values[0, 0] = value; }
        }

        public double Y
        {
            get { return Values[0, 1]; }

            set { Values[0, 1] = value; }
        }

        public double Z
        {
            get { return Values[0, 2]; }

            set { Values[0, 2] = value; }
        }


    }
}
