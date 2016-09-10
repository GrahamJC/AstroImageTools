using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

namespace AstroImageTools
{
    public class PPMXLVizierCatalog : VizierCatalog
    {
        private ILog _log = LogManager.GetLogger( typeof( PPMXLVizierCatalog ) );

        public PPMXLVizierCatalog(VizierServer server)
            : base("PPMXL", "PPMXL catalog (910,469,430 objects)", server)
        {
            Fields = new List<string> { "Name", "Coordinates", "Jmag", "Hmag", "Kmag", "b1mag", "b2mag", "r1mag", "r2mag", "imag" };
            Filters = new List<string> { "Jmag", "Hmag", "Kmag", "b1mag", "b2mag", "r1mag", "r2mag", "imag" };
            MaxFOV = 45;
            MinMagnitude = null;
            MaxMagnitude = 15;
        }

        protected override string UrlQuery( Coordinates coords, double fov)
        {
            // Vizier catalogs use the Astronomical Server URL for querying
            StringBuilder url = new StringBuilder();
            url.Append( "-source=I/317" );
            url.AppendFormat( "&-c.ra={0}", coords.RADegrees );
            url.AppendFormat("&-c.dec={0}", coords.Dec);
            url.AppendFormat( "&-c.rm={0}", fov * 60);
            url.Append( "&-out.form=|" );
            url.Append( "&-out.max=50000" );
            url.Append( "&-out=PPMXL RAJ2000 DEJ2000 pmRA pmDE r1mag" );
            url.Append( UrlFilter( "r1mag", MinMagnitude, MaxMagnitude ) );
            return url.ToString();
        }

        protected override CatalogRecord Parse( string queryResult )
        {
            string[] fields = queryResult.Split( '|' );
            foreach ( string field in fields )
                field.Trim();
            CatalogRecord record = null;
            double raDeg;
            double dec;
            double magnitude = 0;
            if ( ( fields.Length >= 6 )
                && !String.IsNullOrEmpty( fields[ 0 ] )
                && Double.TryParse( fields[ 1 ], out raDeg )
                && Double.TryParse( fields[ 2 ], out dec )
                && ( String.IsNullOrEmpty( fields[ 5 ] ) || Double.TryParse( fields[ 5 ], out magnitude ) ) )
            {
                Coordinates coords = new Coordinates(raDeg / 15, dec);
                record = new CatalogRecord( fields[ 0 ], coords, magnitude );
            }
            else
            {
                _log.WarnFormat( "Error parsing PPMXL record: {0}", String.Join( "|", fields ) );
            }
            return record;
        }
    }
}
