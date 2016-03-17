using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Messages.Events
{
    public class OrderFinalizedEvent
    {
        public Guid CartId { get; set; }
        public List<Guid> Products { get; set; }
    }
}
