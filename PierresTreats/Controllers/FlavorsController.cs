using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PierresTreats.Models;

namespace PierresTreats.Controllers
{
  public class FlavorsController : Controller
  {
    private readonly PierresTreatsContext _db;
    public FlavorsController(PierresTreatsContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Treat> allTreats = _db.Treats.ToList();
      return View(allTreats);
    }

     public ActionResult Create()
    {
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Flavor newFlavor)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.MachineId = new SelectList(_db.Treats, "TreatId", "Name");
        return View(newFlavor);
      }
      else
      {
        _db.Flavors.Add(newFlavor);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      Flavor selectedFlavor = _db.Flavors
                                     .Include(flavor => flavor.JoinEntities)
                                     .ThenInclude(joinEntity => joinEntity.Treat)
                                     .FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(selectedFlavor);
    }

    public ActionResult AddTreat(int id)
    {
      Flavor flavorToAddTreatTo = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      return View(flavorToAddTreatTo);
    }

    [HttpPost]
    public ActionResult Register(Treat selectedTreat, Flavor selectedFlavor)
    {
      #nullable enable
      TreatFlavor? joinEntity = _db.TreatFlavors.FirstOrDefault(join => (join.TreatId == selectedTreat.TreatId && join.FlavorId == selectedFlavor.FlavorId));
      #nullable disable
      if (joinEntity == null && selectedTreat.TreatId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { TreatId = selectedTreat.TreatId, FlavorId = selectedFlavor.FlavorId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = selectedFlavor.FlavorId });
    }

    public ActionResult Edit(int id)
    {
      Flavor selectedFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(selectedFlavor);
    }

    [HttpPost]
    public ActionResult Edit(Flavor flavor)
    {
      _db.Flavors.Update(flavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}