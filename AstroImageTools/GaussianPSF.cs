using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroImageTools
{
    public class GaussianPSF : IPSF
    {
        public double Sigma
        {
            get;
            private set;
        }

        public GaussianPSF(double sigma)
        {
            Sigma = sigma;
        }

        public void SetFWHM(double fwhm)
        {
            Sigma = fwhm / (2 * Math.Sqrt(2 * Math.Log(2)));
        }

        public int FindRadiusByLevel(double peak, double level)
        {
            return (int) Math.Round(Sigma * Math.Sqrt(-2 * Math.Log(level / peak)));
        }

        public double Evaluate(double peak, int dx, int dy)
        {
            return peak * Math.Exp( -( ( dx * dx ) + ( dy * dy ) / ( 2 * Sigma *Sigma ) ) );
        }
    }
}
