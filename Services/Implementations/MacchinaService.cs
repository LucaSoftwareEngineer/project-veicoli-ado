using project_veicoli_ado.Dao.Implementations.Tipologie;
using project_veicoli_ado.Dao.Interface;
using project_veicoli_ado.Dao.Interface.Tipologie;
using project_veicoli_ado.Models;
using project_veicoli_ado.Models.Tipologie;
using project_veicoli_ado.Services.Interface;
using project_veicoli_ado.ViewModels;

namespace project_veicoli_ado.Services.Implementations
{
    public class MacchinaService : IVeicoliService<MacchinaViewModel>
    {
        private readonly IMacchinaDao _macchinaDao;
        private readonly ITipoAlimentazioneDao _tipoAlimentazioneDao;
        private readonly ICategoriaDao _categoriaDao;

        public MacchinaService(IMacchinaDao macchinaDao, ITipoAlimentazioneDao tipoAlimentazioneDao, ICategoriaDao categoriaDao)
        {
            _macchinaDao = macchinaDao;
            _tipoAlimentazioneDao = tipoAlimentazioneDao;
            _categoriaDao = categoriaDao;
        }

        public bool DeleteById(int id)
        {
            try
            {
                _macchinaDao.Delete(id);
                return true;
            } catch
            {
                return false;
            }
        }

        public List<MacchinaViewModel> GetAll()
        {
            return _macchinaDao.GetAll().Select(macchina => new MacchinaViewModel
            {
                VeicoloId = macchina.VeicoloId,
                NumeroRuote = macchina.NumeroRuote,
                TipoAlimentazione = _tipoAlimentazioneDao.GetById(macchina.TipoAlimentazioneId),
                Categoria = _categoriaDao.GetById(macchina.CategoriaId),
                Colore = macchina.Colore,
                Marca = macchina.Marca,
                AnnoProduzione = macchina.AnnoProduzione,
                Modello = macchina.Modello,
                Targa = macchina.Targa,
                Cc = macchina.Cc,
                NumeroPorte = macchina.NumeroPorte
            }).ToList();
        }

        public MacchinaViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Insert(MacchinaViewModel t)
        {
            throw new NotImplementedException();
        }

        public bool Update(MacchinaViewModel t)
        {
            throw new NotImplementedException();
        }
    }
}
