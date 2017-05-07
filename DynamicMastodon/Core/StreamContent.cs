using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMastodon.Core
{
    public class StreamContent
    {
        public dynamic Statuses { get; set; }
        public int? Prev { get; set; }
        public int? Next { get; set; }
    }
}
