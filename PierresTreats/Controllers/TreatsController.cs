using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PierresTreats.Models;
using System.Collections.Generic;
using System.Linq; 

namespace SweetAndSavoryTreats.Controllers
{
  public class TreatsController : Controller
  {
    private readonly PierresTreatsContext _db;

    public TreatsController(PierresTreatsContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Treat> pierresTreats = _db.Treats.ToList();
      return View(pierresTreats);
    }

    public ActionResult Create()
    {
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Treat newTreat)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.EngineerId = new SelectList(_db.Flavors, "FlavorId", "Type");
        return View(newTreat);
      }
      else
      {
        _db.Treats.Add(newTreat);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
    
    public ActionResult Details(int id)
    {
      Treat selectedTreat = _db.Treats
                                   .Include(treat => treat.JoinEntities)
                                   .ThenInclude(joinEntity => joinEntity.Flavor)
                                   .FirstOrDefault(treat => treat.TreatId == id);
      return View(selectedTreat);
    }
  }
}