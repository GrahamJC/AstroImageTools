using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroImageTools
{
    public abstract class ProjectionBase
    {
        public WCSData WCS
        {
            get;
            private set;
        }

        protected ProjectionBase( WCSData wcs )
        {
            WCS = wcs;
        }
    }
}
