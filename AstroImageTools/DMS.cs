using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AstroImageTools
{
    public class DMS
    {
        public int Degrees
        {
            get;
            private set;
        }

        public int Minutes
        {
            get;
            private set;
        }

        public double Seconds
        {
            get;
            private set;
        }

        public DMS(string dmsString)
        {
            Match match = Regex.Match(dmsString, @"'?([+-]?[0-9]*)[ :]([0-9]*)[ :]([0-9]*(\.[0-9]*)?)'?");
            if ((match != null) && (match.Groups.Count >= 4))
            {
                Degrees = Int32.Parse(match.Groups[1].Value);
                if ((Degrees < -360) || (Degrees > 360))
                    throw new ArgumentException("Invalid degrees", "dmsString");
                Minutes = Int32.Parse(match.Groups[2].Value);
                if (Minutes >= 60)
                    throw new ArgumentException("Invalid minutes", "dmsString");
                Seconds = Double.Parse(match.Groups[3].Value);
                if (Seconds >= 60)
                    throw new ArgumentException("Invalid seconds", "dmsString");
            }
            else
            {
                throw new ArgumentException("Invalid value", "dms");
            }
        }

        public DMS(double dmsValue)
        {
            if ((dmsValue <= -360) || (dmsValue >= 360))
            {
                throw new ArgumentException("Invalid value", "dmsValue");
            }
            Degrees = (int) Math.Floor(dmsValue);
            dmsValue = (dmsValue - Degrees) * 60;
            Minutes = (int) Math.Floor(dmsValue);
            Seconds = (dmsValue - Minutes) * 60;
            if (Seconds >= 60)
            {
                Seconds -= 60;
                if (++Minutes == 60)
                {
                    Minutes = 0;
                    ++Degrees;
                }
            }
        }

        public double Value
        {
            get
            {
                return (double) Degrees + ((double) Minutes / 60) + (Seconds / 3600);
            }
        }

        public HMS ToHMS()
        {
            return new HMS(Value / 15);
        }

        public override string ToString()
        {
            return String.Format("{0:00}:{1:00}:{2:00.###}", Degrees, Minutes, Seconds);
        }
    }
}
