using RS_bot.Stickers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace RS_bot
{
    internal class DesantRequest
    {        
        internal async Task DesantRequestFunc(ITelegramBotClient botClient, Message message, DataButtons button)
        {
            List<string> list1 = ExcelRead.ReaadExcelAndSearchNamesAllGroupInSPb();
            List<string> list2 = ExcelRead.ReaadExcelAndSearchNamesAllGroupInLO();

            for (int i = 0; i < list1.Count; i++)
                list1[i] += " Десант";

            for (int i = 0; i < list2.Count; i++)
                list2[i] += " Десант";

            var list = list1.Union(list2);

            await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите группу:", replyMarkup: button.CreateButtonDoubleButtonMarkup(list));
        }
    }
}
