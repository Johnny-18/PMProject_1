using System.Collections.Generic;

namespace RecipeLibrary.Interfaces
{
    public interface IService<T>
    {
        void Add(T obj);

        void Delete(T obj);

        void Delete(string name);

        void Update(string name, T newObj);

        List<T> GetObjects(string name);

        T GetObject(string name);
    }
}