using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PierresTreats.Models;

namespace PierresTreats.Controllers
{
  public class EngineersController : Controller
  {
    private readonly PierresTreatsContext _db;
    public EngineersController(PierresTreatsContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Treat> allTreats = _db.Treats.ToList();
      return View(allTreats);
    }
  }
}