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
        private Camera Camera;
        private Light MainLight;
        ObjectListElement SelectedElement;

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
            //Screen.Measure(new Size(Width, Height));
            //Screen.Arrange(new Rect(0, 0, Screen.DesiredSize.Width, Screen.DesiredSize.Height));
            Cuboid c = new Cuboid();
            Shapes = new IGeometry[] { c };
            Camera = new Camera(new Vector(0,0,-200), new Vector(0,0,0), 0.1, 1000, Math.PI/2, 400, 400);
            MainLight = new Light(new Vector(300, 0, 0), Colors.White);
            objects = new List<ObjectListElement>();
            objects.Add(Camera);
            objects.Add(MainLight);
            objects.Add(c);
            objectList.ItemsSource = objects;


            //Random value = new Random();
            //value.NextBytes(colorArray);
            foreach (IGeometry s in Shapes)
                s.Draw(ref colorArray, Camera, 400, 400, stride, bytesPerPixel);

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
                s.Draw(ref colorArray, Camera, 400, 400, stride, bytesPerPixel);

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
            }
            SelectedElement.Actualize();
            colorArray = new byte[arraySize];
            foreach (IGeometry s in Shapes)
                s.Draw(ref colorArray, Camera, 400, 400, stride, bytesPerPixel);

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
                    TextBlock tb1 = new TextBlock();
                    tb1.Text = "Camera";
                    tb1.Background = Brushes.LightBlue;
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

                    //CustomControls.NumericUpDown num1 = new CustomControls.NumericUpDown();
                    //num1.Name = "positionX";
                    //num1.Value = (decimal)SelectedElement.Position.X;
                    //num1.OnValChanged = NumericUpDown_OnValueChanged;
                    //num1.MinValue = -1000;
                    //num1.MaxValue = 1000;
                    //Grid.SetRow(num1, 0);
                    //Grid.SetColumn(num1, 2);

                    //CustomControls.NumericUpDown num2 = new CustomControls.NumericUpDown();
                    //num2.Name = "positionY";
                    //num2.Value = (decimal)SelectedElement.Position.Y;
                    //num2.OnValChanged = NumericUpDown_OnValueChanged;
                    //num2.MinValue = -1000;
                    //num2.MaxValue = 1000;
                    //Grid.SetRow(num2, 0);
                    //Grid.SetColumn(num2, 4);

                    //CustomControls.NumericUpDown num3 = new CustomControls.NumericUpDown();
                    //num3.Name = "positionZ";
                    //num3.Value = (decimal)SelectedElement.Position.Z;
                    //num3.OnValChanged = NumericUpDown_OnValueChanged;
                    //num3.MinValue = -1000;
                    //num3.MaxValue = 1000;
                    //Grid.SetRow(num3, 0);
                    //Grid.SetColumn(num3, 6);

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
                    panel.Children.Add(tb1);
                    panel.Children.Add(g);
                    break;

            }
        }

        
    }
}
