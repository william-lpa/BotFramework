using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maratona_Bot.Model;
using Maratona_Bot.Services.Abstractions;

namespace Maratona_Bot.Services
{
    public class ProcessPredictions : IProcessPredictions
    {
        public async Task<string> GetPreparedOutput(IEnumerable<Prediction> predictions)
        {
            var maisProvavel = predictions.OrderByDescending(x => Math.Round(x.Probability)).First();

            return await Task.FromResult($"Tenho {Math.Round(maisProvavel.Probability * 100, 2)}% de certeza que esta imagem se encaixaria no álbum {maisProvavel.Tag}. Posso adicioná-la neste album?");
        }
    }
}