using DesafioPG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DesafioPG.Controllers
{
    public class PersonagensController : ApiController
    {

        private static List<Heros> Personagens = new List<Heros>();

        // GET api/values/5
        public List<Heros> Get()
        {
            var model = new Heros();

            var name = "personagensmarvel.txt";

            var caminho = System.AppDomain.CurrentDomain.BaseDirectory.ToString();           

            var FullPath =  Path.Combine(caminho, name);

            var jsonString = File.ReadAllText(FullPath);

            model = JsonConvert.DeserializeObject<Heros>(jsonString);

            Personagens.Add(model);

            return Personagens;
        }

       
    }
}
