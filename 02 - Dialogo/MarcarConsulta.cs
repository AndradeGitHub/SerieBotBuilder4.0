using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts;
using System;
using System.Linq;
using Microsoft.Recognizers.Text;
using System.Collections.Generic;

namespace ExemploDialogo
{
    public class MarcarConsulta : IBot
    {

        DialogSet dialogos;
        string nomeCliente;
        string convenio;
        DateTime dataHora;


        public MarcarConsulta()
        {
            dialogos = new DialogSet();

            dialogos.Add("marcarConsulta", new WaterfallStep[] {
                async (dc, args, next) =>
                {
                    await dc.Prompt("texto","Qual o seu nome ?");
                },
                async (dc, args, next) =>
                {
                     nomeCliente = ((TextResult)args).Value;
                    await dc.Prompt("texto",$"{nomeCliente}, qual o convênio ?");
                },
                async (dc, args, next) =>
                {
                     convenio = ((TextResult)args).Value;
                    await dc.Prompt("dataHora","Certo... Qual o dia e horário ?");
                },
                async (dc, args, next) =>
                {
                    var resultado = ((DateTimeResult)args).Resolution.First();
                    dataHora = Convert.ToDateTime(resultado.Value);

                    await dc.Context.SendActivity($"Está marcado. Dia {dataHora.ToString("dd/MM/yyyy HH:mm:ss")}");
                    await dc.End();
                },
            });

            dialogos.Add("dataHora", new Microsoft.Bot.Builder.Dialogs.DateTimePrompt(Culture.Portuguese));
            dialogos.Add("texto", new Microsoft.Bot.Builder.Dialogs.TextPrompt());
        }

        public async Task OnTurn(ITurnContext context)
        {

            var state = ConversationState<Dictionary<string, object>>.Get(context);
            var dc = dialogos.CreateContext(context, state);
            await dc.Continue();

            if (context.Activity.Type == ActivityTypes.Message)
            {
                if (!context.Responded)
                {
                    await dc.Begin("marcarConsulta");
                }
            }
        }
    }
}
