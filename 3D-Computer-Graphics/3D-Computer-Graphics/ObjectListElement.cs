using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using _3D_Computer_Graphics.Geometry;

namespace _3D_Computer_Graphics
{
    [XmlInclude(typeof(Camera))]
    [XmlInclude(typeof(Light))]
    [XmlInclude(typeof(Cuboid))]
    [XmlInclude(typeof(Cone))]
    public abstract class ObjectListElement
    {
        public string Title { get; set; }
        public Vector Position { get; set; }
        public Vector Rotation { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public ObjectListElement() { }

        public abstract void Actualize();
    }
}
