using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Maratona_Bot.Model;
using Maratona_Bot.Services;
using Maratona_Bot.Services.Abstractions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Maratona_Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private static CustomVision customVision;
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

            if (activity.Text?.Contains("Obter fotos") ?? false)
            {
                await context.PostAsync($"Irei recuperar as imagens! Para isso, por favor me diga o nome do álbum de fotos");
                context.Wait((c, a) => RecuperarImagensAlbuns(c));

            }
            else
            {
                await context.PostAsync($"Vou determinar para qual álbum esta foto deve ser armazenada, só um momento...");
                await ClassificarImagem(context);
            }
        }

        public async Task ClassificarImagem(IDialogContext context)
        {
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
                customVision = new CustomVision(content, StorageProvider, RetrievalProvider);
                var reply = await customVision.ProcessImage();
                await contexto.PostAsync(reply);
                contexto.Wait((c, a) => customVision.StoreImage());
            }
            catch (Exception)
            {
                await contexto.PostAsync("Ops! Deu algo errado na hora de analisar sua imagem!");
            }
        }

        private async Task RecuperarImagensAlbuns(IDialogContext contexto)
        {
            var activity = contexto.Activity as Activity;

            try
            {
                var customVision = new CustomVision(null, StorageProvider, RetrievalProvider);
                var reply = await customVision.RetrieveImagesAsync(activity.Text ?? "Sem Nome");
                var filesCount = reply.Files.Count();

                await contexto.PostAsync(filesCount > 0 ? $"Obaaa, encontrei {reply.Files.Count()} fotos no album." : "Infelizmente eu não encontrei nenhuma foto no álbum :(");

                if (filesCount == 0)
                {
                    contexto.Wait(MessageReceivedAsync);
                    return;
                }

                var message = AttachFiles(activity, reply);
                await contexto.PostAsync(message);
            }
            catch (Exception err)
            {
                await contexto.PostAsync("Ops! Deu algo errado na hora de analisar sua imagem!");
            }

            contexto.Wait(MessageReceivedAsync);
        }

        private Activity AttachFiles(Activity activity, FilesRetrieved reply)
        {
            var message = activity.CreateReply();

            var heroCard = new HeroCard("Álbum de fotos", reply.Files.First().AlbumName);
            message.Attachments.Add(heroCard.ToAttachment());
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            foreach (var file in reply.Files)
            {
                message.Attachments.Add(new Attachment("image/jpeg", $"data:image/jpeg;base64,{Convert.ToBase64String(file.Data.Data)}"));
            }
            return message;
        }
    }
}