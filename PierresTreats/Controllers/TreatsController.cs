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

    public ActionResult AddFlavor(int id)
    {
      Treat treatToAddFlavorTo = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
      return View(treatToAddFlavorTo);
    }

    [HttpPost]
    public ActionResult AddFlavor(Flavor selectedFlavor, Treat selectedTreat)
    {
      #nullable enable
      TreatFlavor? joinEntity = _db.TreatFlavors.FirstOrDefault(join => (join.FlavorId == selectedFlavor.FlavorId && join.TreatId == selectedTreat.TreatId));
      #nullable disable
      if (joinEntity == null && selectedFlavor.FlavorId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { FlavorId = selectedFlavor.FlavorId, TreatId = selectedTreat.TreatId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", "Engineers", new { id = selectedTreat.TreatId });
    }

    public ActionResult Edit(int id)
    {
      Treat selectedTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
      return View(selectedTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Treats.Update(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      TreatFlavor joinEntity = _db.TreatFlavors.FirstOrDefault(join => join.TreatFlavorId == joinId);
      _db.TreatFlavors.Remove(joinEntity);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntity.TreatId });
    }

    public ActionResult Delete(int id)
    {
      Treat selectedTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(selectedTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat selectedTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _db.Treats.Remove(selectedTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}