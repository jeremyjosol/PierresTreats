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
  }
}