using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using RecipeLibrary.Interfaces;

namespace RecipeLibrary.Services
{
    public class FileWorker : IFileWorker
    {
        private readonly string _path = "recipes.json";

        public void Serialize<T>(T obj)
        {
            if(obj == null)
                throw new ArgumentNullException();

            try
            {
                var json = JsonSerializer.Serialize(obj);
                File.WriteAllText(_path, json);
            }
            catch
            {
                Console.WriteLine("Something wrong with file writing!");
            }
        }

        public List<T> Deserialize<T>()
        {
            try
            {
                var jsonFromFile = File.ReadAllText(_path);
                return JsonSerializer.Deserialize<List<T>>(jsonFromFile);
            }
            catch
            {
                Console.WriteLine("Something wrong with file reading!");
                return null;
            }
        }
    }
}