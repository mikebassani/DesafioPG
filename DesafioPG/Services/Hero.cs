using DesafioPG.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DesafioPG.Services
{
    public class Hero : IApi
    {
        public string BaseUrl
        {
            get
            {
                return "http://gateway.marvel.com";
            }
        }



        public List<Heros> GetHeros()
        {

            #region"Parametros"
            var ts          = DateTime.Now.Ticks.ToString();
            var publicKey   = "da4460fd1429f2d2863f73ace050aa1d";
            var privateKey  = "0bdfdc41351fb0cd6152ccc9f4588783ef34a465";
            var hash        = GerarHash(ts, privateKey, publicKey);
            #endregion

            try
            {
                List<Heros> listaObject = new List<Heros>();

                var model = new Heros();

                var action = string.Format("/v1/public/characters?ts={0}&apikey={1}&hash={2}", ts, publicKey, hash);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + action);

                var response = HttpInstance.GetHttpClientInstance().GetAsync(request.RequestUri.AbsoluteUri).Result;

                var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                model = JsonConvert.DeserializeObject<Heros>(jsonString);                              

                listaObject.Add(model);

                #region"Gerar arquivo"

                var arquivo = JsonConvert.SerializeObject(model);
                var caminho = System.AppDomain.CurrentDomain.BaseDirectory.ToString();               

                var JsonFormatted = JValue.Parse(arquivo).ToString();
                System.IO.File.WriteAllText(@"" + caminho + "personagensmarvel.txt", JsonFormatted);


                #endregion



                return listaObject;
            }
            catch (Exception)
            {
                return null;
            }

            
        }


        private string GerarHash(
          string ts, string privateKey, string publicKey)
        {
            byte[] bytes =
                Encoding.UTF8.GetBytes(ts + privateKey + publicKey);
            var gerador = MD5.Create();
            byte[] bytesHash = gerador.ComputeHash(bytes);
            return BitConverter.ToString(bytesHash)
                .ToLower().Replace("-", String.Empty);
        }


    }

}