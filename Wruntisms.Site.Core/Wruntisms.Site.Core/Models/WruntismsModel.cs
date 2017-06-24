using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wruntisms.Site.Core.Models
{
    using System.IO;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class WruntismsModel
    {
        private readonly AppSettings settings;
        public List<Wruntism> Wruntisms { get; set; } = new List<Wruntism>();

        public WruntismsModel(AppSettings appSettings)
        {
            settings = appSettings;
        }

        public void LoadFromFile(string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = settings.WruntismsFile;
            }

            string json = File.ReadAllText(filePath);

            var arr = (JArray) JsonConvert.DeserializeObject(json);

            Wruntisms = arr.ToObject<List<Wruntism>>();
        }

        public void FixIds()
        {
            for (int i = 0; i < Wruntisms.Count; i++)
            {
                Wruntisms[i].Id = i + 1;
            }
        }

        public void SaveToFile(string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = settings.WruntismsFile;
            }

            string json = JsonConvert.SerializeObject(Wruntisms);

            File.WriteAllText(filePath, json);
        }

        public void RemoveWruntismById(int id)
        {
            Wruntisms.RemoveAll(x => x.Id == id);
        }

        public void AddOrUpdateWruntism(string message, int id = 0)
        {
            if (id == 0)
            {
                id = Wruntisms.Max(x => x.Id) + 1;
            }

            if (Wruntisms.Any(x => x.Id == id))
            {
                Wruntisms.First(x => x.Id == id).Message = message;
            }
            else
            {
                Wruntisms.Add(new Wruntism(id, message));
            }
        }

        public string GetWruntismById(int id)
        {
            return (from x in Wruntisms
                where x.Id == id
                select x.Message).First();
        }

        public IEnumerable<string> GetAllWruntisms()
        {
            return from x in Wruntisms
                select $"{x.Id}: {x.Message}";
        }

        public string GetRandomWruntism()
        {
            var random = new Random();

            int max = Wruntisms.Max(x => x.Id) + 1;

            int id;
            do
            {
                id = random.Next(1, max);
            } while (Wruntisms.All(x => x.Id != id));

            return GetWruntismById(id);
        }
    }

    public class Wruntism
    {
        public int Id { get; set; }
        public string Message { get; set; }

        public Wruntism()
        {
        }

        public Wruntism(int id, string message)
        {
            if (id < 0)
            {
                throw new ArgumentException("Id out of range");
            }
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message null or empty");
            }

            Id = id;
            Message = message;
        }

    }
}
