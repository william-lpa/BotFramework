using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Maratona_Bot.Services.Abstractions;
using Newtonsoft.Json;

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
                    data.CopyTo(ms);
                    var file64 = Convert.ToBase64String(ms.ToArray());
                    var json = JsonConvert.SerializeObject(new { File = file64, AlbumName = album, User = user });
                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                    await httpClient.PostAsync(EndPoint, stringContent);
                }
            }
        }
    }
}