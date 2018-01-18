using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Computer_Graphics.Geometry
{
    public abstract class Geometry : ObjectListElement
    {
        protected Vector Position { get; set; }
        protected List<Triangle> TrianglesGrid { get; set; }
    }
}
