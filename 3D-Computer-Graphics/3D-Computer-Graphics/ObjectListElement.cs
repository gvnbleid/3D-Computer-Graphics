using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Computer_Graphics
{
    public abstract class ObjectListElement
    {
        public string Title { get; set; }
        public Vector Position { get; set; }
        public Vector Rotation { get; set; }
        public ObjectListElement() { }

        public abstract void Actualize();
    }
}
