using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Models
{
    public class ShoppingItemDto
    {
        /// <summary>
        /// Data transfer object for <see cref="ShoppingItem"/>
        /// </summary>
        public ShoppingItemDto() { }

        public ShoppingItemDto(ShoppingItem item)
        {
            ShoppingItemId = item.ShoppingItemId;
            Title = item.Title;
            IsDone = item.IsDone;
            Price = item.Price;
            ShoppingCategoryListId = item.ShoppingCategoryListId;
        }

        [Key]
        public int ShoppingItemId { get; set; }

        [Required]
        public string Title { get; set; }

        public bool IsDone { get; set; }
        public double Price { get; set; }

        public int ShoppingCategoryListId { get; set; }

        public ShoppingItem ToEntity()
        {
            return new ShoppingItem
            {
                ShoppingItemId = ShoppingItemId,
                Title = Title,
                IsDone = IsDone,
                ShoppingCategoryListId = ShoppingCategoryListId,
                Price = Price
            };
        }
    }
}
