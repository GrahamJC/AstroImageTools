using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroImageTools
{
    public interface IPSF
    {
        void SetFWHM(double fwhm);
        int FindRadiusByLevel(double peak, double level);
        double Evaluate(double peak, int dx, int dy);
    }
}
