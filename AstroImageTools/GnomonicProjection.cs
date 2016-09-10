using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroImageTools
{
    public class GnomonicProjection : ProjectionBase, IProjection
    {
        // Cached values from WCS data (radians)
        private double _ra0;
        private double _dec0;
        private double _sinDec0;
        private double _cosDec0;
        private double _rotation;
        private double _scale;

        public GnomonicProjection(WCSData wcs)
            : base(wcs)
        {
            _ra0 = WCS.Coordinates.RADegrees.Value * Math.PI / 180;
            _dec0 = WCS.Coordinates.Dec.Value * Math.PI / 180;
            _sinDec0 = Math.Sin(_dec0);
            _cosDec0 = Math.Cos(_dec0);
            _rotation = WCS.Rotation * Math.PI / 180;
            _scale = (180 / Math.PI) * 3600 / WCS.ArcSecPerPixel;
        }

        public Point Project(Coordinates coords)
        {
            // Get point RA/Dec (radians)
            double ra = coords.RADegrees.Value * Math.PI / 180;
            double dec = coords.Dec.Value * Math.PI / 180;

            // Pre-calculate trig functions
            double sinDec = Math.Sin(dec);
            double cosDec = Math.Cos(dec);
            double sinRADelta = Math.Sin(ra - _ra0);
            double cosRADelta = Math.Cos(ra - _ra0);

            // Calculate point X/Y (standard co-ordinates)
            double denom = (_cosDec0 * cosDec * cosRADelta) + (_sinDec0 * sinDec);
            double stdX = (cosDec * sinRADelta) / denom;
            double stdY = ((_sinDec0 * cosDec * cosRADelta) - (_cosDec0 * sinDec)) / denom;

            // Convert to pixels and rotate (if nexessary)
            int pixX = (int) Math.Round(stdX * _scale);
            int pixY = (int) Math.Round(stdY * _scale);
            if ( _rotation != 0 )
            {
                double rotX = (pixX * Math.Cos(_rotation)) + (pixY * Math.Sin(_rotation));
                double rotY = (-pixX * Math.Sin(_rotation)) + (pixY * Math.Cos(_rotation));
                pixX = (int) Math.Round(rotX);
                pixY = (int) Math.Round(rotY);
            }

            // Return point
            return new Point(WCS.Origin.X - pixX, WCS.Origin.Y + pixY);
        }

        public Coordinates Unproject(Point point)
        {
            // Get pixels and de-rotate (if necessary)
            int pixX = point.X;
            int pixY = point.Y;
            if (_rotation != 0)
            {
                double rotX = (pixX * Math.Cos(-_rotation)) + (pixY * Math.Sin(-_rotation));
                double rotY = (-pixX * Math.Sin(-_rotation)) + (pixY * Math.Cos(-_rotation));
                pixX = (int) Math.Round(rotX);
                pixY = (int) Math.Round(rotY);
            }

            // Convert to standard co-ordinates
            double stdX = pixX / _scale;
            double stdY = pixY / _scale;

            // Calculate point RA/Dec (radians)
            double ra = _ra0 + Math.Atan2( stdX, _cosDec0 - (stdY * _sinDec0 ) );
            double dec = Math.Asin((_sinDec0 + (stdY * _cosDec0)) / Math.Sqrt(1 + (stdX * stdX) + (stdY * stdY)));

            // Return coordinates
            return new Coordinates((ra * 180 / Math.PI) / 15, dec * 180 /Math.PI);
        }
    }
}
