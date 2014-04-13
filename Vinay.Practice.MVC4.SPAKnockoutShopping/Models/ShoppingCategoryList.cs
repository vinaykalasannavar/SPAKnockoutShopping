using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Models
{
    /// <summary>
    /// Shopping Category list entity
    /// </summary>
    public class ShoppingCategoryList
    {
        public int ShoppingCategoryListId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual List<ShoppingItem> ShoppingItems { get; set; }
    }
}