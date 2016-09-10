using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroImageTools
{
    public class CatalogRecord
    {
        public string Name
        {
            get;
            private set;
        }

        public Coordinates Coords
        {
            get;
            private set;
        }

        public double Magnitude
        {
            get;
            private set;
        }

        public double Diameter
        {
            get;
            private set;
        }

        public CatalogRecord( string name, Coordinates coords, double magnitude, double diameter )
        {
            Name = name;
            Coords = coords;
            Magnitude = magnitude;
            Diameter = diameter;
        }

        public CatalogRecord( string name, Coordinates coords, double magnitude )
            : this( name, coords, magnitude, 0 )
        {
        }
    }
}
