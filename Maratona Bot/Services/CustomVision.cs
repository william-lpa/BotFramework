using Maratona_Bot.Services.Abstractions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Maratona_Bot.Services
{
    public class CustomVision
    {
        private readonly IExtractHttpContent extractFileHttp;
        private readonly ReadCustonVisionContent readCustonVision;
        private readonly IStoreFileProvider storeProvider;
        private readonly IRetrieveFileProvider retrieveProvider;

        public CustomVision(IExtractHttpContent extract, ReadCustonVisionContent readCustonVision, IStoreFileProvider storeProvider, IRetrieveFileProvider retrieveProvider)
        {
            this.extractFileHttp = extract;
            this.readCustonVision = readCustonVision;
            this.storeProvider = storeProvider;
            this.retrieveProvider = retrieveProvider;
        }

        public async Task<string> ProcessImage()
        {
            if (extractFileHttp == null || readCustonVision == null)
                return null;

            using (var extractFile = await extractFileHttp.GetContentData())
            {
                await readCustonVision.PostImage(extractFile);
            }

            var readPredictions = await readCustonVision.RetrievePredicitons();

            IProcessPredictions processPredictions = new ProcessPredictions();

            return await processPredictions.GetPreparedOutput(readPredictions);
        }

        public async Task StoreImage()
        {
            var image = await extractFileHttp.Data();
            await storeProvider.StoreFile(image);
        }

        public async Task<IEnumerable<byte[]>> RetrieveImages(string albumName)
        {
            return await retrieveProvider.RetrieveFiles(albumName);
        }
    }
}