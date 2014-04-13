using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Models
{
    public class CheckoutItemDto
    {

        //private ShoppingItemContext db = new ShoppingItemContext();

        public CheckoutItemDto() { }

        public CheckoutItemDto(CheckoutItem item)
        {
            CategoryId = item.CategoryId;
            ProductId = item.ProductId;
            Quantity = item.Quantity;
            //CheckoutListId = item.CheckoutListId;
        }

        //[Key]
        public int CheckoutItemId { get; set; }
        //[Required]
        public int CategoryId { get; set; }
        //[Required]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        //[Required]
        public int Quantity { get; set; }

        public double Price { get; set; }
        public double SubTotal { get; set; }

        //public ShoppingItem Product { get; set; }
        
        //public int CheckoutListId { get; set; }

        public CheckoutItem ToEntity()
        {
            return new CheckoutItem
            {
                ProductId = ProductId,
                CategoryId = CategoryId,
                Quantity = Quantity,
                //Product = FindProduct(ProductId),
                ////Price = Product.Price,
                //SubTotal = Quantity * Price
            };
        }

        //private ShoppingItem FindProduct(int productId)
        //{
        //    return db.ShoppingItems.Find(productId);
        //}
    }
}