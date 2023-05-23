using Helmobilite_RC_LM.Models;

namespace HelmoBilite_RC_LM.Models;

public class Camionneur : UtilisateurHelmo
{
    public string Permis { get; set; }

    public ICollection<OrdreLivraison>? OrdreLivraisons { get; set; }

}