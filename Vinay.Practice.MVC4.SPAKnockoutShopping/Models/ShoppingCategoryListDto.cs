using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Models
{
    /// <summary>
    /// Data transfer object for <see cref="ShoppingCategoryList"/>
    /// </summary>
    public class ShoppingCategoryListDto
    {
        public ShoppingCategoryListDto() { }

        public ShoppingCategoryListDto(ShoppingCategoryList shoppingCategoryList)
        {
            ShoppingCategoryListId = shoppingCategoryList.ShoppingCategoryListId;
            UserId = shoppingCategoryList.UserId;
            Title = shoppingCategoryList.Title;
            ShoppingItems = new List<ShoppingItemDto>();
            foreach (ShoppingItem item in shoppingCategoryList.ShoppingItems)
            {
                ShoppingItems.Add(new ShoppingItemDto(item));
            }
        }

        [Key]
        public int ShoppingCategoryListId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual List<ShoppingItemDto> ShoppingItems { get; set; }

        public ShoppingCategoryList ToEntity()
        {
            ShoppingCategoryList shoppingItem = new ShoppingCategoryList
            {
                Title = Title,
                ShoppingCategoryListId = ShoppingCategoryListId,
                UserId = UserId,
                ShoppingItems = new List<ShoppingItem>()
            };
            foreach (ShoppingItemDto item in ShoppingItems)
            {
                shoppingItem.ShoppingItems.Add(item.ToEntity());
            }

            return shoppingItem;
        }
    }
}