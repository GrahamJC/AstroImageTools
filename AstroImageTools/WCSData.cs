using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

namespace AstroImageTools
{
    public class WCSData
    {
        private ILog _log = LogManager.GetLogger( typeof( WCSData ) );

        public Coordinates Coordinates
        {
            get;
            private set;
        }

        public double Rotation
        {
            get;
            private set;
        }

        public WCSProjectionType ProjectionType
        {
            get;
            private set;
        }

        public Point Origin
        {
            get;
            private set;
        }

        public double ArcSecPerPixel
        {
            get;
            private set;
        }

        public WCSData(Coordinates coords, double rotation, WCSProjectionType projectionType, Point origin, double arcSecPerPixel)
        {
            Coordinates = coords;
            Rotation = rotation;
            ProjectionType = projectionType;
            Origin = origin;
            ArcSecPerPixel = arcSecPerPixel;
        }

        private IProjection _projection;
        public IProjection Projection
        {
            get
            {
                if (_projection == null)
                {
                    switch (ProjectionType)
                    {
                        case WCSProjectionType.TAN:
                            _projection = new GnomonicProjection( this );
                            break;
                        default:
                            throw new InvalidOperationException(String.Format("{0} projection not supported", ProjectionType));
                    }
                }
                return _projection;
            }
        }
    }
}
