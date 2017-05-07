using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMastodon.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodAttribute : Attribute
    {
        public Method Method { get; private set; }
        public MethodAttribute(Method method) { Method = method; }
    }
}
