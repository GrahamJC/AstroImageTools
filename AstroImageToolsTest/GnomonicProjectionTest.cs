using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AstroImageTools;

namespace AstroImageToolsTest
{
    [TestClass]
    public class GnomonicProjectionTest
    {
        [TestMethod]
        public void TestProject()
        {
            WCSData wcs = new WCSData(new Coordinates(0, 0), 0, WCSProjectionType.TAN, new Point(0, 0), 1);
            IProjection projection = wcs.Projection;
            Point point1 = projection.Project(new Coordinates(0, 0));
            Point point2 = projection.Project(new Coordinates(0, 1));
            Coordinates coords2 = projection.Unproject(point2);
        }
    }
}
