using project_veicoli_ado.Models.Tipologie;

namespace project_veicoli_ado.ViewModels
{
    public class VeicoloViewModel
    {
        public int VeicoloId { get; set; }
        public int NumeroRuote { get; set; }
        public TipoAlimentazione TipoAlimentazione { get; set; }
        public Categoria Categoria { get; set; }
        public string Colore { get; set; }
        public string Marca { get; set; }
        public int AnnoProduzione { get; set; }
        public string Modello { get; set; }
    }
}
