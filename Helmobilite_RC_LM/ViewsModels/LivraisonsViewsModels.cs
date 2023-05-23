using System.ComponentModel.DataAnnotations;
using HelmoBilite_RC_LM.Models;

namespace Helmobilite_RC_LM.ViewsModels;

public class LivraisonsViewsModels
{
    [Required]
    [Display(Name = "Lieux de chargement")]
    public Adresse LieuChargement { get; set; }
    [Required]
    [Display(Name = "Lieux de dechargement")]
    public Adresse LieuDechargement { get; set; }
    [Required]
    [Display(Name = "Date de chargement")]
    public DateTime DateChargement { get; set; }
    [Required]
    [Display(Name = "Date de dechargement")]
    public DateTime DateDechargement { get; set; }
    [Required]
    [Display(Name = "Contenu")]
    public string Contenu { get; set; }
    
}