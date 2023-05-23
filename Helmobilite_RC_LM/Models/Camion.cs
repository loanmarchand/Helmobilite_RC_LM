using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelmoBilite_RC_LM.Models;

public class Camion
{
    public int Id { get; set; }
    [Required]
    public string Marque { get; set; }
    [Required]
    public string Modele { get; set; }
    [Required]
    [Display(Name = "Plaque d'immatriculation")]
    public string PlaqueImmatriculation { get; set; }
    [Required]
    public string Type { get; set; }
    [Required]
    [Display(Name = "Tonnage maximum")]
    public int TonnageMaximum { get; set; }
        
    [NotMapped]
    public IFormFile PhotoFile { get; set; }
    [Required]
    public string Photo { get; set; }
    [Required]
    public string Statut { get; set; }

    public ICollection<OrdreLivraison>? OrdreLivraisons { get; set; }
}