using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sean.Core.DependencyInjection
{
    public class DIObject
    {
        public Type ImplementationType { get; set; }
        public ServiceLifeStyle LifeStyle { get; set; }
    }
}
