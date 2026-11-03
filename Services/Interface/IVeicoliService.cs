using project_veicoli_ado.Models;

namespace project_veicoli_ado.Services.Interface
{
    public interface IVeicoliService<T> where T : Veicolo
    {
        public bool Insert(T t);
        public bool Update(T t);
        public List<T> GetAll();
        public T GetById(int id);
        public bool DeleteById(int id);
    }
}
