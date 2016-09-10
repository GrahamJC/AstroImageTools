using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroImageTools
{
    public interface ICatalog
    {
        string Name { get; }
        string Description { get; }
        Task<IList<CatalogRecord>> LoadAsync(Coordinates coords, double fov);
    }
}
