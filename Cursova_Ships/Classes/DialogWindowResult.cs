using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursova_Ships.Classes
{
    //Клас для повернення результатів діалогово вікна
    public class DialogWindowResult
    {
        private double speed;
        private string type;
        private string name;
        private bool failure;

        public double Speed
        {
            get {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
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

        public bool Failure
        {
            get 
            {
                return failure;
            }
            set
            {
                failure = value;
            }
        }

        public DialogWindowResult() { }
        public DialogWindowResult(double speed, string type, string name)
        {
            Speed = speed;
            Type = type;
            Name = name;
        }
    }
}
