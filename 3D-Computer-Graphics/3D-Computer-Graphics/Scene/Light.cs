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
        public double AmbientFactor { get; set; }
        public double DiffuseFactor { get; set; }
        public double SpecularFactor { get; set; }
        public double SpecularHardness { get; set; }
        public Color LightColor { get; set; }
        private static int Counter { get; set; } = -1;

        public Light(Vector position, Color color)
        {
            Counter++;
            Title = "Light " + Counter;
            Position = position.DeepClone();
            LightColor = color;
        }

        public override void Actualize()
        {
            throw new NotImplementedException();
        }
    }
}
