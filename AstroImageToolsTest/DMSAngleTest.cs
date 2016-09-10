using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AstroImageTools;

namespace AstroImageToolsTest
{
    [TestClass]
    public class DMSAngleTest
    {
        [TestMethod]
        public void TestFromString()
        {
            DMS dms = new DMS( "12:34:56" );
            Assert.AreEqual( 12 + ( 34.0 / 60 ) + ( 56.0 / 3600 ), dms.Value );
            Assert.AreEqual( "12:34:56", dms.ToString() );
        }

        [TestMethod]
        public void TestFromAngle()
        {
            DMS dms = new DMS( 12.3456 );
            Assert.AreEqual( 12.3456, dms.Value );
        }
    }
}
