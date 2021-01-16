using System.Collections.Generic;

namespace RecipeLibrary.Interfaces
{
    public interface IFileWorker
    {
        void Serialize<T>(T obj);
        List<T> Deserialize<T>();
    }
}