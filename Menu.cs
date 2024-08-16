using RS_bot.Stickers;
using RS_bot.Subcommittees;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RS_bot
{
    abstract class Menu
    {
        public abstract Task KeyBoardAsync(ITelegramBotClient botClient, Message message, DataButtons buttons, string text);
    }

    class MainMenu : Menu
    {
        StickersCreate sticker = new StickersCreate(1);

        public override async Task KeyBoardAsync(ITelegramBotClient botClient, Message message, DataButtons buttons, string text)
        {
            sticker.StickersSend(botClient, message);
            await botClient.SendTextMessageAsync(message.Chat.Id, "Добро пожаловать!", replyMarkup: buttons.CreateButtonDoubleButtonMarkup(buttons.MainMenu));
        }

        public async Task KeyBoardAsync1(ITelegramBotClient botClient, Message message, DataButtons buttons, string text)
        {
            sticker.StickersSend1(botClient, message);
            await botClient.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: buttons.CreateButtonDoubleButtonMarkup(buttons.MainMenu));
        }
        /*public override async Task StickersSend(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendStickerAsync(
             chatId: message.Chat.Id,
             sticker: InputFile.FromFileId("CAACAgIAAxkBAAECO4Vl6BMQ1PV5CYqSgR42ZrTqNourfQACAQADFm5MEh97vwZE6duLNAQ"));
        }*/
    }

    class SubComiteesMenu : Menu
    {
        StickersCreate sticker = new StickersCreate(5);
        DataButtons buttons1 = new DataButtons();
        public SubComiteesMenu(ITelegramBotClient botClient, Message message, DataButtons buttons, string text)
        {
            KeyBoardAsync(botClient, message, buttons, text);
        }

        public SubComiteesMenu(ITelegramBotClient botClient, Message message, List<string> buttons)
        {
            KeyBoardForSubComiteesAsync(botClient, message, buttons);
        }

        private async Task KeyBoardForSubComiteesAsync(ITelegramBotClient botClient, Message message, List<string> buttons)
        {
            //buttons1.CreateButtonDoubleWithEndButtonMarkup(buttons, /*buttons1.ReverseMenu[1]*/"проверка");
            await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите интересующий вас подкомитет", replyMarkup: buttons1.CreateButtonDoubleWithEndButtonMarkup(buttons, DataButtons.reverseMenu[1]));
        }

        //private async Task KeyBoardForSubComitees1Async(ITelegramBotClient botClient, Message message, DataButtons buttons)
        //{
        //    await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите интересующий вас подкомитет", replyMarkup: buttons.CreateButtonDoubleButtonMarkup(buttons.SubcometeesNaSPb));
        //}

        public override async Task KeyBoardAsync(ITelegramBotClient botClient, Message message, DataButtons buttons, string text)
        {
            sticker.StickersSend(botClient, message);
            await botClient.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: buttons.CreateButtonDoubleWhithEndButtonMarkup(buttons.SubComitees, DataButtons.reverseMenu[0]));
        }
    }
    class ComiteesMenu : Menu
    {
        StickersCreate sticker = new StickersCreate(2);
        public override async Task KeyBoardAsync(ITelegramBotClient botClient, Message message, DataButtons buttons, string text)
        {
            sticker.StickersSend(botClient, message);
            await botClient.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: buttons.CreateButtonDoubleWhithEndButtonMarkup(buttons.Сomitees, DataButtons.reverseMenu[0]));
        }
    }
    class Booklets: Menu
    {
        public override async Task KeyBoardAsync(ITelegramBotClient botClient, Message message, DataButtons buttons, string text)
        {            
            await botClient.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: buttons.CreateButtonDoubleWhithEndButtonMarkup(buttons.Booklets, DataButtons.reverseMenu[2]));
        }
    }
    class SheduleMenu : Menu
    {
        StickersCreate sticker = new StickersCreate(6);
        public override async Task KeyBoardAsync(ITelegramBotClient botClient, Message message, DataButtons buttons, string text)
        {
            sticker.StickersSend(botClient, message);
            await botClient.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: buttons.CreateButtonDoubleWhithEndButtonMarkup(buttons.SheduleMain, DataButtons.reverseMenu[0]));
        }
    }
    class LiteratureMenu : Menu
    {
        StickersCreate sticker = new StickersCreate(4);
        public override async Task KeyBoardAsync(ITelegramBotClient botClient, Message message, DataButtons buttons, string text)
        {
            sticker.StickersSend(botClient, message);
            await botClient.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: buttons.CreateButtonDoubleWhithEndButtonMarkup(buttons.LiteratureMenu, DataButtons.reverseMenu[0]));
        }
    }

    class HelpsLiteratureMenu : Menu
    {
        public override async Task KeyBoardAsync(ITelegramBotClient botClient, Message message, DataButtons buttons, string text)
        {
            await using Stream stream = System.IO.File.OpenRead("HelpLiterature.png");
            await botClient.SendPhotoAsync(
                message.Chat.Id,
                photo: InputFile.FromStream(stream: stream, fileName: "HelpLiterature.png"),
                caption: "Карта по разделу Литература выздоровления🗺");

            await botClient.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: buttons.CreateButtonDoubleWhithEndButtonMarkup(buttons.HealingLiterature, DataButtons.reverseMenu[2]));
        }
        internal async Task SendHelpsLiterature(ITelegramBotClient botClient, Message message, List<string> listNamesLiterature)
        {
            try
            {
                var media = new List<InputMediaDocument>();
                foreach (var fileName in listNamesLiterature)
                {
                    //await using Stream stream = System.IO.File.OpenRead(fileName);
                    var inputMedia = new InputMediaDocument(new InputFileStream(System.IO.File.OpenRead(fileName), fileName));
                    media.Add(inputMedia);
                }

                await botClient.SendMediaGroupAsync(message.Chat.Id, media);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message.ToString());
            }
        }
        
        internal async Task SendHelpsLiteraturePhoto(ITelegramBotClient botClient, Message message, List<string> listNamesPhotos)
        {
            try
            {                
                var media = new List<InputMediaPhoto>();

                foreach (var item in listNamesPhotos)
                {
                    media.Add(new InputMediaPhoto(InputFile.FromUri(item)));
                }

                await botClient.SendMediaGroupAsync(
                    chatId: message.Chat.Id,
                    media);               
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message.ToString());
            }
        }            
    }

    class ServiceLiteratureMenu : Menu
    {
        public override async Task KeyBoardAsync(ITelegramBotClient botClient, Message message, DataButtons buttons, string text)
        {
            await using Stream stream = System.IO.File.OpenRead("SlugLiterature.png");
            await botClient.SendPhotoAsync(
                message.Chat.Id,
                photo: InputFile.FromStream(stream: stream, fileName: "SlugLiterature.png"),
                caption: "Карта по разделу Служебная литература 🗺");


            await botClient.SendTextMessageAsync(message.Chat.Id, text, replyMarkup: buttons.CreateButtonDoubleWhithEndButtonMarkup(buttons.ServiceLiterature, DataButtons.reverseMenu[2]));
        }

        internal async Task SendServiceLiterature(ITelegramBotClient botClient, Message message, List<string> listNamesLiterature)
        {
            try
            {
                var media = new List<InputMediaDocument>();
                foreach (var fileName in listNamesLiterature)
                {
                    //await using Stream stream = System.IO.File.OpenRead(fileName);
                    var inputMedia = new InputMediaDocument(new InputFileStream(System.IO.File.OpenRead(fileName), fileName));
                    media.Add(inputMedia);
                }

                await botClient.SendMediaGroupAsync(message.Chat.Id, media);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message.ToString());
            }
        }       
    }    
}
