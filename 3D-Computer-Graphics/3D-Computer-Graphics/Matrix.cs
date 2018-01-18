using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Computer_Graphics
{
    public class Matrix
    {
        public int Rows { get; }
        public int Cols { get; }
        private double[,] mat;

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            mat = new double[rows, cols];
        }

        public Matrix(int rows, int cols, params double[] elements )
        {
            Rows = rows;
            Cols = cols;
            mat = new double[rows, cols];
            for (int i = 0; i < elements.Length; i++)
                mat[i / Cols, i % Cols] = elements[i];
        }

        public double this[int iRow, int iCol]
        {
            get { return mat[iRow, iCol]; }
            set { mat[iRow, iCol] = value; }
        }

        public Matrix GetCol(int k)
        {
            Matrix m = new Matrix(Rows, 1);
            for (int i = 0; i < Rows; i++) m[i, 0] = mat[i, k];
            return m;
        }

        public void SetCol(Matrix v, int k)
        {
            for (int i = 0; i < Rows; i++) mat[i, k] = v[i, 0];
        }

        public Matrix Duplicate()
        {
            Matrix matrix = new Matrix(Rows, Cols);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    matrix[i, j] = mat[i, j];
            return matrix;
        }

        public static Matrix Multiply(Matrix m1, Matrix m2)                  
        {
            if (m1.Cols != m2.Rows) throw new MException("Wrong dimensions of matrix!");

            Matrix result = ZeroMatrix(m1.Rows, m2.Cols);
            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; j < result.Cols; j++)
                    for (int k = 0; k < m1.Cols; k++)
                        result[i, j] += m1[i, k] * m2[k, j];
            return result;
        }
        private static Matrix MultiplyByConstant(double n, Matrix m)                         
        {
            Matrix r = new Matrix(m.Rows, m.Cols);
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    r[i, j] = m[i, j] * n;
            return r;
        }
        private static Matrix Add(Matrix m1, Matrix m2)        
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols) throw new MException("Matrices must have the same dimensions!");
            Matrix r = new Matrix(m1.Rows, m1.Cols);
            for (int i = 0; i < r.Rows; i++)
                for (int j = 0; j < r.Cols; j++)
                    r[i, j] = m1[i, j] + m2[i, j];
            return r;
        }

        public static Matrix ZeroMatrix(int iRows, int iCols) 
        {
            Matrix matrix = new Matrix(iRows, iCols);
            for (int i = 0; i < iRows; i++)
                for (int j = 0; j < iCols; j++)
                    matrix[i, j] = 0;
            return matrix;
        }

        

        //   O P E R A T O R S

        public static Matrix operator -(Matrix m)
        { return Matrix.MultiplyByConstant(-1, m); }

        public static Matrix operator +(Matrix m1, Matrix m2)
        { return Matrix.Add(m1, m2); }

        public static Matrix operator -(Matrix m1, Matrix m2)
        { return Matrix.Add(m1, -m2); }

        public static Matrix operator *(Matrix m1, Matrix m2)
        { return Matrix.Multiply(m1, m2); }

        public static Matrix operator *(double n, Matrix m)
        { return Matrix.MultiplyByConstant(n, m); }

        public static Vector operator *(Matrix m, Vector v)
        {
            if (m.Cols != v.Dim) throw new MException("Dimensions must agree");
            double[] tmp = new double[m.Rows];
            for(int i=0;i<m.Rows;i++)
            {
                double sum = 0;
                for (int j = 0; j < m.Cols; j++)
                    sum += m[i, j] * v[j];
                tmp[i] = sum;
            }
            return new Vector(tmp);
        }
    }

    //  The class for exceptions

    public class MException : Exception
    {
        public MException(string Message)
            : base(Message)
        { }
    }
}
