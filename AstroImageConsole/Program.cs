using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using log4net.Config;
using nom.tam.fits;
using nom.tam.util;

using AstroImageTools;

namespace AstroImageConsole
{
    public class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        private const int _pixelsX = 1920;
        private const int _pixelsY = 1280;
        private const double _arcSecPerPixel = 5;
        //private static readonly Coordinates _coords = new Coordinates("03:47:00", "24:07:00");
        private static readonly Coordinates _coords = new Coordinates(0, 90);
        private const double _rotation = 0;
        private const double _backgroundMagnitude = 16;
        private const double _saturationMagnitude = 6;
        private static readonly float _backgroundLevel = (float)Math.Pow(100, (_saturationMagnitude - _backgroundMagnitude) / 5);
        private const double _fwhm = 5;

        private static void DrawStar(CCDImage<float> image, int x, int y, double magnitude, IPSF psf)
        {
            double peak = Math.Pow(100, (_saturationMagnitude - magnitude) / 5);
            int aperture = psf.FindRadiusByLevel(peak, _backgroundLevel * 0.1) * 2;
            //_log.DebugFormat("DrawStar: {0} => {1}, {2}", magnitude, peak, aperture);

            int dxMin = (int) Math.Max(-aperture, -x);
            int dxMax = (int) Math.Min(aperture, image.Width - x - 1);
            int dyMin = (int) Math.Max(-aperture, -y);
            int dyMax = (int) Math.Min(aperture, image.Height - y - 1);
            float[,] pixels = image.GetPixels(x + dxMin, y + dyMin, x + dxMax, y + dyMax);
            for (int dx = dxMin; dx <= dxMax; ++dx)
            {
                for (int dy = dyMin; dy <= dyMax; ++dy)
                {
                    double value = psf.Evaluate(peak, dx, dy);
                    pixels[dx - dxMin, dy - dyMin] = (float)Math.Min(1, pixels[dx - dxMin, dy - dyMin] + value);
                }
            }
            image.SetPixels(x + dxMin, y + dyMin, pixels);
        }

        static void Main(string[] args)
        {

            // Load Log4Net configuration
            XmlConfigurator.Configure();
            _log.Info("AstroToolsConsole started");

            Console.WriteLine("AstroImageConsole V1.0");
            Console.WriteLine();

            // Create WCS data
            WCSData wcs = new WCSData(_coords, _rotation, WCSProjectionType.TAN, new Point(_pixelsX / 2, _pixelsY / 2), _arcSecPerPixel);

            // Get stars from Vizier catalog
            //ICatalog catalog = new PPMXLVizierCatalog(VizierServer.Mirrors[0]);
            ICatalog catalog = new GuideStarCatalog(ConfigurationManager.AppSettings["GSCBaseDirectory"]);
            double fov = (Math.Sqrt((_pixelsX * _pixelsX) + (_pixelsY * _pixelsY)) * _arcSecPerPixel / 3600) / 2;
            IList<CatalogRecord> stars = catalog.LoadAsync(_coords, fov).Result;
            _log.DebugFormat("{0} stars loaded from {1}", stars.Count, catalog.Name);

            // Create CCD image
            CCDImage<float> ccdImage = new CCDImage<float>(wcs, _pixelsX, _pixelsY);
            ccdImage.SetBackground(_backgroundLevel);

            // Draw starts
            IPSF psf = new MoffatPSF(1, 2);
            //IPSF psf = new GaussianPSF(1);
            psf.SetFWHM(_fwhm / _arcSecPerPixel);
            IProjection projection = wcs.Projection;
            int starsDrawn = 0;
            foreach ( CatalogRecord star in stars )
            {
                Point p = projection.Project(star.Coords);
                //_log.DebugFormat("Projection: ({0}, {1}) => ({2}, {3})", star.Coords.RADegrees, star.Coords.Dec, p.X, p.Y);
                if ((p.X >= 0) && (p.X < ccdImage.Width) && (p.Y >= 0) && (p.Y < ccdImage.Height))
                {
                    DrawStar(ccdImage, p.X, p.Y, star.Magnitude, psf);
                    ++starsDrawn;
                }
            }
            _log.DebugFormat("{0} stars drawn", starsDrawn);

            // Write image to FITS file
            Fits fits = ccdImage.ToFits();
            BufferedDataStream stream = new BufferedDataStream(new FileStream(@"E:\Temp\AstroImageConsole.fits", FileMode.Create));
            fits.Write(stream);
            stream.Close();

            _log.Info("AstroToolsConsole complete");
        }
    }
}
