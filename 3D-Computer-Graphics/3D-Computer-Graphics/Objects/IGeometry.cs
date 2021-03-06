﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Computer_Graphics.Geometry
{
    public interface IGeometry
    {
        void Draw(ref byte[] colorArray, Camera c, List<Light> l, int width, int height, int stride, int bytesPerPixel);
    }
}
