using System.Collections.Generic;
using RecipeLibrary.Interfaces;

namespace RecipeLibrary.Services
{
    public class FileWorker : IFileWorker
    {
        public void Serialize<T>(T obj)
        {
            throw new System.NotImplementedException();
        }

        public List<T> Deserialize<T>()
        {
            throw new System.NotImplementedException();
        }
    }
}