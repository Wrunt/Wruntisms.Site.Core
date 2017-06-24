using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wruntisms.Site.Core.Controllers
{
    using Microsoft.Extensions.Options;
    using Models;

    [Produces("application/json")]
    [Route("api/Wruntisms")]
    public class WruntismsController : Controller
    {
        private readonly AppSettings settings;

        public WruntismsController(IOptions<AppSettings> settings)
        {
            this.settings = settings.Value;
        }

        // GET: api/Wruntisms
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var model = GetModel();

            return model.GetAllWruntisms();
        }

        // GET: api/Wruntisms/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            var model = GetModel();

            return model.GetWruntismById(id);
        }

        [HttpGet, Route("Random")]
        public string GetRandom()
        {
            var model = GetModel();

            return model.GetRandomWruntism();
        }
        
        // POST: api/Wruntisms
        [HttpPost]
        public void Post([FromBody]string value)
        {
            var model = GetModel();
            model.FixIds();
            model.AddOrUpdateWruntism(value);
            model.SaveToFile();
        }
        
        // PUT: api/Wruntisms/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            var model = GetModel();
            model.AddOrUpdateWruntism(value, id);
            model.SaveToFile();
        }

        [HttpPut, Route("FixIds")]
        public void PutFixIds()
        {
            var model = GetModel();
            model.FixIds();
            model.SaveToFile();
        }
        
        // DELETE: api/Wruntisms/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var model = GetModel();
            model.RemoveWruntismById(id);
            model.SaveToFile();
        }

        private WruntismsModel GetModel()
        {
            var model = new WruntismsModel(settings);
            model.LoadFromFile();
            return model;
        }
    }
}
