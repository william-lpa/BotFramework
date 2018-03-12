using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Maratona_Bot.Services
{
    public class StreamFileContent : ReadCustonVisionContent, IExtractHttpContent
    {
        public StreamFileContent(string url) : base(url) { }
        protected override string CustomVisionUri => ConfigurationManager.AppSettings["CustomVisionAttachment"];

        public async Task<byte[]> Data()
        {
            var responseMessage = await new HttpClient().GetAsync(url);
            return await responseMessage.Content.ReadAsByteArrayAsync();
        }

        public async Task<HttpContent> GetContentData()
        {
            var data = await Data();
            var content = new ByteArrayContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return content;
        }
    }
}