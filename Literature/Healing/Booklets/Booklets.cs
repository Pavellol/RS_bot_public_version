using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot;

using System.Net;
using Microsoft.VisualBasic;
using Telegram.Bot.Types.Enums;
using System.IO;

namespace RS_bot.Literature.Healing.Booklets
{
    internal class Booklets
    {
        private static List<string> routesBooklets = new List<string>
        {
            "Белый буклет.pdf",
            "Буклет для групп.pdf",
            "В неволе.pdf",
            "Двенадцать концепций обслуживания АН.pdf",
            "Ознакомительное руководство в АН.pdf",
            "Работа над 4-ым шагом АН.pdf"
        };

        public List<string> RoutesBooklets { get { return routesBooklets; } }

        internal async Task SendDocumentBooklet(ITelegramBotClient botClient, Message message)
        {
            for (int i = 0; i < routesBooklets.Count; i++)
            {
                try
                {
                    await using Stream stream = System.IO.File.OpenRead(routesBooklets[i]);
                    await botClient.SendDocumentAsync(
                        chatId: message.Chat.Id,
                        document: InputFile.FromStream(stream: stream, fileName: routesBooklets[i]));
                }
                catch (Exception e)
                {
                    await Console.Out.WriteLineAsync(e.Message.ToString());
                }
            }            
        }
    }
}
