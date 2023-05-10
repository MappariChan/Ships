using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Cursova_Ships.Classes
{
    //Клас поверхні моря
    public class Sea : ISurface
    {
        private UIElement3D seaSurface;

        public UIElement3D SeaSurface
        {
            get
            {
                return seaSurface;
            }
            set
            {
                seaSurface = value;
            }
        }

        public Sea() { }
        public Sea(UIElement3D seaSurface)
        {
            SeaSurface = seaSurface;
        }

        public Point3D FindSize()
        {
            return new Point3D()
            {
                X = SeaSurface.FindBounds(SeaSurface.Transform).SizeX,
                Y = SeaSurface.FindBounds(SeaSurface.Transform).SizeY,
                Z = SeaSurface.FindBounds(SeaSurface.Transform).SizeZ
            };
        }

        public void SetTransform(Transform3D transform)
        {
            SeaSurface.Transform = transform;
        }
    }
}
