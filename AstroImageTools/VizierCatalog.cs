using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using log4net;

namespace AstroImageTools
{
    public abstract class VizierCatalog : CatalogBase, ICatalog
    {
        private ILog _log = LogManager.GetLogger( typeof( VizierCatalog ) );

        public VizierServer Server
        {
            get;
            private set;
        }

        public IList<string> Fields
        {
            get;
            protected set;

        }
        public IList<string> Filters
        {
            get;
            protected set;
        }

        public double MaxFOV
        {
            get;
            protected set;
        }

        public double? MinMagnitude
        {
            get;
            protected set;
        }

        public double? MaxMagnitude
        {
            get;
            protected set;
        }

        protected VizierCatalog(string name, string description, VizierServer server)
            : base(name, description)
        {
            Server = server;
        }

        protected string UrlFilter( string field, double? min, double? max )
        {
            if ( min.HasValue && max.HasValue )
                return String.Format( "&{0}={1}..{2}", field, min, max );
            else if ( min.HasValue )
                return String.Format( "&{0}=>{1}", field, min );
            else if ( max.HasValue )
                return String.Format( "&{0}=<{1}", field, max );
            else
                return String.Empty;
        }

        protected abstract string UrlQuery( Coordinates coords, double fov );
        protected abstract CatalogRecord Parse( string queryResult );

        public async Task<IList<CatalogRecord>> LoadAsync( Coordinates coords, double fov )
        {
            // Initialize result
            IList<CatalogRecord> records = new List<CatalogRecord>();

            // Build URL to query catalog
            UriBuilder uri = new UriBuilder( Server.Protocol, Server.HostName );
            uri.Path = "viz-bin/asu-tsv";
            uri.Query = UrlQuery( coords, fov);

            // Execute the query
            string queryResult = null;
            try
            {
                HttpClient httpClient = new HttpClient();
                _log.DebugFormat( "Executing query {0}", uri.ToString() );
                queryResult = await httpClient.GetStringAsync( uri.Uri );
            }
            catch ( Exception e )
            {
                _log.Error( "Unexpected exception querying VizieR catalog", e );
            }

            // Parse the results
            foreach ( string queryLine in queryResult.Split( '\n' ) )
            {
                if ( !String.IsNullOrEmpty( queryLine ) && !queryLine.StartsWith( "#" ) )
                {
                    CatalogRecord record = Parse( queryLine );
                    if ( record != null )
                    {
                        records.Add( record );
                    }
                }
            }
            return records;
        }
    }
}
