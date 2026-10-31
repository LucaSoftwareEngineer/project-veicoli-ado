namespace project_veicoli_ado.Models
{
    public class Moto : Veicolo
    {
        public int MotoId { get; set; }
        public string Targa { get; set; }
        public int Cc { get; set; }
    }
}
