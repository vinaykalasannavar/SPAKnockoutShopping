using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Models
{
    public class CheckoutItem
    {
        public int CheckoutItemId { get; set; }

        //[Required]
        public int CategoryId { get; set; }
        //[Required]
        public int ProductId { get; set; }
        //[Required]
        public int Quantity { get; set; }

        //public double Price
        //{
        //    get
        //    {
        //        double price = 0;

        //        if (this.Product != null)
        //        {
        //            price = Product.Price;
        //        }

        //        return price;
        //    }
        //}

        //public double SubTotal { get; set; }
        //public ShoppingItem Product { get; set; }


        //[ForeignKey("CheckoutList")]
        //public int CheckoutListId { get; set; }

        //public CheckoutList CheckoutList { get; set; }
    }
}
