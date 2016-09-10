using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AstroImageTools;

namespace AstroImageToolsTest
{
    [TestClass]
    public class PPMXLVizierCatalogTest
    {
        [TestMethod]
        public void TestQuery()
        {
            VizierServer server = VizierServer.Mirrors[ 0 ];
            ICatalog catalog = new PPMXLVizierCatalog( server );
            IList<CatalogRecord> records = catalog.LoadAsync( new Coordinates( 0, 0 ), 30 ).Result;
        }
    }
}
