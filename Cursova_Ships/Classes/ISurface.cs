using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Cursova_Ships.Classes
{
    //Інтерфейс для поверхні в 3д сцені
    public interface ISurface
    {
        public Point3D FindSize();

        public void SetTransform(Transform3D transform);
    }
}
