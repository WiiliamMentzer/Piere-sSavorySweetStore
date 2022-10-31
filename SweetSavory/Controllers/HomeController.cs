using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SweetSavory.Models;
using System.Collections.Generic;
using System.Linq;

namespace SweetSavory.Controllers
{
    public class HomeController : Controller
    {
      private readonly SweetSavoryContext _db;

      public HomeController(SweetSavoryContext db)
      {
        _db = db;
      }

      public ActionResult Index()
      {
        ViewBag.Treat = new List<Treat>( _db.Treats);
        return View( _db.Flavors.ToList());
      }
    }
}