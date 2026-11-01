using project_veicoli_ado.Models;

namespace project_veicoli_ado.Dao.Interface
{
    public interface IMacchinaDao
    {
        List<Macchina> GetAll();
        Macchina GetById(int id);
        void Insert(Macchina macchina);
        void Update(Macchina macchina);
        void Delete(int id);
    }
}
