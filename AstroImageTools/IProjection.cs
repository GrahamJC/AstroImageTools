using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroImageTools
{
    public interface IProjection
    {
        Point Project(Coordinates coords);
        Coordinates Unproject(Point point);
    }
}
