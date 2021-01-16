using System.Collections.Generic;

namespace RecipeLibrary.Interfaces
{
    public interface IService<T>
    {
        void Create(T obj);

        void Delete(T obj);

        List<T> GetAll();

        T Get(string searchStr = "");
    }
}