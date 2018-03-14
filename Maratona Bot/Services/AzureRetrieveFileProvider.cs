using Maratona_Bot.Services.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Maratona_Bot.Services
{
    public class AzureRetrieveFileProvider:IRetrieveFileProvider
    {
        public string EndPoint => "";

        public async Task<IEnumerable<byte[]>> RetrieveFiles(string albumName)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(EndPoint);

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var streamContent = await response.Content.ReadAsStreamAsync();
                    //var arquivos = JsonConvert.DeserializeObject<IEnumerable<byte[]>>(streamContent);
                }
                return null;
            }
        }
    }
}