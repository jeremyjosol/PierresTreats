using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PierresTreats.Models
{
  public class Flavor 
  {
    public int FlavorId { get; set; }
    [Required(ErrorMessage = "* Please enter a valid entry for this field.")]
    public string Type { get; set; }
    public List<TreatFlavor> JoinEntities { get; set; }
  }
}