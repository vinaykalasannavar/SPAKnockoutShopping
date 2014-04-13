using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vinay.Practice.MVC4.SPAKnockoutShopping.Filters;
using Vinay.Practice.MVC4.SPAKnockoutShopping.Models;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Controllers
{
    [Authorize]
    [ValidateHttpAntiForgeryToken]
    public class ShoppingItemController : ApiController
    {
        private ShoppingItemContext db = new ShoppingItemContext();

        // PUT api/ShoppingItem/5
        public HttpResponseMessage PutShoppingItem(int id, ShoppingItemDto shoppingItemDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != shoppingItemDto.ShoppingItemId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            ShoppingItem shoppingItem = shoppingItemDto.ToEntity();
            ShoppingCategoryList shoppingcategoryList = db.ShoppingCategoryLists.Find(shoppingItem.ShoppingCategoryListId);
            if (shoppingcategoryList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (shoppingcategoryList.UserId != User.Identity.Name)
            {
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            // Need to detach to avoid duplicate primary key exception when SaveChanges is called
            db.Entry(shoppingcategoryList).State = EntityState.Detached;
            db.Entry(shoppingItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/ShoppingItem
        public HttpResponseMessage PostShoppingItem(ShoppingItemDto shoppingItemDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ShoppingCategoryList shoppingCategoryList = db.ShoppingCategoryLists.Find(shoppingItemDto.ShoppingCategoryListId);
            if (shoppingCategoryList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (shoppingCategoryList.UserId != User.Identity.Name)
            {
                // Trying to add a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            ShoppingItem shoppingItem = shoppingItemDto.ToEntity();

            // Need to detach to avoid loop reference exception during JSON serialization
            db.Entry(shoppingCategoryList).State = EntityState.Detached;
            db.ShoppingItems.Add(shoppingItem);
            db.SaveChanges();
            shoppingItemDto.ShoppingItemId = shoppingItem.ShoppingItemId;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, shoppingItemDto);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = shoppingItemDto.ShoppingItemId }));
            return response;
        }

        // DELETE api/ShoppingItem/5
        public HttpResponseMessage DeleteShoppingItem(int id)
        {
            ShoppingItem shoppingItem = db.ShoppingItems.Find(id);
            if (shoppingItem == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (db.Entry(shoppingItem.ShoppingCategoryList).Entity.UserId != User.Identity.Name)
            {
                // Trying to delete a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            ShoppingItemDto shoppingItemDto = new ShoppingItemDto(shoppingItem);
            db.ShoppingItems.Remove(shoppingItem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK, shoppingItemDto);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}