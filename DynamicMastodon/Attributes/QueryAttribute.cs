using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMastodon.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class QueryAttribute : Attribute
    {
        public String Query { get; private set; }
        public QueryAttribute(String query) { Query = query; }
    }
}
