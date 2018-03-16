using Maratona_Bot.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maratona_Bot.Services.Abstractions
{
    public interface IRetrieveFileProvider
    {
        Task<FilesRetrieved> RetrieveFiles(string albumName);
    }
}
