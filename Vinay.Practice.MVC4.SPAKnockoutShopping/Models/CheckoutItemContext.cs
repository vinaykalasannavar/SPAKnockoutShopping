using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Models
{
    public class CheckoutItemContext
    
        : DbContext
    {
        public CheckoutItemContext()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<CheckoutItem> CheckoutItems { get; set; }
        public DbSet<CheckoutList> CheckoutLists { get; set; }
    }
}