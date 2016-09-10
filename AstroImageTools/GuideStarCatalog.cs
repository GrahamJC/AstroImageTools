using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using nom.tam.fits;

namespace AstroImageTools
{
    public class GuideStarCatalog : CatalogBase, ICatalog
    {
        private ILog _log = LogManager.GetLogger(typeof(GuideStarCatalog));

        // Class methods
        static public Fits OpenGSCFile(String filename)
        {
            // Read compressed files into memory (the CSharpFITS library does not handle compressed files
            // because they do not support deferred input)
            if (filename.EndsWith(".gz"))
            {
                FileStream fileStream = new FileStream(filename, FileMode.Open);
                GZipStream gzStream = new GZipStream(fileStream, CompressionMode.Decompress);
                MemoryStream memStream = new MemoryStream();
                gzStream.CopyTo(memStream);
                memStream.Seek(0, SeekOrigin.Begin);
                return new nom.tam.fits.Fits(memStream);
            }
            else
                return new nom.tam.fits.Fits(filename);
        }

        // Instance data
        private List<GSCRegion> _regions;

        // Constructors
        public GuideStarCatalog(String baseDirectory)
            : base("GSC1.1", "Hubble Guide Star Catalog V1.1")
        {
            BaseDirectory = baseDirectory;
        }

        // Properties
        public String BaseDirectory { get; private set; }
        public List<GSCRegion> Regions
        {
            get
            {
                if (_regions == null)
                {
                    _regions = new List<GSCRegion>();
                    Fits regionsFits = GuideStarCatalog.OpenGSCFile(String.Format("{0}\\TABLES\\REGIONS.TBL", BaseDirectory));
                    AsciiTableHDU hdu = regionsFits.GetHDU(1) as AsciiTableHDU;
                    AsciiTable table = hdu.GetData() as AsciiTable;
                    for (Int32 i = 0; i < table.NRows; ++i)
                    {
                        _regions.Add(new GSCRegion(BaseDirectory, (Object[]) table.GetRow(i)));
                    }
                }
                return _regions;
            }
        }

        // ICatalog

        public async Task<IList<CatalogRecord>> LoadAsync(Coordinates coords, double fov)
        {
            // Initialize result
            IList<CatalogRecord> records = new List<CatalogRecord>();

            // Process each region
            foreach (GSCRegion region in Regions)
            {
                // Check if any part of the region falls within the FOV
                if ((coords.AngularSeparation(new Coordinates(region.RightAscensionMin, region.DeclinationMin)) <= fov)
                   || (coords.AngularSeparation(new Coordinates(region.RightAscensionMin, region.DeclinationMax)) <= fov)
                   || (coords.AngularSeparation(new Coordinates(region.RightAscensionMax, region.DeclinationMin)) <= fov)
                   || (coords.AngularSeparation(new Coordinates(region.RightAscensionMax, region.DeclinationMax)) <= fov))
                {
                    // Check each star in region
                    foreach(GSCStar star in region.Stars)
                    {
                        if (coords.AngularSeparation(new Coordinates(star.RightAscension, star.Declination)) <= fov)
                        {
                            records.Add(new CatalogRecord("GSC" + star.ID.ToString(), new Coordinates(star.RightAscension, star.Declination), star.Magnitude));
                        }
                    }
                }
            }
            return records;
        }
    }
}
