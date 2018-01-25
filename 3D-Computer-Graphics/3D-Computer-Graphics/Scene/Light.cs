using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _3D_Computer_Graphics
{
    public class Light : ObjectListElement
    {
        public double LightIntensity { get; set; }
        public double LightDistance { get; set; }
        public static double AmbientFactor { get; set; } = 0.1;
        public double DiffuseFactor { get; set; }
        public double SpecularFactor { get; set; }
        public double SpecularHardness { get; set; }
        public Color LightColor { get; set; }
        private static int Counter { get; set; } = -1;

        public Light()
        {
            Counter++;
            Title = "Light " + Counter;
            Position = new Vector(0, 0, 0, 1);
            Position.Y = -Position.Y;
            LightColor = Colors.White;
            Rotation = new Vector(0, 0, 0, 0);
        }

        public Light(Vector position, Color color)
        {
            Counter++;
            Title = "Light " + Counter;
            Position = position.DeepClone();
            Position.Y = -Position.Y;
            LightColor = color;
            Rotation = new Vector(0, 0, 0, 0);
            DiffuseFactor = 0.5;
            SpecularFactor = 0.5;
        }

        public override void Actualize()
        {
        }
    }
}
