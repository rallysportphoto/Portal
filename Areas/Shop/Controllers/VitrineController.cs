/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Areas.Shop.Models;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Areas.Shop.Controllers
{
    public class VitrineController : Controller
    {
        //
        // GET: /Shop/Vitrine/
        public ActionResult Index(string uri)
        {
            var tokens = (uri ?? "").Split('/');

            if(string.IsNullOrWhiteSpace(uri)) return ShopHome();
            else if (SkuExists(tokens[tokens.GetUpperBound(0)]))
            {
                return SKU(tokens);
            }
            else return ShowCategory(tokens);
        }

        private bool SkuExists(string sku)
        {
            using (var ctx = new SportDataContext())
            {
                return ctx.Products.Any(p => p.SKU == sku && p.Enabled);
            }
        }

        public ActionResult ShopHome()
        {
            using (var ctx = new SportDataContext())
            {
                var items = ctx.Products.Where(p => p.Featured);
                var categories = ctx.Categories.Where(c => c.Parent == null);
            }
            return Content("HOME");
        }

        public ActionResult ShowCategory(string[] tokens)
        {
            using (var ctx = new SportDataContext())
            {
                Category category=null;

                foreach (var token in tokens)
                {
                    category = GetCategory(token, category, ctx);
                    if (category == null) return HttpNotFound();
                }
                return View(category);
            }
        }

        private Category GetCategory(string token, Category category, SportDataContext ctx)
        {
            return ctx.Categories.FirstOrDefault(c => c.Parent == category && c.Slug == token);
        }

        public ActionResult SKU(string[] tokens)
        {
            using (var ctx = new SportDataContext())
            {
                var product = ctx.Products.FirstOrDefault(p => p.SKU == tokens[tokens.GetUpperBound(0)]);
                if (product != null) return View(product);
                return HttpNotFound();
            }
        }
	}
}