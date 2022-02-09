using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sean.Core.DependencyInjection
{
    public class DIImpl
    {
        public object ImplementationInstance { get; set; }
        public Type ImplementationType { get; set; }
        public ServiceLifeStyle LifeStyle { get; set; }
    }
}
