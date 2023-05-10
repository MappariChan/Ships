using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Cursova_Ships.Classes
{
    //Клас заплив, що містить маршрут та корабель який його пропливає
    public class Voyage
    {

        private Ship ship;
        private Point3D start;
        private Point3D end;

        public Ship Ship {
            get {
                return ship;
            }
            set
            {
                ship = value;
            }
        }
        public Point3D Start {
            get {
                return start;
            }
            set
            {
                start = value;
            }
        }
        public Point3D End {

            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }

        public Voyage() { }
        public Voyage(Ship ship, Point3D end)
        {
            Ship = ship;
            Start = Ship.CurrentPosition;
            End = end;
        }

        public void MoveShip()
        {
            Ship.Move(End);
        }

        public bool IsReallyEnd()
        {
            return Ship.IsReallyStopped(End);
        }

        public bool IsShipInsideOfPoligon(Poligon poligon)
        {
            return Ship.IsInsideOfPoligon(poligon, End);
        }
    }
}
