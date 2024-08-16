using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace RS_bot.Vol_AN
{
    public class Vol_AN
    {
        public  async Task SendMessageAboutVolontInAN(ITelegramBotClient botClient, Message message)
        {
            try
            {
                await botClient.SendStickerAsync(
                    chatId: message.Chat.Id,
                    sticker: InputFile.FromFileId("CAACAgIAAxkBAAEMVb1mcYU_VEi-YBvMVXqxYDVEScwbQAAC1gsAAv_z8EnmAAEEOmtEBg41BA"));

                await using Stream stream = System.IO.File.OpenRead("botVol_AN.png");
                await botClient.SendPhotoAsync(
                    message.Chat.Id,
                    photo: InputFile.FromStream(stream: stream, fileName: "botVol_AN.png"),
                    caption: "<a href=\"https://t.me/vol_an_info_bot\">Инфо бот для волонтеров АН</a>\n\n<b>Расскажет вам какие есть направления волонтерства в сообществе и поможет определиться с направлением</b>",
                    parseMode: ParseMode.Html);
            }
            catch (Exception ex)
            {                
                await botClient.SendStickerAsync(
                    chatId: message.Chat.Id,
                    sticker: InputFile.FromFileId("CAACAgIAAxkBAAEMVxdmcuoODBLgBPUEUioAAVVRQ7cVpOUAAvYMAAJZYfBJF57KYhh6Pco1BA"));
                await botClient.SendTextMessageAsync(message.Chat.Id, "К сожалению произошла ошибка, обратитесь к администратору бота.");
                await Console.Out.WriteLineAsync(ex.Message);
            }
            
        }
    }
}
