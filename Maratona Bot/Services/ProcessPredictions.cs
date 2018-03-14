using System.Collections.Generic;
using System.Threading.Tasks;
using Maratona_Bot.Model;
using Maratona_Bot.Services.Abstractions;

namespace Maratona_Bot.Services
{
    public class ProcessPredictions : IProcessPredictions
    {
        public Task<string> GetPreparedOutput(IEnumerable<Prediction> predictions)
        {
            return Task.FromResult("");
        }
    }
}