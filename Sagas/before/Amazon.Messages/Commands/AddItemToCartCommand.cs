using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Messages.Commands
{
    public class AddItemToCartCommand
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
    }
}
