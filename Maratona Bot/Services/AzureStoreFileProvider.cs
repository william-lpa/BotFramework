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

        public async Task StoreFile(ByteArrayContent image)
        {
            using (var httpClient = new HttpClient())
            {
                image.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                await httpClient.PostAsync(EndPoint, image);
            }
        }
    }
}