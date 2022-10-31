using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SweetSavory.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SweetSavory.Controllers
{
  [Authorize]
  public class TreatsController : Controller
  {
    private readonly SweetSavoryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public TreatsController(UserManager<ApplicationUser> userManager, SweetSavoryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var userTreats = _db.Treats.Where(entry => entry.User.Id == currentUser.Id).ToList().OrderByDescending(entry=> entry.Rating);
      return View(userTreats);
    }

    public ActionResult Create()
    {
    //     var ingredients = _db.Ingredients.Select(c => new { 
    //     IngredientId = c.IngredientId, 
    //     IngredientName = c.IngredientName 
    // }).ToList();

      List<SelectListItem> Rating = new List<SelectListItem>();
      // ViewBag.TreatSelection = new MultiSelectList(Treats, "TreatId", "TreatName");

      // foreach (Ingredient ingredient in _db.Ingredients)
      // {
      //   IngredientSelection.Add(new SelectListItem { Text = ingredient.IngredientName, Value= ingredient.IngredientName});
      // }

      Rating.Add(new SelectListItem { Text = "5", Value = "5"});
      Rating.Add(new SelectListItem { Text = "4", Value = "4"});
      Rating.Add(new SelectListItem { Text = "3", Value = "3"});
      Rating.Add(new SelectListItem { Text = "2", Value = "2"});
      Rating.Add(new SelectListItem { Text = "1", Value = "1"});

      // ViewBag.IngredientSelection = IngredientSelection;
      ViewBag.Rating = Rating;

      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Treat treat, int FlavorId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      treat.User = currentUser;
      _db.Treats.Add(treat);
      _db.SaveChanges();
      if (FlavorId != 0)
      {
        _db.FlavorTreat.Add(new FlavorTreat() { FlavorId = FlavorId, TreatId = treat.TreatId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisTreat = _db.Treats
          .Include(Treat => Treat.JoinEntities)
          .ThenInclude(join => join.Flavor)
          .FirstOrDefault(Treat => Treat.TreatId == id);
      return View(thisTreat);
    }

    public ActionResult Edit(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(Treat => Treat.TreatId == id);
      ViewBag.FlavorId = new SelectList( _db.Flavors, "FlavorId", "FlavorName");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Entry(treat).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddFlavor(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(Treat => Treat.TreatId == id);
      ViewBag.FlavorId = new SelectList( _db.Flavors, "FlavorId", "FlavorName");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult AddFlavor(Treat Treat, int FlavorId)
    {
      if (FlavorId != 0)
      {
        _db.FlavorTreat.Add(new FlavorTreat() { TreatId = Treat.TreatId, FlavorId = FlavorId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    // public ActionResult AddIngredient(int id)
    // {
    //   var thisTreat = _db.Treats.FirstOrDefault(Treat => Treat.TreatId == id);
    //   ViewBag.IngredientId = new SelectList( _db.Ingredients, "IngredientId", "IngredientName");
    //   return View(thisTreat);
    // }

    // [HttpPost]
    // public ActionResult AddIngredient(Recipe Recipe, int IngredientId)
    // {
    //   if (IngredientId != 0)
    //   {
    //     _db.RecipeIngredient.Add(new RecipeIngredient() { RecipeId = Recipe.RecipeId, IngredientId = IngredientId });
    //     _db.SaveChanges();
    //   }
    //   return RedirectToAction("Index");
    // }

    public ActionResult Delete(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(Treat => Treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisTreat = _db.Treats.FirstOrDefault(Treat => Treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    // [HttpPost]
    // public ActionResult RemoveIngredient(int joinId)
    // {
    //   var joinEntry = _db.TreatIngredient.FirstOrDefault(entry => entry.TreatIngredientId == joinId);
    //   _db.TreatIngredient.Remove(joinEntry);
    //   _db.SaveChanges();
    //   return RedirectToAction("Index");
    // }
  }
}
