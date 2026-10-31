using project_veicoli_ado.Models;

namespace project_veicoli_ado.Dao.Interface
{
    public interface IBiciDao
    {
        List<Bici> GetAll();
        Bici GetById(int id);
        void Insert(Bici bici);
        void Update(Bici bici);
        void Delete(int id);
    }
}
