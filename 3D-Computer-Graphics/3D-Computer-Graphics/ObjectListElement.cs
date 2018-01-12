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
        public static int Counter { get; set; } = -1;
        public ObjectListElement() { }
    }
}
