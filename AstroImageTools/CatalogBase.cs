using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroImageTools
{
    public abstract class CatalogBase
    {
        public string Name
        {
            get;
            protected set;
        }

        public string Description
        {
            get;
            protected set;
        }

        protected CatalogBase(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
