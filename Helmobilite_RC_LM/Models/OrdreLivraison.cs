namespace HelmoBilite_RC_LM.Models
{
    public class OrdreLivraison
    {
        public int Id { get; set; }
        public Livraison Livraison { get; set; }
        public Camionneur Camionneur { get; set; }  
        public Camion Camion { get; set; }

    }
}
