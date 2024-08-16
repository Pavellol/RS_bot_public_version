using RS_bot.Subcommittees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RS_bot.Stickers
{
    internal class StickersCreate
    {
             
        private int stickerIndex;
        public string IdStickers { get; set; }

        public static List<string> stickersId = new List<string>()
        {
            //ok 0
            "CAACAgIAAxkBAAELzBVmAtHwEgAB7yKaBePX893xx5L-PZwAAskKAALtzPFJEAi9sru6cRc0BA",
            //hi 1
            "CAACAgIAAxkBAAELzBlmAtMdIaS1aamxQz7-MMS4YWaCCQACig4AAtEh8EkHpmbhWHEjLjQE", 
            //Vo 2
            "CAACAgIAAxkBAAELzBtmAtM5nzykNFvsHO28aNHnUgexdwACiAsAAoCB8EmzCVuNQkIAAUk0BA",
            //like 3
            "CAACAgIAAxkBAAELzYVmBAWyFN7j6FmK7ldqA4eFOYe1YQACww0AApNB8UlLxkxsDXE6sDQE",
            //think 4
            "CAACAgIAAxkBAAELzYdmBAXCfXURdG-FpDXV26wHTqRAzwACQgsAAr8d8En10koHIoNIwDQE",
            //Cool 5
            "CAACAgIAAxkBAAELzcRmBCRwfkX0VaNAAXiPkDogjQO6LwACOhEAAmc76EmBbrMsFpkeiDQE",
            //SuperMan 6
            "CAACAgIAAxkBAAEL1pZmDU_hWr6jAnU6yCbuzTsnRWjYDAACNBAAAktH8UnAxI9qmR8SIDQE",
            // Нормально делай, нормально будет 7
            "CAACAgIAAxkBAAEL16RmDeNENCe2KojmyVAmo0vsRjT1vwAC_gwAAp-08UmQJ_rEyNFlAjQE",
            //love 8 
            "CAACAgIAAxkBAAEMVb1mcYU_VEi-YBvMVXqxYDVEScwbQAAC1gsAAv_z8EnmAAEEOmtEBg41BA", 
            //bleat
            "CAACAgIAAxkBAAEMVxdmcuoODBLgBPUEUioAAVVRQ7cVpOUAAvYMAAJZYfBJF57KYhh6Pco1BA"
        };

        public StickersCreate(int index)
        {
            stickerIndex = index;
        }

        public async Task StickersSend(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendStickerAsync(
             chatId: message.Chat.Id,
             sticker: InputFile.FromFileId(stickersId[stickerIndex]));
        }
        public async Task StickersSend1(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendStickerAsync(
             chatId: message.Chat.Id,
             sticker: InputFile.FromFileId(stickersId[7]));
        }
        public async Task StickersSend8(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendStickerAsync(
             chatId: message.Chat.Id,
             sticker: InputFile.FromFileId(stickersId[8]));
        }
    }
}
