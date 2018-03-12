using Maratona_Bot.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maratona_Bot.Services.Abstractions
{
    public interface IProcessPredictions
    {
        Task<string> GetPreparedOutput(IEnumerable<Prediction> predictions);
    }
}
