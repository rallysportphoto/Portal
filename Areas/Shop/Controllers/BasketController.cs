/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Areas.Shop.Controllers
{
    public class BasketController : Controller
    {
        //
        // GET: /Shop/Basket/
        public ActionResult Index()
        {
            return View();
        }
	}
}