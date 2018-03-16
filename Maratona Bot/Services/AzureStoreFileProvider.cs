using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Maratona_Bot.Services.Abstractions;

namespace Maratona_Bot.Services
{
    public class AzureStoreFileProvider : IStoreFileProvider
    {
        public string EndPoint => "http://localhost:1337/files";

        public async Task StoreFile(ByteArrayContent image, string album, string user)
        {
            using (var httpClient = new HttpClient())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    var data = await image.ReadAsStreamAsync();
                    //image.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    data.CopyTo(ms);

                    await httpClient.PostAsync(EndPoint, new StringContent(Convert.ToBase64String(ms.ToArray())));

                }
            }
        }
    }
}