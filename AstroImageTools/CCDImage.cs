using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using nom.tam.fits;

namespace AstroImageTools
{
    public class CCDImage<T>
    {
        public WCSData WCS
        {
            get;
            private set;
        }

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        T[][] _pixels;

        public CCDImage( WCSData wcs, int width, int height )
        {
            WCS = wcs;
            Width = width;
            Height = height;
            _pixels = new T[height][];
            for (int i = 0; i < height; ++i)
            {
                _pixels[i] = new T[width];
            }
        }

        public static CCDImage<T> FromFits<T>( Fits fits )
        {
            return new CCDImage<T>(null, 0, 0);
        }

        public T this[ int x, int y ]
        {
            get
            {
                return _pixels[y][x];
            }
            set
            {
                _pixels[y][x] = value;
            }
        }

        public void SetBackground(T level)
        {
            for ( int y = 0; y < Height; ++y )
            {
                T[] row = _pixels[y];
                for (int x = 0; x < Width; ++x)
                {
                    row[x] = level;
                }
            }
        }

        public T[,] GetPixels( int x1, int y1, int x2, int y2 )
        {
            T[,] values = new T[x2 - x1 + 1, y2 - y1 + 1];
            for (int y = y1; y <= y2; ++y)
            {
                T[] row = _pixels[y];
                for (int x = x1; x <= x2; ++x)
                {
                    values[x - x1, y - y1] = row[x];
                }
            }
            return values;
        }

        public void SetPixels(int x0, int y0, T[,] values)
        {
            for (int y = 0; y < values.GetLength(1); ++y)
            {
                T[] row = _pixels[y0 + y];
                for (int x = 0; x < values.GetLength(0); ++x)
                {
                    row[x0 + x] = values[x, y];
                }
            }
        }

        public Fits ToFits()
        {
            // Create basic FITS file
            Fits fits = new Fits();
            BasicHDU hdu = FitsFactory.HDUFactory( _pixels );

            // Add WCS keywords
            if (WCS != null)
            {
                hdu.AddValue("CRVAL1", WCS.Coordinates.RADegrees.Value, String.Empty);
                hdu.AddValue("CRVAL2", WCS.Coordinates.Dec.Value, String.Empty);
                hdu.AddValue("CTYPE1", "RA---" + WCS.ProjectionType.ToString(), String.Empty);
                hdu.AddValue("CTYPE2", "DEC--" + WCS.ProjectionType.ToString(), String.Empty);
                hdu.AddValue("CRPIX1", WCS.Origin.X, String.Empty);
                hdu.AddValue("CRPIX2", WCS.Origin.Y, String.Empty);
                hdu.AddValue("CROTA1", WCS.Rotation, String.Empty);
                hdu.AddValue("CROTA2", WCS.Rotation, String.Empty);
                hdu.AddValue("CDELT1", WCS.ArcSecPerPixel / 3600, String.Empty);
                hdu.AddValue("CDELT2", WCS.ArcSecPerPixel / 3600, String.Empty);
            }

            // Add HDU to FITS file and rerun it
            fits.AddHDU(hdu);
            return fits;
        }
    }
}
