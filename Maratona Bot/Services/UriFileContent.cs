using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Maratona_Bot.Services
{
    public class UriFileContent : ReadCustonVisionContent, IExtractHttpContent
    {
        public UriFileContent(string url) : base(url)
        {
        }

        protected override string CustomVisionUri => ConfigurationManager.AppSettings["CustomVisionUri"];

        public async Task<byte[]> Data()
        {
            WebClient webClient = new WebClient();
            return await Task.FromResult(webClient.DownloadData(url));
        }

        public async Task<HttpContent> GetContentData()
        {
            var data = Encoding.UTF8.GetBytes("{ 'url': '" + url + "' }");
            var content = new ByteArrayContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await Task.FromResult(content);
        }
    }
}