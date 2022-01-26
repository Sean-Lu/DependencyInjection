using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sean.Core.DependencyInjection
{
    public class UnresolvedTypeException : Exception
    {
        public Type UnresolvedType { get; }

        public UnresolvedTypeException(Type unresolvedType, string message) : base(message)
        {
            UnresolvedType = unresolvedType;
        }
    }
}
