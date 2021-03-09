using DesafioPG.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DesafioPG.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            Hero Api = new Hero();
            var result = Api.GetHeros();

            return View();
        }
    }
}
