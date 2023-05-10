using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cursova_Ships.Classes
{
    //Клас вітру, відповідає за генерацію вектору вітру, та його зміни
    public class Wind
    {
        private Vector vector;
        private double speed;
        public Vector Vector {
            get {
                return vector;
            }
            set
            {
                vector = value;
            }
        }
        public double Speed {
            get {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        public Wind()
        {
            Random random = new Random();
            int randInt = random.Next(-1,1);
            double randDouble = random.NextDouble();
            double x = randInt + randDouble;
            int sign = 0;
            while(sign == 0)    sign = random.Next(-100, 101);
            sign /= Math.Abs(sign);
            double y = Math.Sqrt(1 - Math.Pow(x, 2)) * sign;
            Vector = new Vector(x, y);
            Speed = random.NextDouble();
        }
        public Wind(Vector vector, double speed)
        {
            Vector = vector;
            Speed = speed;
        }

        public double ChangeDirectionAndSpeed()
        {
            Random random = new Random();
            double angle = random.Next(-2, 3);
            double newX = Vector.X * Math.Cos(angle * Math.PI / 180) + Vector.Y * Math.Sin(angle * Math.PI / 180);
            double newY = -Vector.X * Math.Sin(angle * Math.PI / 180) + Vector.Y * Math.Cos(angle * Math.PI / 180);
            Vector = new Vector(newX, newY);
            double speedDeviation = random.Next(-10, 11) / 1000.0;
            Speed += speedDeviation;
            if (Speed > 1)
            {
                Speed = 1;
            }
            else if (Speed < 0)
            {
                Speed = 0;
            }
            return angle;
        }
    }
}
