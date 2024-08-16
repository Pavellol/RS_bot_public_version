using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using RS_bot.Stickers;

namespace RS_bot
{
    internal class SeminarRequest
    {
        internal async Task SeminarRequestEtFirst(ITelegramBotClient botClient, Message message, DataButtons button)
        {                       
            await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите интересующий вас семинар:", replyMarkup: button.CreateButtonDoubleWhithEndButtonMarkup(button.SeminarsName, DataButtons.reverseMenu[0]));

            StickersCreate sticker = new StickersCreate(8);
            sticker.StickersSend8(botClient, message);
        }
        internal async Task SeminarRequestEtSecond(ITelegramBotClient botClient, Message message, DataButtons button)
        {
            List<string> list1 = ExcelRead.ReaadExcelAndSearchNamesAllGroupInSPb();
            List<string> list2 = ExcelRead.ReaadExcelAndSearchNamesAllGroupInLO();

            for (int i = 0; i < list1.Count; i++)
                list1[i] += " Семинар";            

            for (int i = 0; i < list2.Count; i++)
                list2[i] += " Семинар";

            var list = list1.Union(list2);
            
            await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите группу:", replyMarkup: button.CreateButtonDoubleButtonMarkup(list));
        }
    }
}
