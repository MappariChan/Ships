using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Cursova_Ships.Classes
{
    //Абстрактний клас корабель
    public abstract class Ship
    {
        private UIElement3D shipGeometry;
        private Point3D currentPosition;
        private double speed;
        private string name;

        public UIElement3D ShipGeometry {
            get
            {
                return shipGeometry;
            }
            set
            {
                shipGeometry = value;
            }

        }
        public Point3D CurrentPosition
        {
            get
            {
                return currentPosition;
            }
            set
            {
                currentPosition = value;
            }
        }
        public double Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }
        public string Name 
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        private bool waiting;
        private bool stopped;
        private bool isInside;

        public Ship() 
        {
            waiting = false;
            stopped = false;
            isInside = false;
        }

        public Ship(UIElement3D shipGeometry, Point3D currentPosition, double speed, string name)
        {
            waiting = false;
            stopped = false;
            isInside = false;
            ShipGeometry = shipGeometry;
            CurrentPosition = currentPosition;
            Speed = speed;
            Name = name;
        }

        public abstract string ToString();

        public virtual void Move(Point3D end)
        {
            if (IsWaiting() || IsStopped())
            {
                return;
            }
            var vect = new Vector3D(end.X - CurrentPosition.X, end.Y - CurrentPosition.Y, 0);
            if (vect.Length <= Speed)
            {
                stopped = true;
                return;
            }
            double tg;
            double startAngle = 0;
            if (vect.X * vect.Y > 0)
            {
                tg = Math.Abs(vect.Y) / Math.Abs(vect.X);
                if (vect.X > 0) startAngle = 0;
                else startAngle = Math.PI;
            }
            else
            {
                tg = Math.Abs(vect.X) / Math.Abs(vect.Y);
                if (vect.X < 0) startAngle = Math.PI / 2;
                else startAngle = Math.PI * 3 / 2;
            }
            double stepX = Speed * Math.Cos(startAngle + Math.Atan(tg));
            double stepY = Speed * Math.Sin(startAngle + Math.Atan(tg));
            var newCurrentPosition = CurrentPosition;
            newCurrentPosition.X += stepX;
            newCurrentPosition.Y += stepY;
            CurrentPosition = newCurrentPosition;
            var tGroup = (Transform3DGroup)ShipGeometry.Transform;
            var tTranslate = new TranslateTransform3D(new Vector3D(stepX, stepY, 0));
            tGroup.Children.Add(tTranslate);
        }

        private double GetDistance(Ship ship)
        {
            var vector = new Vector(CurrentPosition.X - ship.CurrentPosition.X, CurrentPosition.Y - ship.CurrentPosition.Y);
            return vector.Length;
        }

        public bool IsWaiting()
        {
            return waiting;
        }

        public bool IsStopped()
        {
            return stopped;
        }

        public void Wait()
        {
            waiting = true;
        }

        public void ContinueMooving()
        {
            waiting = false;
        }

        public void Unstope()
        {
            stopped = false;
        }

        public void PredictColisionWith(Ship ship)
        {
            if (GetDistance(ship) < 10)
            {
                if (!IsWaiting() && !ship.IsWaiting() && !IsStopped() && !ship.IsStopped())
                {
                    ship.Wait();
                }
                else if (ship.IsStopped())
                {
                    stopped = true;
                }
            }
        }

        public bool IsReallyStopped(Point3D end)
        {
            var vect = new Vector3D(end.X - CurrentPosition.X, end.Y - CurrentPosition.Y, 0);
            if (vect.Length <= Speed)
            {
                return true;
            }
            return false;
        }

        public bool IsInsideOfPoligon(Poligon poligon, Point3D end)
        {
            if (poligon == null)
            {
                return false;
            }
            Vector direction = new Vector(end.X - CurrentPosition.X, end.Y - CurrentPosition.Y);
            var points = poligon.Points;
            int intersectionCount = 0;
            for (int i = 0; i < points.Count; i++)
            {
                var A = points[i];
                var B = points[(i + 1) % points.Count];
                var CA = new Vector(A.X - CurrentPosition.X, A.Y - CurrentPosition.Y);
                var CB = new Vector(B.X - CurrentPosition.X, B.Y - CurrentPosition.Y);
                double alpha = Vector.AngleBetween(direction, CA);
                double beta = Vector.AngleBetween(direction, CB);
                if ((Math.Abs(alpha) + Math.Abs(beta)) <= 180 && alpha*beta < 0)
                {
                    intersectionCount++;
                }
            }
            if (intersectionCount != 1)
            {
                isInside = false;
            }
            else if (intersectionCount == 1 && !isInside)
            {
                isInside = true;
                return true;
            }
            return false;
        }
    }
}
