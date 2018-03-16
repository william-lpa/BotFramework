using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Maratona_Bot.Services;
using Maratona_Bot.Services.Abstractions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Maratona_Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private IStoreFileProvider StorageProvider => new AzureStoreFileProvider();
        private IRetrieveFileProvider RetrievalProvider => new AzureRetrieveFileProvider();

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text?.Contains("get") ?? false)
            {
                await context.PostAsync($"irei recuperar as imagens! Só um momento por favor...");
                await RecuperarImagensAlbuns(context);
            }
            await context.PostAsync($"Vou determinar para qual álbum esta foto deve ser armazenada, só um momento...");
            await ClassificarImagem(context);

        }

        public async Task ClassificarImagem(IDialogContext context)
        {
            //context.Wait((c, a) => ProcessarImagemAsync(c, a));
            await ProcessarImagemAsync(context);
        }

        private async Task ProcessarImagemAsync(IDialogContext contexto) //IAwaitable<IMessageActivity> argument

        {
            var activity = contexto.Activity as Activity;
            var hasAttachment = activity.Attachments?.Any() == true;
            var uri = string.Empty;
            IExtractHttpContent content = null;

            if (hasAttachment)
            {
                uri = activity.Attachments[0].ContentUrl;
                content = new StreamFileContent(uri);
            }
            else
            {
                uri = activity.Text;
                content = new UriFileContent(uri);
            }
            try
            {
                var customVision = new CustomVision(content, StorageProvider, RetrievalProvider);
                var reply = await customVision.ProcessImage();
                await customVision.StoreImage();

                await contexto.PostAsync(reply);
            }
            catch (Exception)
            {
                await contexto.PostAsync("Ops! Deu algo errado na hora de analisar sua imagem!");
            }

            contexto.Wait(MessageReceivedAsync);
        }

        private async Task RecuperarImagensAlbuns(IDialogContext contexto)

        {
            var activity = contexto.Activity as Activity;

            try
            {
                var customVision = new CustomVision(null, StorageProvider, RetrievalProvider);
                var reply = await customVision.RetrieveImages("teste2");
                System.IO.File.WriteAllBytes(@"C:\Worskpace\ddd.jpg", reply.Files.First().Data.Data);
                //a wait contexto.PostAsync(reply);
            }
            catch (Exception err)
            {
                await contexto.PostAsync("Ops! Deu algo errado na hora de analisar sua imagem!");
            }

            contexto.Wait(MessageReceivedAsync);
        }

    }
}