namespace project_veicoli_ado.Models
{
    public class Macchina : Veicolo
    {
        public int MacchinaId { get; set; }
        public string Targa { get; set; }
        public int Cc { get; set; }
        public int NumeroPorte { get; set; }
    }
}
