using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _3D_Computer_Graphics.Geometry
{
    public abstract class Geometry : ObjectListElement
    {
        protected List<Triangle> TrianglesGrid { get; set; }
        protected Color ObjectColor { get; set; }
    }
}
