using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using log4net.Config;

namespace AstroImageToolsTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Setup
    {
        [AssemblyInitialize]
        public static void Configure( TestContext context )
        {
            XmlConfigurator.Configure();
        }
    }
}
