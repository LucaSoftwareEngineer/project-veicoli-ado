namespace project_veicoli_ado.Models
{
    public class Bici : Veicolo
    {
        public int BiciId { get; set; }
        public int NumeroCorone { get; set; }
        public int NumeroMarce { get; set; }
        public int TipoFrenoId { get; set; }
        public int TipoSospensioneId { get; set; }
        public bool Pieghevole { get; set; }
    }
}
