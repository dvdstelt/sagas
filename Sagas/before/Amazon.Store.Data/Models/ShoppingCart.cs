using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Store.Data.Models
{
    public class ShoppingCart
    {
        public Guid CartId { get; set; }
        public bool IsPaid { get; set; }
        public ICollection<ShoppingCartItem> Products { get; set; }
    }

    public class ShoppingCartItem
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
