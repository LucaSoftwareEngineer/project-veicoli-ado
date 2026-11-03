using project_veicoli_ado.Dao.Interface;
using project_veicoli_ado.Models;
using project_veicoli_ado.Services.Interface;

namespace project_veicoli_ado.Services.Implementations
{
    public class MacchinaService : IVeicoliService<Macchina>
    {
        private readonly IMacchinaDao _macchinaDao;

        public MacchinaService(IMacchinaDao macchinaDao)
        {
            _macchinaDao = macchinaDao;
        }

        public bool DeleteById(int id)
        {
            try
            {
                _macchinaDao.Delete(id);
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        public List<Macchina> GetAll()
        {
            return _macchinaDao.GetAll();
        }

        public Macchina GetById(int id)
        {
            return _macchinaDao.GetById(id);
        }

        public bool Insert(Macchina t)
        {
            try
            {
                _macchinaDao.Insert(t);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(Macchina t)
        {
            try
            {
                _macchinaDao.Update(t);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
