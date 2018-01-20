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
        private Camera selectedCamera;
        private Light MainLight;
        ObjectListElement SelectedElement;

        private List<ObjectListElement> objects;
        private List<Light> lights;

        private static WriteableBitmap wb = new WriteableBitmap(400, 400, 96, 96, PixelFormats.Bgra32, null);
        private static Int32Rect rect = new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight);
        private static int bytesPerPixel = (wb.Format.BitsPerPixel + 7) / 8;
        private static int stride = wb.PixelWidth * bytesPerPixel;
        private static int arraySize = stride * wb.PixelHeight;
        private static byte[] colorArray = new byte[arraySize];

        public MainWindow()
        {
            InitializeComponent();
            //Screen.Measure(new Size(Width, Height));
            //Screen.Arrange(new Rect(0, 0, Screen.DesiredSize.Width, Screen.DesiredSize.Height));
            Cuboid c = new Cuboid();
            Shapes = new IGeometry[] { c };
            Camera cam = new Camera(new Vector(50,0,-200,1), new Vector(-50,0,200,0), 0.1, 1000, Math.PI/2, 400, 400);
            selectedCamera = cam;
            MainLight = new Light(new Vector(300, 0, 0, 1), Colors.White);
            objects = new List<ObjectListElement>();
            objects.Add(cam);
            objects.Add(MainLight);
            objects.Add(c);
            objectList.ItemsSource = objects;

            lights = new List<Light>();
            lights.Add(MainLight);


            //Random value = new Random();
            //value.NextBytes(colorArray);
            foreach (IGeometry s in Shapes)
                s.Draw(ref colorArray, selectedCamera, lights, 400, 400, stride, bytesPerPixel);

            wb.WritePixels(rect, colorArray, stride, 0);
            Screen.Source = wb;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void NumericUpDown_OnValueChanged(Object sender, Decimal newValue)
        {
            // Camera = new Camera(new Vector((double)positionX.Value, (double)positionY.Value, (double)positionZ.Value), new Vector(0, 0, 0), 0.1, 1000, Math.PI / 2, 400, 400);
            CustomControls.NumericUpDown num = sender as CustomControls.NumericUpDown;
            switch (num.Name)
            {
                case "positionX":
                    SelectedElement.Position.X = (double)newValue;
                    break;
                case "positionY":
                    SelectedElement.Position.Y = (double)newValue;
                    break;
                case "positionZ":
                    SelectedElement.Position.Z = (double)newValue;
                    break;
            }
            SelectedElement.Actualize();
            colorArray = new byte[arraySize];
            foreach (IGeometry s in Shapes)
                s.Draw(ref colorArray, selectedCamera, lights, 400, 400, stride, bytesPerPixel);

            wb.WritePixels(rect, colorArray, stride, 0);
            Screen.Source = wb;
        }

        private void Tbx1_TextChanged(object sender, TextChangedEventArgs e)
       {
            TextBox tb = sender as TextBox;
            switch(tb.Name)
            {
                case "positionX":
                    if (double.TryParse(tb.Text, out double res))
                        SelectedElement.Position.X = res;
                    break;
                case "positionY":
                    if (double.TryParse(tb.Text, out res))
                        SelectedElement.Position.Y = res;
                    break;
                case "positionZ":
                    if (double.TryParse(tb.Text, out res))
                        SelectedElement.Position.Z = res;
                    break;
                case "rotationX":
                    if (double.TryParse(tb.Text, out res))
                        SelectedElement.Rotation.X = res;
                    break;
                case "rotationY":
                    if (double.TryParse(tb.Text, out res))
                        SelectedElement.Rotation.Y = res;
                    break;
                case "rotationZ":
                    if (double.TryParse(tb.Text, out res))
                        SelectedElement.Rotation.Z = res;
                    break;
            }
            SelectedElement.Actualize();
            colorArray = new byte[arraySize];
            foreach (IGeometry s in Shapes)
                s.Draw(ref colorArray, selectedCamera, lights, 400, 400, stride, bytesPerPixel);

            wb.WritePixels(rect, colorArray, stride, 0);
            Screen.Source = wb;
        }

        private void objectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panel.Children.Clear();
            SelectedElement = objects.Find(o => o.Title.Equals(((ObjectListElement)objectList.SelectedItem).Title));
            string objectType = SelectedElement.Title.Split(' ')[0];
            switch (objectType)
            {
                case "Camera":
                    CreateCameraPanel();
                    break;
                case "Light":
                    CreateLightPanel();
                    break;
                case "Cuboid":
                    CreateCuboidPanel();
                    break;

            }
        }

        private void Camera_Click(object sender, RoutedEventArgs e)
        {
            Camera c = new Camera(selectedCamera);
            objects.Add(c); 
        }

        private void Light_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cuboid_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cone_Click(object sender, RoutedEventArgs e)
        {

        }

        private Grid CreateBaseGrid()
        { 
            TextBlock tb2 = new TextBlock();
            tb2.Text = "Position";
            Grid.SetRow(tb2, 0);
            Grid.SetColumn(tb2, 0);
            TextBlock tb3 = new TextBlock();
            tb3.Text = "X";
            Grid.SetRow(tb3, 0);
            Grid.SetColumn(tb3, 1);
            TextBlock tb4 = new TextBlock();
            tb4.Text = "Y";
            Grid.SetRow(tb4, 0);
            Grid.SetColumn(tb4, 3);
            TextBlock tb5 = new TextBlock();
            tb5.Text = "Z";
            Grid.SetRow(tb5, 0);
            Grid.SetColumn(tb5, 4);
            TextBlock tb6 = new TextBlock();
            tb6.Text = "Rotation";
            Grid.SetRow(tb6, 1);
            Grid.SetColumn(tb6, 0);
            TextBlock tb7 = new TextBlock();
            tb7.Text = "X";
            Grid.SetRow(tb7, 1);
            Grid.SetColumn(tb7, 1);
            TextBlock tb8 = new TextBlock();
            tb8.Text = "Y";
            Grid.SetRow(tb8, 1);
            Grid.SetColumn(tb8, 3);
            TextBlock tb9 = new TextBlock();
            tb9.Text = "Z";
            Grid.SetRow(tb9, 1);
            Grid.SetColumn(tb9, 5);

            TextBox tbx1 = new TextBox();
            tbx1.Name = "positionX";
            tbx1.Text = "" + SelectedElement.Position.X;
            Grid.SetRow(tbx1, 0);
            Grid.SetColumn(tbx1, 2);
            tbx1.TextChanged += Tbx1_TextChanged;

            TextBox tbx2 = new TextBox();
            tbx2.Name = "positionY";
            tbx2.Text = "" + SelectedElement.Position.Y;
            Grid.SetRow(tbx2, 0);
            Grid.SetColumn(tbx2, 4);
            tbx2.TextChanged += Tbx1_TextChanged;

            TextBox tbx3 = new TextBox();
            tbx3.Name = "positionZ";
            tbx3.Text = "" + SelectedElement.Position.Z;
            Grid.SetRow(tbx3, 0);
            Grid.SetColumn(tbx3, 6);
            tbx3.TextChanged += Tbx1_TextChanged;

            //------------------------------------------------

            TextBox tbx4 = new TextBox();
            tbx4.Name = "rotationX";
            tbx4.Text = "" + SelectedElement.Rotation.X;
            Grid.SetRow(tbx4, 1);
            Grid.SetColumn(tbx4, 2);
            tbx4.TextChanged += Tbx1_TextChanged;

            TextBox tbx5 = new TextBox();
            tbx5.Name = "rotationY";
            tbx5.Text = "" + SelectedElement.Rotation.Y;
            Grid.SetRow(tbx5, 1);
            Grid.SetColumn(tbx5, 4);
            tbx5.TextChanged += Tbx1_TextChanged;

            TextBox tbx6 = new TextBox();
            tbx6.Name = "rotationZ";
            tbx6.Text = "" + SelectedElement.Rotation.Z;
            Grid.SetRow(tbx6, 1);
            Grid.SetColumn(tbx6, 6);
            tbx6.TextChanged += Tbx1_TextChanged;

            Grid g = new Grid();
            g.ShowGridLines = true;
            RowDefinition gridRow1 = new RowDefinition();
            RowDefinition gridRow2 = new RowDefinition();
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            ColumnDefinition gridCol3 = new ColumnDefinition();
            ColumnDefinition gridCol4 = new ColumnDefinition();
            ColumnDefinition gridCol5 = new ColumnDefinition();
            ColumnDefinition gridCol6 = new ColumnDefinition();
            ColumnDefinition gridCol7 = new ColumnDefinition();
            g.RowDefinitions.Add(gridRow1);
            g.RowDefinitions.Add(gridRow2);
            g.ColumnDefinitions.Add(gridCol1);
            g.ColumnDefinitions.Add(gridCol2);
            g.ColumnDefinitions.Add(gridCol3);
            g.ColumnDefinitions.Add(gridCol4);
            g.ColumnDefinitions.Add(gridCol5);
            g.ColumnDefinitions.Add(gridCol6);
            g.ColumnDefinitions.Add(gridCol7);
            g.Children.Add(tb2);
            g.Children.Add(tb3);
            g.Children.Add(tb4);
            g.Children.Add(tb5);
            g.Children.Add(tb6);
            g.Children.Add(tb7);
            g.Children.Add(tb8);
            g.Children.Add(tb9);
            g.Children.Add(tbx1);
            g.Children.Add(tbx2);
            g.Children.Add(tbx3);
            g.Children.Add(tbx4);
            g.Children.Add(tbx5);
            g.Children.Add(tbx6);
            return g;
        }

        private void CreateCameraPanel()
        {
            TextBlock tb1 = new TextBlock();
            tb1.Text = "Camera";
            tb1.Background = Brushes.LightBlue;
            Grid g = CreateBaseGrid();
            panel.Children.Add(tb1);
            panel.Children.Add(g);
        }

        private void CreateLightPanel()
        {
            TextBlock tb1 = new TextBlock();
            tb1.Text = "Light";
            tb1.Background = Brushes.LightBlue;
            Grid g = CreateBaseGrid();
            panel.Children.Add(tb1);
            panel.Children.Add(g);
        }

        private void CreateCuboidPanel()
        {
            TextBlock tb1 = new TextBlock();
            tb1.Text = "Cuboid";
            tb1.Background = Brushes.LightBlue;
            Grid g = CreateBaseGrid();
            RowDefinition gridRow = new RowDefinition();
            g.RowDefinitions.Add(gridRow);

            TextBlock tb6 = new TextBlock();
            tb6.Text = "Scale";
            Grid.SetRow(tb6, 2);
            Grid.SetColumn(tb6, 0);
            TextBlock tb7 = new TextBlock();
            tb7.Text = "X";
            Grid.SetRow(tb7, 2);
            Grid.SetColumn(tb7, 1);
            TextBlock tb8 = new TextBlock();
            tb8.Text = "Y";
            Grid.SetRow(tb8, 2);
            Grid.SetColumn(tb8, 3);
            TextBlock tb9 = new TextBlock();
            tb9.Text = "Z";
            Grid.SetRow(tb9, 2);
            Grid.SetColumn(tb9, 5);

            TextBox tbx4 = new TextBox();
            tbx4.Name = "rotationX";
            tbx4.Text = "" + SelectedElement.Rotation.X;
            Grid.SetRow(tbx4, 1);
            Grid.SetColumn(tbx4, 2);
            tbx4.TextChanged += Tbx1_TextChanged;

            TextBox tbx5 = new TextBox();
            tbx5.Name = "rotationY";
            tbx5.Text = "" + SelectedElement.Rotation.Y;
            Grid.SetRow(tbx5, 1);
            Grid.SetColumn(tbx5, 4);
            tbx5.TextChanged += Tbx1_TextChanged;

            TextBox tbx6 = new TextBox();
            tbx6.Name = "rotationZ";
            tbx6.Text = "" + SelectedElement.Rotation.Z;
            Grid.SetRow(tbx6, 1);
            Grid.SetColumn(tbx6, 6);
            tbx6.TextChanged += Tbx1_TextChanged;

            g.Children.Add(tb6);
            g.Children.Add(tb7);
            g.Children.Add(tb8);
            g.Children.Add(tb9);
            g.Children.Add(tbx4);
            g.Children.Add(tbx5);
            g.Children.Add(tbx6);
            panel.Children.Add(tb1);
            panel.Children.Add(g);
        }

        
    }
}
