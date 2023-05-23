using System.ComponentModel.DataAnnotations;
using HelmoBilite_RC_LM.Models;

namespace Helmobilite_RC_LM.Models;

public class UtilisateurHelmo : Utilisateur
{
    [Required]
    public string Nom { get; set; }
    [Required]
    public string Prenom { get; set; }
    [Required]
    public string Matricule { get; set; }
}