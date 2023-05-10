using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Cursova_Ships.Classes
{
    //Клас моторного корабля, що наслідується від абстрактного класу корабля
    public class MotorShip : Ship
    {
        public MotorShip() : base() { }
        public MotorShip(UIElement3D shipGeometry, Point3D currentPosition, double speed, string name) : base(shipGeometry, currentPosition, speed, name) { }

        override public string ToString()
        {
            return "Motor ship";
        }
    }
}
