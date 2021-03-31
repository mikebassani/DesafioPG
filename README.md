# Desafio Marvel

Construir uma API utilizando framework .NET ou .NETCORE para acessar a API da MARVEL resgatando informações dos personagens e gravando em um arquivo “personagensmarvel.txt” dentro da raiz do projeto. Exemplo de informações para montar a requisição.

**GET /v1/public/characters?ts=1&apikey=473da253b3977826288936c4a61c0991&hash=8be15a064f155 7728066139e0619aaf6 HTTP/1.1 Host: gateway.marvel.com**

Será retornado um documento no formato JSON e dentro do documento contém uma lista chamada **results**  que será usada para montar o arquivo "personagensmarvel.txt”. 

O arquivo “personagensmarvel.txt” deverá conter os campos: 
**id, name, description, comics, series, stories e events.** 
Os campos **comics, series, stories, events** são listas que contêm items, porem, precisamos apenas do campo **name** de cada item da lista items. 

A formatação do arquivo “personagensmarvel.txt” ficara de responsabilidade do desafiante. 

Diferenciais: Utilização de Swagger


# Resolução do desafio

 Criei uma pasta **services**,  dentro dela eu cirei a classe **hero** com os métodos  responsáveis pelo consumo da Api ( **BaseUrl** e **GetHeros**) 
 
 Na Homecontroller,  chamei o método (GetHeros)

## GetHeros

Para o consumo da API, são necessários os seguintes parâmetros: **ts**, **publicKey**  e  **privateKey**.
 
**ts** = Valor gerado pela data e hora atual { **DateTime.Now.Ticks.ToString();** }
Você obtém a chave publica e  a privada  no site : https://developer.marvel.com

Após obter essas informações deve ser gerado o hash

 **hash** - um resumo md5 do parâmetro ts, sua chave privada e sua chave pública (por exemplo, md5 (ts + privateKey + publicKey)

montei minha uri {string.Format("/v1/public/characters?ts=**{0}**&apikey=**{1}**&hash=**{2}**", **ts, publicKey, hash**);)

Criei a classe **HttpInstance** pois obtive a informação de que fazer o uso do using é uma má pratica e pode gerar exceções!

Fiz a requisição, desserializei  o json mapeando com a model Heros pegando somente às informações necessárias
#### Gerando o arquivo
Serializei o json já mapeado e o guardei em uma variável Arquivo.

Embora o teste não tenha exigido que o arquivo fosse formatado, eu formatei no tipo json para ficar bonito
e salvei na raiz do meu projeto com o nome proposto.




# RETORNAR TODOS OS PERSONAGENS
Me informaram que eu estava retornando somente 20 registros em ordem alfabética dos personagens.

Após pesquisar na documentação verifiquei a existência de dois parâmetros opcionais :
**offset** (A API da Marvel diz “Ignore o número especificado de recursos no conjunto de resultados”. o que ele faz “Pega a partir do numero especificado, se você colocar 100 ele pegará a partir do centésimo resultado”).

**limit** (Limita o conjunto de resultados ao número especificado de recursos.{Limite máximo permitido = 100})

**Caso queira retornar todos os personagens ou "Paginar" e limitar os resultados, basta modificar o código abaixo**
NA PASTA SERVICES, NA CLASSE HERO  ALTERE O MÉTODO **GetHeros** PELO CODIGO ABAIXO


`````
 public List<Heros> GetHeros()
        {

            #region"Parametros"
            var ts = DateTime.Now.Ticks.ToString();
            var publicKey = "da4460fd1429f2d2863f73ace050aa1d";
            var privateKey = "0bdfdc41351fb0cd6152ccc9f4588783ef34a465";
            var hash = GerarHash(ts, privateKey, publicKey);
            var offset = 0;
            var limit = 100;
            #endregion

            try
            {
                List<object> listaObject = new List<object>();
               
                for (var i = 1; i <= 15; i++)
                {

                    var model = new Heros();
                    var modelResults = new List<Heros.Result>();

                    var action = string.Format("/v1/public/characters?ts={0}&apikey={1}&hash={2}&limit={3}&offset={4}", ts, publicKey, hash, limit, offset);

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + action);

                    var response = HttpInstance.GetHttpClientInstance().GetAsync(request.RequestUri.AbsoluteUri).Result;

                    var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    model = JsonConvert.DeserializeObject<Heros>(jsonString);
                    modelResults = model.data.results;

                    listaObject.AddRange(modelResults.ToList());

                    offset = limit * i;

                }



                #region"Gerar arquivo"

                var arquivo = JsonConvert.SerializeObject(listaObject);
                var caminho = System.AppDomain.CurrentDomain.BaseDirectory.ToString();

                var JsonFormatted = JValue.Parse(arquivo).ToString();
                System.IO.File.WriteAllText(@"" + caminho + "personagensmarvel.txt", JsonFormatted);


                #endregion


                return null;
            }
            catch (Exception)
            {
                return null;
            }


        }
`````

**PARA RETORNAR TODOS OS ARQUIVOS MODIFICADOS QUANDO FOR CONSUMIR A API CRIADA**

No PersonagensController Altere o método **GET** pelo codigo abaixo!

```
public List<object> Get()
        {
           var model = new List<object>();

            var name = "personagensmarvel.txt";

            var caminho = System.AppDomain.CurrentDomain.BaseDirectory.ToString();

            var FullPath = Path.Combine(caminho, name);

            var jsonString = File.ReadAllText(FullPath);

            model = JsonConvert.DeserializeObject<List<object>>(jsonString);            

            return model;
        }
```




## Diferenciais 

Criei um método **GET**  que lê e desserializa o arquivo "personagensmarvel.txt”. retornado uma lista

Usei o Swgger para documentar a Api


## End

### Tela (Index)

![Capturar](https://user-images.githubusercontent.com/47748537/110571587-7ec52b00-8136-11eb-9d05-ac170691a37e.JPG)


### Swagger

![1](https://user-images.githubusercontent.com/47748537/110571702-b502aa80-8136-11eb-86bd-91b7b3155983.JPG)

