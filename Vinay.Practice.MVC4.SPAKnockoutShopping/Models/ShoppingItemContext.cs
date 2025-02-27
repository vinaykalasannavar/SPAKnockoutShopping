﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Models
{
    // You can add custom code to this file. Changes will not be overwritten.
    // 
    // If you want Entity Framework to drop and regenerate your database
    // automatically whenever you change your model schema, add the following
    // code to the Application_Start method in your Global.asax file.
    // Note: this will destroy and re-create your database with every model change.
    // 
    // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Vinay.Practice.MVC4.SPAKnockoutShopping.Models.ShoppingItemContext>());
    public class ShoppingItemContext : DbContext
    {
        public ShoppingItemContext()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<ShoppingItem> ShoppingItems { get; set; }
        public DbSet<ShoppingCategoryList> ShoppingCategoryLists { get; set; }
    }
}