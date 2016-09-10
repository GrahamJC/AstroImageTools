using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AstroImageTools
{
    public class HMS
    {
        public int Hours
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

        public HMS( string hmsString )
        {
            Match match = Regex.Match( hmsString, @"'?([0-9]*)[ :]([0-9]*)[ :]([0-9]*(\.[0-9]*)?)'?" );
            if ( ( match != null ) && ( match.Groups.Count >= 4 ) )
            {
                Hours = Int32.Parse( match.Groups[ 1 ].Value );
                if ( Hours >= 24 )
                    throw new ArgumentException("Invalid hours", "hmsString");
                Minutes = Int32.Parse( match.Groups[ 2 ].Value );
                if ( Minutes >= 60 )
                    throw new ArgumentException( "Invalid minutes", "hmsString");
                Seconds = Double.Parse( match.Groups[ 3 ].Value );
                if ( Seconds >= 60 )
                    throw new ArgumentException( "Invalid seconds", "hmsString");
            }
            else
            {
                throw new ArgumentException("Invalid value", "hms" );
            }
        }

        public HMS( double hmsValue )
        {
            if ( ( hmsValue < 0 ) || (hmsValue >= 24 ) )
            {
                throw new ArgumentException("Invalid value", "hmsValue");
            }
            Hours = (int)Math.Floor(hmsValue);
            hmsValue = (hmsValue - Hours ) * 60;
            Minutes = (int)Math.Floor(hmsValue);
            Seconds = (hmsValue - Minutes ) * 60;
            if (Seconds >= 60)
            {
                Seconds -= 60;
                if (++Minutes == 60)
                {
                    Minutes = 0;
                    ++Hours;
                }
            }
        }

        public double Value
        {
            get
            {
                return (double)Hours + ( (double)Minutes / 60 ) + ( Seconds / 3600 );
            }
        }

        public override string ToString()
        {
            return String.Format( "{0:00}:{1:00}:{2:00.###}", Hours, Minutes, Seconds );
        }

        public DMS ToDMS()
        {
            return new DMS(Value * 15);
        }
    }
}
