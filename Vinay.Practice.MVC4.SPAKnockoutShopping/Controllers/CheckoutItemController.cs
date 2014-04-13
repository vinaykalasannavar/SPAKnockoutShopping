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
    public class CheckoutItemController : ApiController
    {
        private CheckoutItemContext db = new CheckoutItemContext();

        //// GET api/CheckoutItem
        //public IEnumerable<CheckoutItem> GetCheckoutItems()
        //{
        //    return db.ShoppingItems.AsEnumerable();
        //}

        //// GET api/CheckoutItem/5
        //public CheckoutItem GetCheckoutItem(int id)
        //{
        //    CheckoutItem checkoutitem = db.ShoppingItems.Find(id);
        //    if (checkoutitem == null)
        //    {
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        //    }

        //    return checkoutitem;
        //}

        // PUT api/CheckoutItem/5
        public HttpResponseMessage PutCheckoutItem(int id, CheckoutItem checkoutitem)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != checkoutitem.CheckoutItemId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(checkoutitem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/CheckoutItem
        public HttpResponseMessage PostCheckoutItem(CheckoutItem[] checkoutitems)
        {
            if (ModelState.IsValid)
            {
                //db.ShoppingItems.Add(checkoutitem);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, checkoutitems);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = checkoutitems }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/CheckoutItem/5
        public HttpResponseMessage DeleteCheckoutItem(int id)
        {
            CheckoutItem checkoutitem = db.CheckoutItems.Find(id);
            if (checkoutitem == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.CheckoutItems.Remove(checkoutitem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, checkoutitem);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}