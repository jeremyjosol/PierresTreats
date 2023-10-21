using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PierresTreats.Models
{
  public class Treat
  {
    public int TreatId { get; set; }
    [Required(ErrorMessage = "* Please enter a valid entry for this field.")]
    public string Name { get; set; }
    public List<TreatFlavor> JoinEntities { get; set; }
  }
}