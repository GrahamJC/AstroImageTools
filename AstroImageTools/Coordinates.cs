using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroImageTools
{
    public class Coordinates
    {
        public HMS RA
        {
            get;
            private set;
        }

        public DMS RADegrees
        {
            get
            {
                return RA.ToDMS();
            }
        }

        public DMS Dec
        {
            get;
            private set;
        }

        public Coordinates(HMS ra, DMS dec)
        {
            RA = ra;
            Dec = dec;
        }

        public Coordinates( double ra, double dec )
            : this( new HMS(ra), new DMS(dec))
        {
        }

        public Coordinates(string ra, string dec)
            : this(new HMS(ra), new DMS(dec))
        {
        }

        public double AngularSeparation( Coordinates coords )
        {
            // Get RA/Dec in radians
            double ra1 = RADegrees.Value * Math.PI / 180;
            double dec1 = Dec.Value * Math.PI / 180;
            double ra2 = coords.RADegrees.Value * Math.PI / 180;
            double dec2 = coords.Dec.Value * Math.PI / 180;

            // Return angular separation
            double separation = Math.Acos((Math.Sin(dec1) * Math.Sin(dec2)) + (Math.Cos(dec1) * Math.Cos(dec2) * Math.Cos(ra1 - ra2)));
            return separation * 180 / Math.PI;
        }
    }
}
