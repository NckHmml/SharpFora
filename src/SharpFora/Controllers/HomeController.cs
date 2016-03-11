using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SharpFora.Attributes;
using SharpFora.DAL;

namespace SharpFora.Controllers
{
    [Static]
    public class HomeController : Controller
    {
        public SharpForaContext DbContext { get; set; }

        public HomeController(SharpForaContext dbContext)
        {
            DbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Templates()
        {
            return View();
        }
    }
}
