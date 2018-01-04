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

namespace _3D_Computer_Graphics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IGeometry[] Shapes;
        private Scene.Camera Camera;

        private static WriteableBitmap wb = new WriteableBitmap(400, 200, 96, 96, PixelFormats.Bgra32, null);
        private static Int32Rect rect = new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight);
        private static int bytesPerPixel = (wb.Format.BitsPerPixel + 7) / 8;
        private static int stride = wb.PixelWidth * bytesPerPixel;
        private static int arraySize = stride * wb.PixelHeight;
        private static byte[] colorArray = new byte[arraySize];

        public MainWindow()
        {
            Shapes = new IGeometry[] { new Cuboid() };
            Camera = new Scene.Camera();
            InitializeComponent();

            Random value = new Random();
            value.NextBytes(colorArray);
            foreach (IGeometry s in Shapes)
                s.Draw(ref colorArray, Camera.View, 400, 200, stride, bytesPerPixel);
            for (int i = 0; i < colorArray.Length; i++)
            {
                if (i % 4 == 0)
                    colorArray[i] = 255;
                else
                    colorArray[i] = 0;
            }
            wb.WritePixels(rect, colorArray, stride, 0);
            Screen.Source = wb;
        }
    }
}
