using project_veicoli_ado.Models;

namespace project_veicoli_ado.Dao.Interface
{
    public interface IMotoDao
    {
        List<Moto> GetAll();
        Moto GetById(int id);
        void Insert(Moto moto);
        void Update(Moto moto);
        void Delete(int id);
    }
}
