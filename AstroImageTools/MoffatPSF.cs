using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroImageTools
{
    public class MoffatPSF : IPSF
    {
        public double Sigma
        {
            get;
            private set;
        }

        public double Beta
        {
            get;
            private set;
        }

        public MoffatPSF( double sigma, double beta )
        {
            Sigma = sigma;
            Beta = beta;
        }

        public void SetFWHM(double fwhm)
        {
            Sigma = fwhm / (2 * Math.Sqrt(Math.Pow(2, 1 / Beta) - 1));
        }

        public int FindRadiusByLevel(double peak, double level)
        {
            return (int) Math.Round(Sigma * Math.Sqrt(Math.Pow(level / peak, 1 / -Beta) - 1));
        }

        public double Evaluate(double peak, int dx, int dy)
        {
            return peak * Math.Pow(1 + ( ( ( dx * dx ) + ( dy * dy ) ) / ( Sigma *Sigma ) ), -Beta );
        }
    }
}
