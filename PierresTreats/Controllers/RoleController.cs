using Microsoft.AspNetCore.Mvc;
using PierresTreats.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace RecipeBook.Controllers
{
  public class RoleController : Controller  
  {  
    private readonly RoleManager<IdentityRole> _roleManager;  
    private readonly UserManager<ApplicationUser> _userManager;  
    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)  
    {  
      _roleManager = roleManager;  
      _userManager = userManager;  
    }

    private void Errors(IdentityResult result)
    {
      foreach (IdentityError error in result.Errors)
      ModelState.AddModelError("", error.Description);
    }

    public ViewResult Index()
    {
      List<IdentityRole> roles = _roleManager.Roles.ToList();
      Dictionary<IdentityRole, List<ApplicationUser>> usersWithRole = new Dictionary<IdentityRole, List<ApplicationUser>>();

      foreach (IdentityRole role in roles)
      {
        List<ApplicationUser> usersInRole = _userManager.GetUsersInRoleAsync(role.Name).Result.ToList();
        usersWithRole.Add(role, usersInRole);
      }
      return View(usersWithRole);
    }

    public IActionResult Create() => View();
      
    [HttpPost]
    public async Task<IActionResult> Create([Required] string name)
    {
      if (ModelState.IsValid)
      {
        IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
        if (result.Succeeded)
        return RedirectToAction("Index");
        else
        Errors(result);
      }
      return View(name);
    }
  }
}