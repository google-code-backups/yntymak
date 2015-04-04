using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Yntymak.Controllers
{
    public class NavController : Controller
    {
        //
        // GET: /Nav/

        public PartialViewResult Menu()
        {
            return PartialView();
        }

        public PartialViewResult BottomMenu()
        {
            return PartialView();
        }

    }
}
