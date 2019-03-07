using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExemploSimples
{
    /// <summary>
    /// Classe com a estrutura do estado do bot.
    /// </summary>
    public class BotSimplesState : IStoreItem
    {
        public bool PergunteiNome { get; set; }
        public string ETag { get; set; } = "*";
    }
}
