using System.ComponentModel.DataAnnotations;

namespace HelmoBilite_RC_LM.Models;

public class Client : Utilisateur
{
    [Required]
    [Display(Name = "Nom de l'entreprise")]
    public string CompanyName { get; set; }
    [Required]
    [Display(Name = "Adresse de l'entreprise")]
    public int IdAdresse { get; set; }
    
    public Adresse CompagnyAdress { get; set; }
    public ICollection<Livraison>? Livraisons { get; set; }
}