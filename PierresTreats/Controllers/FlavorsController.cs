using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PierresTreats.Models;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PierresTreats.Controllers
{
  [Authorize]
  public class FlavorsController : Controller
  {
    private readonly PierresTreatsContext _db;
    public FlavorsController(PierresTreatsContext db)
    {
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
      List<Flavor> allFlavors = _db.Flavors.ToList();
      return View(allFlavors);
    }

    public ActionResult Create()
    {
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public ActionResult Create(Flavor newFlavor)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
        return View(newFlavor);
      }
      else
      {
        _db.Flavors.Add(newFlavor);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    [AllowAnonymous]
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

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public ActionResult AddTreat(Treat selectedTreat, Flavor selectedFlavor)
    {
      #nullable enable
      TreatFlavor? joinEntity = _db.TreatFlavors.FirstOrDefault(join => (join.TreatId == selectedTreat.TreatId && join.FlavorId == selectedFlavor.FlavorId));
      #nullable disable
      if (joinEntity == null && selectedTreat.TreatId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { TreatId = selectedTreat.TreatId, FlavorId = selectedFlavor.FlavorId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", "Flavors", new { id = selectedFlavor.FlavorId });
    }

    public ActionResult Edit(int id)
    {
      Flavor selectedFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(selectedFlavor);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public ActionResult Edit(Flavor flavor)
    {
      _db.Flavors.Update(flavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      TreatFlavor joinEntity = _db.TreatFlavors.FirstOrDefault(join => join.TreatFlavorId == joinId);
      _db.TreatFlavors.Remove(joinEntity);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = joinEntity.FlavorId });
    }

    public ActionResult Delete(int id)
    {
      Flavor selectedFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      return View(selectedFlavor);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Flavor selectedFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
      _db.Flavors.Remove(selectedFlavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}