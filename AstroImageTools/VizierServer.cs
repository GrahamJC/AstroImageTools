using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroImageTools
{
    public class VizierServer
    {
        public string Name
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public string Protocol
        {
            get;
            private set;
        }

        public string HostName
        {
            get;
            private set;
        }

        public static IList<VizierServer> Mirrors
        {
            get;
            private set;
        }

        static VizierServer()
        {
            Mirrors = new List<VizierServer>
            {
                new VizierServer { Name = "CDS", Description = "(cdsarc.u-strasbg.fr) Strasbourg, France", Protocol = "http", HostName = "cdsarc.u-strasbg.fr" },
                new VizierServer { Name = "ADAC", Description = "(vizier.nao.ac.jp) Tokyo, Japan", Protocol = "http", HostName = "vizier.nao.ac.jp" }
            };
        }
    }
}
