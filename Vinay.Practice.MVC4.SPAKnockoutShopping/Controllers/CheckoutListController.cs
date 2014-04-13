using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Vinay.Practice.MVC4.SPAKnockoutShopping.Models;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Controllers
{
    public class CheckoutListController : ApiController
    {
        private CheckoutItemContext coDb = new CheckoutItemContext();
        private ShoppingItemContext tdDb = new ShoppingItemContext();

        // GET api/CheckoutList
        public IEnumerable<CheckoutListDto> GetCheckoutLists()
        {
            return coDb.CheckoutLists
                .Select(checkoutList => new CheckoutListDto(checkoutList)); ;
        }

        // GET api/CheckoutList/5
        public CheckoutListDto GetCheckoutList(int id)
        {
            CheckoutList checkoutlist = coDb.CheckoutLists.Find(id);
            if (checkoutlist == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return new CheckoutListDto(checkoutlist);
        }

        // PUT api/CheckoutList/5
        public HttpResponseMessage PutCheckoutList(int id, CheckoutListDto checkoutlistDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != checkoutlistDto.CheckoutListId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            coDb.Entry(checkoutlistDto).State = EntityState.Modified;

            try
            {
                coDb.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/CheckoutList
        public HttpResponseMessage PostCheckoutList(CheckoutListDto checkoutlist)
        {
            if (ModelState.IsValid)
            {

                foreach (CheckoutItemDto item in checkoutlist.CheckoutItems)
                {
                    var product = tdDb.ShoppingItems.Find(item.ProductId);
                    item.SubTotal = product.Price * item.Quantity;
                    checkoutlist.TotalPrice += item.SubTotal;
                    item.Price = product.Price;
                    item.ProductName = product.Title;
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, checkoutlist);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = checkoutlist.CheckoutListId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/CheckoutList/5
        public HttpResponseMessage DeleteCheckoutList(int id)
        {
            CheckoutList checkoutlist = coDb.CheckoutLists.Find(id);
            if (checkoutlist == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            coDb.CheckoutLists.Remove(checkoutlist);

            try
            {
                coDb.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, checkoutlist);
        }

        protected override void Dispose(bool disposing)
        {
            coDb.Dispose();
            base.Dispose(disposing);
        }
    }
}