using System.Net.Http;
using System.Threading.Tasks;

namespace Maratona_Bot.Services
{
    public interface IExtractHttpContent
    {
        Task<HttpContent> GetContentData();
        Task<byte[]> Data();
    }
}