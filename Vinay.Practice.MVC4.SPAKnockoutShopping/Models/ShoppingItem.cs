using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Models
{
    /// <summary>
    /// ShoppingItem item entity
    /// </summary>
    public class ShoppingItem
    {
        public int ShoppingItemId { get; set; }

        [Required]
        public string Title { get; set; }
        public bool IsDone { get; set; }

        public double Price { get; set; }

        [ForeignKey("ShoppingCategoryList")]
        public int ShoppingCategoryListId { get; set; }
        public virtual ShoppingCategoryList ShoppingCategoryList { get; set; }
    }
}