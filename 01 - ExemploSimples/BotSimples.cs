using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace ExemploSimples
{
    public class BotSimples : IBot
    {

        private static readonly MemoryStorage _myStorage = new MemoryStorage();

        /// <summary>
        /// Every Conversation turn for our Bot will call this method. In here
        /// the bot checks the Activty type to verify it's a message, bumps the 
        /// turn conversation 'Turn' count, and then echoes the users typing
        /// back to them. 
        /// </summary>
        /// <param name="context">Turn scoped context containing all the data needed
        /// for processing this conversation turn. </param>        
        public async Task OnTurnAsync(ITurnContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            // O bot só irá tratar mensagens.
            if (context.Activity.Type == ActivityTypes.Message)
            {
                // Obtenho o estado da conversação.
                BotSimplesState estado = _myStorage.ReadAsync<BotSimplesState>(new string[] { "BotSimplesState" }).Result?.FirstOrDefault().Value;

                if (estado is null || !estado.PergunteiNome)
                {
                    estado = new BotSimplesState();
                    estado.PergunteiNome = true;
                    try
                    {
                        await _myStorage.WriteAsync(new Dictionary<string, object>(
                            new List<KeyValuePair<string, object>>()
                            {
                                new KeyValuePair<string, object>("BotSimplesState", estado)
                            }), cancellationToken);
                    }
                    catch
                    {
                        await context.SendActivityAsync("Erro ao armazenar.");
                    }
                    await context.SendActivityAsync("Qual o seu nome ?");
                }
                else
                {
                    var nome = context.Activity.Text;

                    if (string.IsNullOrEmpty(nome))
                    {
                        await context.SendActivityAsync("Desculpe, pode repetir ?");
                    }
                    else
                    {
                        estado.PergunteiNome = false;
                        try
                        {
                            await _myStorage.WriteAsync(new Dictionary<string, object>(
                                new List<KeyValuePair<string, object>>()
                                {
                                new KeyValuePair<string, object>("BotSimplesState", estado)
                                }), cancellationToken);
                        }
                        catch
                        {
                            await context.SendActivityAsync("Erro ao armazenar.");
                        }
                        await context.SendActivityAsync($"Oi {nome}, seja bem vindo ao nosso chat de teste.");
                    }
                }

            }
        }
    }
}
