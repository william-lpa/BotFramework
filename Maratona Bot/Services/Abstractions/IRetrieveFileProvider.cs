using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maratona_Bot.Services.Abstractions
{
    public interface IRetrieveFileProvider
    {
        Task<IEnumerable<byte[]>> RetrieveFiles(string albumName);
    }
}
