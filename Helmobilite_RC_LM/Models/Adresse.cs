using System.ComponentModel.DataAnnotations;

namespace HelmoBilite_RC_LM.Models;

public class Adresse
{
    public int Id { get; set; }
    [Required]
    public string Rue { get; set; }
    [Required]
    public int Numero { get; set; }
    [Required]
    public string Ville { get; set; }
    [Required]
    [Display(Name = "Code Postal")]
    public int CodePostal { get; set; }
    [Required]
    public string Pays { get; set; }

}