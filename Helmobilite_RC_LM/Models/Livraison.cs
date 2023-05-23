using System.ComponentModel.DataAnnotations;

namespace HelmoBilite_RC_LM.Models;

public class Livraison
{
    public int Id { get; set; }
    [Required]
    [Display(Name = "Lieu de chargement")]
    public int LieuChargementId { get; set; }
    [Required]
    [Display(Name = "Lieu de déchargement")]
    public int LieuDechargementId { get; set; }
    [Required]
    public string Contenu { get; set; }
    [Required]
    [Display(Name = "Date de chargement")]
    public DateTime DateHeureChargement { get; set; }
    [Required]
    [Display(Name = "Date de déchargement")]
    public DateTime DateHeureDechargement { get; set; }
    [Required]
    public string Statut { get; set; }
    [Required]
    [Display(Name = "Client")]
    public string ClientId { get; set; }
    public Client Client { get; set; }

    public Adresse LieuChargement { get; set; }
    public Adresse LieuDechargement { get; set; }

    public bool CheckDate()
    {
        return DateHeureChargement < DateHeureDechargement;
    }
}