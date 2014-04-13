using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Models
{
    public class CheckoutListDto
    {
        public CheckoutListDto() { }

        public CheckoutListDto(CheckoutList checkoutList)
        {
            CheckoutListId = checkoutList.CheckoutListId;
            UserId = checkoutList.UserId;
            //TotalPrice = checkoutList.TotalPrice;
            CheckoutItems = new List<CheckoutItemDto>();
            foreach (CheckoutItem item in checkoutList.CheckoutItems)
            {
                CheckoutItems.Add(new CheckoutItemDto(item));
            }
        }
        
        [Key]
        public int CheckoutListId { get; set; }
        public string UserId { get; set; }
        public double TotalPrice { get; set; }

        public virtual List<CheckoutItemDto> CheckoutItems { get; set; }


        public CheckoutList ToEntity()
        {
            CheckoutList checkOutList = new CheckoutList
            {
                CheckoutListId = CheckoutListId,
                UserId = UserId,
                //TotalPrice = TotalPrice,
                CheckoutItems = new List<CheckoutItem>()
            };

            foreach (CheckoutItemDto item in CheckoutItems)
            {
                checkOutList.CheckoutItems.Add(item.ToEntity());
            }

            return checkOutList;
        }

    }
}