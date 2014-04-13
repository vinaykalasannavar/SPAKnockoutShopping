using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vinay.Practice.MVC4.SPAKnockoutShopping.Filters;
using Vinay.Practice.MVC4.SPAKnockoutShopping.Models;

namespace Vinay.Practice.MVC4.SPAKnockoutShopping.Controllers
{
    [Authorize]
    public class ShoppingCategoryListController : ApiController
    {
        private ShoppingItemContext db = new ShoppingItemContext();

        // GET api/ShoppingCategoryList
        public IEnumerable<ShoppingCategoryListDto> GetShoppingCategoryLists()
        {
            var shoppingCategoryLists = db.ShoppingCategoryLists.Include("ShoppingItems")
                .Where(u => u.UserId == User.Identity.Name)
                .OrderByDescending(u => u.ShoppingCategoryListId)
                .AsEnumerable()
                .Select(shoppingCategoryList => new ShoppingCategoryListDto(shoppingCategoryList));

            return shoppingCategoryLists;
        }

        // GET api/ShoppingCategoryList/5
        public ShoppingCategoryListDto GetShoppingCategoryList(int id)
        {
            ShoppingCategoryList shoppingCategoryList = db.ShoppingCategoryLists.Find(id);
            if (shoppingCategoryList == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            if (shoppingCategoryList.UserId != User.Identity.Name)
            {
                // Trying to modify a record that does not belong to the user
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized));
            }

            return new ShoppingCategoryListDto(shoppingCategoryList);
        }

        // PUT api/ShoppingCategoryList/5
        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage PutShoppingCategoryList(int id, ShoppingCategoryListDto shoppingCategoryListDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != shoppingCategoryListDto.ShoppingCategoryListId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            ShoppingCategoryList shoppingCategoryList = shoppingCategoryListDto.ToEntity();
            if (db.Entry(shoppingCategoryList).Entity.UserId != User.Identity.Name)
            {
                // Trying to modify a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            db.Entry(shoppingCategoryList).State = EntityState.Modified;

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

        // POST api/ShoppingCategoryList
        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage PostShoppingCategoryList(ShoppingCategoryListDto shoppingCategoryListDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            shoppingCategoryListDto.UserId = User.Identity.Name;
            ShoppingCategoryList shoppingCategoryList = shoppingCategoryListDto.ToEntity();
            db.ShoppingCategoryLists.Add(shoppingCategoryList);
            db.SaveChanges();
            shoppingCategoryListDto.ShoppingCategoryListId = shoppingCategoryList.ShoppingCategoryListId;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, shoppingCategoryListDto);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = shoppingCategoryListDto.ShoppingCategoryListId }));
            return response;
        }

        // DELETE api/ShoppingCategoryList/5
        [ValidateHttpAntiForgeryToken]
        public HttpResponseMessage deleteShoppingCategoryItem(int id)
        {
            ShoppingCategoryList shoppingCategoryList = db.ShoppingCategoryLists.Find(id);
            if (shoppingCategoryList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (db.Entry(shoppingCategoryList).Entity.UserId != User.Identity.Name)
            {
                // Trying to delete a record that does not belong to the user
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            ShoppingCategoryListDto shoppingCategoryListDto = new ShoppingCategoryListDto(shoppingCategoryList);
            db.ShoppingCategoryLists.Remove(shoppingCategoryList);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK, shoppingCategoryListDto);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}