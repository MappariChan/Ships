using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Cursova_Ships.Classes
{
    //Парусний корабель, наслідується від звичайного абстрактного корабля, Пливе лише у напрямку вітру, та його швидкість залежить також від вітру
    public class SailingShip : Ship
    {

        private Wind Wind { get; set; }

        public SailingShip() : base() { }
        public SailingShip(UIElement3D shipGeometry, double speed, Point3D currentPosition, Wind wind, string name) : base(shipGeometry, currentPosition, speed, name) 
        {
            Wind = wind;
        }

        override public string ToString()
        {
            return "Sailing ship";
        }

        private double CalculateSpeed(Point3D end)
        {
            Vector shipDirection = new Vector(end.X - CurrentPosition.X, end.Y - CurrentPosition.Y);
            double angle = Vector.AngleBetween(shipDirection, Wind.Vector);
            if (angle > 90 || angle < -90)
            {
                return 0;
            }
            return Wind.Speed * Math.Cos(angle*Math.PI/180);
        }

        public override void Move(Point3D end)
        {
            Speed = CalculateSpeed(end);
            base.Move(end);
        }
    }
}
