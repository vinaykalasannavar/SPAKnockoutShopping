using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Models
{
    public class CheckoutList
    {
        public int CheckoutListId { get; set; }
        public string UserId { get; set; }
        //public double TotalPrice
        //{
        //    get
        //    {
        //        double totalPrice = 0;
        //        foreach (CheckoutItem item in CheckoutItems)
        //        {
        //            totalPrice = item.Price * item.Quantity;
        //        }

        //        return totalPrice;
        //    }
        //}
        public List<CheckoutItem> CheckoutItems { get; set; }
    }
}