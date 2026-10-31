namespace project_veicoli_ado.Models
{
    public class Veicolo
    {
        public int Id { get; set; }
        public int NumeroRuote { get; set; }
        public int TipoAlimentazioneId { get; set; }
        public int CategoriaId { get; set; }
        public string Colore { get; set; }
        public string Marca { get; set; }
        public int AnnoProduzione { get; set; }
        public string Modello { get; set; }
    }
}
