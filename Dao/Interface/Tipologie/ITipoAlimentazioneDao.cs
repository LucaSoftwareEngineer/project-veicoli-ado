using project_veicoli_ado.Models.Tipologie;

namespace project_veicoli_ado.Dao.Interface.Tipologie
{
    public interface ITipoAlimentazioneDao
    {
        public TipoAlimentazione GetById(int id);
    }
}
