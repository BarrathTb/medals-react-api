using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class Country
{
  [Key]
  public int Id { get; set; }

  [Required]
  public string Name { get; set; }

  public int Gold { get; set; }
  public int Silver { get; set; }
  public int Bronze { get; set; }




}
