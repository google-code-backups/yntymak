using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Yntymak.Controllers
{
    public class SlideController : Controller
    {
        //
        // GET: /Slide/

        public PartialViewResult Slider()
        {
            return PartialView();
        }

    }
}
