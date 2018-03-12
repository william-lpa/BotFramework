using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Maratona_Bot.Model;
using Maratona_Bot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

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

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            await context.PostAsync($"Vou determinar para qual álbum, esta foto deve ser armazenada, só um momento...");
            await ClassificarImagem(context);

        }

        public async Task ClassificarImagem(IDialogContext context)
        {
            context.Wait((c, a) => ProcessarImagemAsync(c, a));
        }

        private async Task ProcessarImagemAsync(IDialogContext contexto, IAwaitable<IMessageActivity> argument)

        {
            var activity = await argument;
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
                content = new UriFileContent();
            }
            try
            {
                var customVision = new CustomVision(content,null);
                var reply = await customVision.ProcessImage();

                await contexto.PostAsync(reply);
            }
            catch (Exception)
            {
                await contexto.PostAsync("Ops! Deu algo errado na hora de analisar sua imagem!");
            }

            contexto.Wait(MessageReceivedAsync);
        }

    }
}