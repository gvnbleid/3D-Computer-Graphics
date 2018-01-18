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
using System.Text.RegularExpressions;

namespace _3D_Computer_Graphics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IGeometry[] Shapes;
        private Scene.Camera Camera;
        private List<ObjectListElement> objects;

        private static WriteableBitmap wb = new WriteableBitmap(400, 400, 96, 96, PixelFormats.Bgra32, null);
        private static Int32Rect rect = new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight);
        private static int bytesPerPixel = (wb.Format.BitsPerPixel + 7) / 8;
        private static int stride = wb.PixelWidth * bytesPerPixel;
        private static int arraySize = stride * wb.PixelHeight;
        private static byte[] colorArray = new byte[arraySize];

        public MainWindow()
        {
            InitializeComponent();

            Shapes = new IGeometry[] { new Cuboid() };
            Camera = new Scene.Camera(0,0,-200);
            objects = new List<ObjectListElement>();
            objects.Add(Camera);
            objectList.ItemsSource = objects;


            //Random value = new Random();
            //value.NextBytes(colorArray);
            foreach (IGeometry s in Shapes)
                s.Draw(ref colorArray, Camera.View, 400, 400, stride, bytesPerPixel);

            wb.WritePixels(rect, colorArray, stride, 0);
            Screen.Source = wb;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void NumericUpDown_OnValueChanged(Decimal newValue)
        {
            Camera = new Scene.Camera((double)NumericUpDownX.Value, (double)NumericUpDownY.Value, (double)NumericUpDownZ.Value);
            colorArray = new byte[arraySize];
            foreach (IGeometry s in Shapes)
                s.Draw(ref colorArray, Camera.View, 400, 400, stride, bytesPerPixel);

            wb.WritePixels(rect, colorArray, stride, 0);
            Screen.Source = wb;
        }

        //private void Camera_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    bool ifX = int.TryParse(CameraXAxis.Text, out int x);
        //    bool ifY = int.TryParse(CameraYAxis.Text, out int y);
        //    bool ifZ = int.TryParse(CameraZAxis.Text, out int z);
        //    if (ifX && ifY && ifZ)
        //        Camera = new Scene.Camera(x, y, z);
        //    colorArray = new byte[arraySize];
        //    foreach (IGeometry s in Shapes)
        //        s.Draw(ref colorArray, Camera.View, 400, 400, stride, bytesPerPixel);

        //    wb.WritePixels(rect, colorArray, stride, 0);
        //    Screen.Source = wb;
        //}
    }
}
