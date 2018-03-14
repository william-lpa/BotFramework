using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Maratona_Bot.Dialogs;
using Maratona_Bot.Model;
using Newtonsoft.Json;

namespace Maratona_Bot.Services
{
    public abstract class ReadCustonVisionContent
    {
        protected readonly string url;
        private string stringResponse;
        public ReadCustonVisionContent(string url) => this.url = url;

        private string CustomApiKey => ConfigurationManager.AppSettings["CustomVisionKey"];
        protected abstract string CustomVisionUri { get; }

        public async Task PostImage(HttpContent content)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-key", CustomApiKey);
            var response = await client.PostAsync(CustomVisionUri, content).ConfigureAwait(false);
            stringResponse = await response.Content.ReadAsStringAsync();
        }

        public async Task<IEnumerable<Prediction>> RetrievePredicitons()
        {
            return await Task.FromResult(JsonConvert.DeserializeObject<CustomVisionResult>(stringResponse).Predictions.OrderByDescending(c => c.Probability));
        }
    }
}