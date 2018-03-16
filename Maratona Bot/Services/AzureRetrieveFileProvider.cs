using Maratona_Bot.Model;
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
    public class AzureRetrieveFileProvider : IRetrieveFileProvider
    {
        public string EndPoint => "http://localhost:1337/files?albumNames=";

        public async Task<FilesRetrieved> RetrieveFiles(string albumName)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(string.Concat(EndPoint, albumName));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var streamContent = await response.Content.ReadAsStringAsync();
                    var files = JsonConvert.DeserializeObject<FilesRetrieved>(streamContent);
                    return files;
                }
                return null;
            }
        }
    }
}