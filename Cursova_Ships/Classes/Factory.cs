using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Cursova_Ships.Classes
{
    //Клас що створює класи викликаючи їхні конструктори та поміщає їх на сцену
    public static class Factory
    {
        public static Sea CreateSea(string source, HelixViewport3D viewport)
        {
            var seaSurface = new FileModelVisual3D()
            {
                Source = source
            };
            viewport.Children.Add(seaSurface);
            return new Sea(seaSurface);
        }

        public static Ship CreateShip(Point3D currentPosition, double speed, string name, HelixViewport3D viewport, Wind wind, string type)
        {
            var shipGeometry = new FileModelVisual3D();
            Preparings.PrepareShip(shipGeometry, currentPosition);
            viewport.Children.Add(shipGeometry);
            if (type == "Motor ship")
            {
                shipGeometry.Source = @"D:\Cursova\Cursova_Ships\Cursova_Ships\Objects\SimpleBoat.obj";
                return new MotorShip(shipGeometry, currentPosition, speed, name);
            }
            else if (type == "Sailing ship")
            {
                shipGeometry.Source = @"D:\Cursova\Cursova_Ships\Cursova_Ships\Objects\SimpleSailingBoat.obj";
                return new SailingShip(shipGeometry, speed, currentPosition, wind, name);
            }
            else 
            {
                return null;
            }
        }

        public static Voyage CreateVoyage(Ship ship, Point3D end)
        {
            Preparings.RotateShip(ship, end);
            return new Voyage(ship, end);
        }
    }
}
