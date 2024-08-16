using RS_bot;
using RS_bot.Daily;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using OfficeOpenXml;
using RS_bot.Vol_AN;
using System;
using static System.Net.WebRequestMethods;
using System.IO;
using System.Linq;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Укажите нужный контекст лицензии

//var botClient = new TelegramBotClient("xxx");

//основной бот
var botClient = new TelegramBotClient("xxx");

using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    
    if (update.Type == UpdateType.Message && update?.Message?.Text != null)
    {
        await HandleMessage(botClient, update.Message, update);
        return;
    }
    /*else if (update.Type == UpdateType.CallbackQuery && update?.Message?.Text != null)
    {
        await HandleCallbackQuery(botClient, update.Message,update.Message,);
        return;
    }*/
}
    
    async Task HandleMessage(ITelegramBotClient botClient, Message message, Update update)
    {        
        var chatId = message.Chat.Id;
        DataButtons buttonsText = new DataButtons();
                 
        string necessaryIdChat = "-1001743464202";
        int necessaryIdTopic = 1015;   
    
         Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}.");  

        //стартовая точка
        if (message.Text == "/start" /*|| buttonsText.ReverseMenu.IndexOf(message.Text) == 0*/)
        {
            DataButtons buttons = new DataButtons();
            MainMenu mainMenu = new MainMenu();
            await mainMenu.KeyBoardAsync(botClient, message, buttons, "Добро пожаловать!");
            buttonsText.UserRequestList.Clear();
        }
        //ТРЕНИНГ Ступень 1, человек выбирает группу, куда запрашивать тренинг
        if (buttonsText.TreningsName.Contains(message.Text))
        {
            TreningRequest treningRequest = new TreningRequest();
            treningRequest.TreningRequestEtSecond(botClient, message, buttonsText);
            
            buttonsText.AddUserRequestForUs(message.Text);
        }
        //ТРЕНИНГ Ступень 2, человек выбирает дату, семинар
        if (message.Text.Contains(" Тренинг"))
        {
            buttonsText.AddUserRequestForUs(message.Text);

            await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите дату:", replyMarkup: buttonsText.CreateButtonDoubleButtonMarkup(buttonsText.WorkDateArray));
        }
        if (message.Text.Contains(" Десант"))
        {
            buttonsText.AddUserRequestForUs(message.Text);

            await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите дату:", replyMarkup: buttonsText.CreateButtonDoubleButtonMarkup(buttonsText.WorkDateArray));
        }
        //CЕМИНАР Ступень 1, человек выбирает группу, куда запрашивать семинар
        if (buttonsText.SeminarsName.Contains(message.Text))
        {
            SeminarRequest seminarRequest = new SeminarRequest();
            seminarRequest.SeminarRequestEtSecond(botClient, message, buttonsText);
            
            buttonsText.AddUserRequestForUs(message.Text);
        }
        //CЕМИНАР Ступень 2, человек выбирает дату, семинар
        if (message.Text.Contains(" Семинар"))
        {
            buttonsText.AddUserRequestForUs(message.Text);

            await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите дату:", replyMarkup: buttonsText.CreateButtonDoubleButtonMarkup(buttonsText.WorkDateArray));
        }
        //CЕМИНАР Ступень 3, человек выбирает время для семиара
        if (buttonsText.WorkDateArray.Contains(message.Text))
        {
            buttonsText.AddUserRequestForUs(message.Text);
            await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите дату:", replyMarkup: buttonsText.CreateButtonDoubleButtonMarkup(buttonsText.TimeNamesRequest));
        }
        //CЕМИНАР Ступень 4, отправка в чат листа
        if (buttonsText.TimeNamesRequest.Contains(message.Text))
        {
            buttonsText.AddUserRequestForUs(message.Text);
            MainMenu mainMenu = new MainMenu();
            string userRequest = "Получен запрос: \n";
            for (int i = 0; i < buttonsText.UserRequestList.Count; i++)
            {
                userRequest += buttonsText.UserRequestList[i] + "\n";
            }
            
            await botClient.SendTextMessageAsync(
                chatId: necessaryIdChat,
                text: userRequest + "Запрос от пользователя: " + message.From,
                messageThreadId: necessaryIdTopic);

            buttonsText.UserRequestList.Clear();
                
            await mainMenu.KeyBoardAsync1(botClient, message, buttonsText, "Ваш запрос успешно отправлен, вскором времени с вами свяжутся служащие РС");            
        }
        
        if (message.Text.StartsWith("/") == true)
        {
            if(message.Text == "/join_chanel_telegram")
            {                
                await using Stream stream = System.IO.File.OpenRead("ImageForNaSPbRassilki.jpeg");

                await botClient.SendPhotoAsync(
                    message.Chat.Id,
                    photo: InputFile.FromStream(stream: stream, fileName: "ImageForNaSPbRassilki.jpeg"),
                    caption: "Рассылка АН СПб в <a href=\"https://t.me/naspb12\">Telegram</a>",
                    parseMode: ParseMode.Html);
            }
            else if (message.Text == "/join_site")
            {               
                await using Stream stream = System.IO.File.OpenRead("LogoForSite.jpeg");
                await botClient.SendPhotoAsync(
                    message.Chat.Id,
                    photo: InputFile.FromStream(stream: stream, fileName: "LogoForSite.jpeg"),
                    caption: " <a href=\"https://na-spb.ru/\">Сайт</a> Анонимных наркоманов Санкт-Петербурга",
                    parseMode: ParseMode.Html);                           
            }
            else if (message.Text == "/join_chanel_whats_app")
            {
                await using Stream stream = System.IO.File.OpenRead("AnonsWhatsApp.jpg");
                await botClient.SendPhotoAsync(
                    message.Chat.Id,
                    photo: InputFile.FromStream(stream: stream, fileName: "AnonsWhatsApp.jpg"),
                    caption: "Рассылка АН СПб в <a href=\"https://chat.whatsapp.com/7As29C2YDe2CVgn2esrUvv\">WhatsApp</a> ",
                    parseMode: ParseMode.Html);
            }            
            else if (message.Text == "/seminar")
            {
                SeminarRequest seminarRequest = new SeminarRequest();                                
                seminarRequest.SeminarRequestEtFirst(botClient, message, buttonsText);          
            }
            else if(message.Text == "/trening")
            {
                TreningRequest treningRequest = new TreningRequest();
                treningRequest.TreningRequestEtFirst(botClient, message, buttonsText);
            }            
            else if (message.Text == "/desant")
            {
                DesantRequest desantRequest = new DesantRequest();
                desantRequest.DesantRequestFunc(botClient, message, buttonsText);
            }            
        }        

        if (buttonsText.ReverseMenu.IndexOf(message.Text) == 0)
        {
            DataButtons buttons = new DataButtons();
            MainMenu mainMenu = new MainMenu();

            await mainMenu.KeyBoardAsync1(botClient, message, buttons, "Вы в главном меню");
        }
        //Обработка главного меню
        if (buttonsText.MainMenu.Contains(message.Text) || buttonsText.ReverseMenu.Contains(message.Text))
        {
            DataButtons buttons = new DataButtons();

            if (buttonsText.MainMenu.IndexOf(message.Text) == 0 || buttonsText.ReverseMenu.IndexOf(message.Text) == 1)
            {
                SubComiteesMenu subComMenu = new SubComiteesMenu(botClient, message, buttons, "Выберите местность");
            }
            else if (buttonsText.MainMenu.IndexOf(message.Text) == 1)
            {
                ComiteesMenu comMenu = new ComiteesMenu();
                await comMenu.KeyBoardAsync(botClient, message, buttons, "Выберите комитет");
            }
            else if (buttonsText.MainMenu.IndexOf(message.Text) == 2 || buttonsText.ReverseMenu.IndexOf(message.Text) == 3)
            {
                SheduleMenu sheduleMenu = new SheduleMenu();
                await sheduleMenu.KeyBoardAsync(botClient, message, buttons, "Выберите один из фильтров");
            }
            else if (buttonsText.MainMenu.IndexOf(message.Text) == 3 || buttonsText.ReverseMenu.IndexOf(message.Text) == 2)
            {                
                LiteratureMenu usefulLiteraturMenu = new LiteratureMenu();
                await usefulLiteraturMenu.KeyBoardAsync(botClient, message, buttons, "Выберите доступную литературу");                               
            }
            else if (buttonsText.MainMenu.IndexOf(message.Text) == 4)
            {
                DailyPDF dailyPDF = new DailyPDF();
                await dailyPDF.MainPDFAsync(botClient, message);
            }
            else if (buttonsText.MainMenu.IndexOf(message.Text) == 5)
            {
                Vol_AN vol = new Vol_AN();
                vol.SendMessageAboutVolontInAN(botClient, message);                                       
            }
        }
        //Обработка меню Подкомитеты
        if (buttonsText.SubComitees.Contains(message.Text))
        {
            DataButtons buttons = new DataButtons();

            if (buttonsText.SubComitees.IndexOf(message.Text) == 0)
            {
                SubComiteesMenu subComMenu = new SubComiteesMenu(botClient, message, buttons.SubcometeesNaSPb);
            }
            else if (buttonsText.SubComitees.IndexOf(message.Text) == 2)
            {
                SubComiteesMenu subComMenu = new SubComiteesMenu(botClient, message, buttons.SubcometeesCenter);
            }
            else if (buttonsText.SubComitees.IndexOf(message.Text) == 1)
            {
                SubComiteesMenu subComMenu = new SubComiteesMenu(botClient, message, buttons.SubcometeesSouth);
            }
        }
        //Обработка меню подкомитетов АН СПб
        if (buttonsText.SubcometeesNaSPb.Contains(message.Text))
        {
            DataButtons buttons = new DataButtons();
            ExcelRead excel = new ExcelRead();

            try
            {
                List<double> listMapPoint = excel.ReadExcelForSubComittesMapPoint(message.Text, 0);
                if ((listMapPoint != null))
                {
                    await botClient.SendLocationAsync(message.Chat.Id, listMapPoint[0], listMapPoint[1]);
                }
            }
            catch (Exception)
            {

                throw;
            }

            try
            {
                List<string> list = excel.ReadExcelForSubComittes(message.Text, 0);
                string result = string.Join("\n\n", list);
                await botClient.SendTextMessageAsync(message.Chat.Id, result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        // подкомитеты
        if (buttonsText.SubcometeesCenter.Contains(message.Text))
        {
            DataButtons buttons = new DataButtons();
            ExcelRead excel = new ExcelRead();

            try
            {
                List<double> listMapPoint = excel.ReadExcelForSubComittesMapPoint(message.Text, 1);
                if ((listMapPoint != null))
                {
                    await botClient.SendLocationAsync(message.Chat.Id, listMapPoint[0], listMapPoint[1]);
                }
            }
            catch (Exception)
            {

                throw;
            }

            try
            {
                List<string> list = excel.ReadExcelForSubComittes(message.Text, 1);
                string result = string.Join("\n\n", list);
                await botClient.SendTextMessageAsync(message.Chat.Id, result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        // обработка кнопок разных МКО
        if (buttonsText.SubcometeesSouth.Contains(message.Text))
        {
            DataButtons buttons = new DataButtons();
            ExcelRead excel = new ExcelRead();

            try
            {
                List<double> listMapPoint = excel.ReadExcelForSubComittesMapPoint(message.Text, 2);
                if ((listMapPoint != null))
                {
                    await botClient.SendLocationAsync(message.Chat.Id, listMapPoint[0], listMapPoint[1]);
                }
            }
            catch (Exception)
            {

                throw;
            }

            try
            {
                List<string> list = excel.ReadExcelForSubComittes(message.Text, 2);
                string result = string.Join("\n\n", list);
                await botClient.SendTextMessageAsync(message.Chat.Id, result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Обработка меню Литература
        if (buttonsText.LiteratureMenu.Contains(message.Text) || buttonsText.ReverseMenu.Contains(message.Text))
        {
            DataButtons buttons = new DataButtons();

            if (buttonsText.LiteratureMenu.IndexOf(message.Text) == 0)
            {
                HelpsLiteratureMenu heplsLiteratureMenu = new HelpsLiteratureMenu();
                heplsLiteratureMenu.KeyBoardAsync(botClient, message, buttons, "Выберите литературу выздоровления");
            }
            else if (buttonsText.LiteratureMenu.IndexOf(message.Text) == 1)
            {
                ServiceLiteratureMenu serviceLiteratureMenu = new ServiceLiteratureMenu();
                serviceLiteratureMenu.KeyBoardAsync(botClient, message, buttons, "Выберите служебную литературу");
            }
        }
        //Обработка меню: Служебная литература
        if (buttonsText.ServiceLiterature.Contains(message.Text))
        {
            DataButtons buttons = new DataButtons();
            ServiceLiteratureMenu service = new ServiceLiteratureMenu();

            if (buttons.ServiceLiterature.IndexOf(message.Text) == 0)
            {
                service.SendServiceLiterature(botClient, message, buttons.SP);
            }
            else if (buttons.ServiceLiterature.IndexOf(message.Text) == 1)
            {
                service.SendServiceLiterature(botClient, message, buttons.ByLiterature);
            }
            else if (buttons.ServiceLiterature.IndexOf(message.Text) == 2)
            {
                service.SendServiceLiterature(botClient, message, buttons.Ballots);
            }
            else if (buttons.ServiceLiterature.IndexOf(message.Text) == 3)
            {
                service.SendServiceLiterature(botClient, message, buttons.OtherMaterials);
            }
        }
        //Обработка меню расписание
        if (buttonsText.SheduleMain.Contains(message.Text))
        {
            DataButtons buttons = new DataButtons();
            SheduleGroupOfExcel sheduleGroup = new SheduleGroupOfExcel();

            if (buttonsText.SheduleMain.IndexOf(message.Text) == 0)
            {
                sheduleGroup.SendSheduleGroupToday(botClient, message);
            }
            else if(buttonsText.SheduleMain.IndexOf(message.Text) == 2)
            {
                sheduleGroup.SendSheduleOpenGroup(botClient, message);
            }
            else if(buttonsText.SheduleMain.IndexOf(message.Text) == 1)
            {
                sheduleGroup.SendSheduleNearestGroup(botClient, message);            
            }
            else if (buttonsText.SheduleMain.IndexOf(message.Text) == 3)
            {
                sheduleGroup.SendSheduleGroupLO(botClient, message);
            }
            else if (buttonsText.SheduleMain.IndexOf(message.Text) == 4)
            {
                sheduleGroup.SendSheduleGroupOnline(botClient, message);
            }
            else if (buttonsText.SheduleMain.IndexOf(message.Text) == 6)
            {
                sheduleGroup.SendSheduleSubComittes(botClient, message);
            }   
        }
        //Обработка меню  Комитетов
        if (buttonsText.Сomitees.Contains(message.Text))
        {
            DataButtons buttons = new DataButtons();
            ExcelRead excel = new ExcelRead();

            try
            {
                List<double> listMapPoint = excel.ReadExcelForComittesMapPoint(message.Text);
                await botClient.SendLocationAsync(message.Chat.Id, listMapPoint[0], listMapPoint[1]);

                List<string> list = excel.ReadExcelForComittesMainInformation(message.Text);
                string result = string.Join("\n\n", list);
                await botClient.SendTextMessageAsync(message.Chat.Id, result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Обработка меню Литература выздоровления
        if (buttonsText.HealingLiterature.Contains(message.Text) || buttonsText.ReverseMenu.Contains(message.Text))
        {
            DataButtons buttons = new DataButtons();
            HelpsLiteratureMenu heplsLiteratureMenu = new HelpsLiteratureMenu();
            ServiceLiteratureMenu serviceLiteratureMenu = new ServiceLiteratureMenu();

            if (buttonsText.HealingLiterature.IndexOf(message.Text) == 0)
            {                
                heplsLiteratureMenu.SendHelpsLiterature(botClient, message, buttons.Books);
            }
            else if (buttonsText.HealingLiterature.IndexOf(message.Text) == 1)
            {
                heplsLiteratureMenu.SendHelpsLiterature(botClient, message, buttons.Booklets);
            }
            else if (buttonsText.HealingLiterature.IndexOf(message.Text) == 2)
            {
                heplsLiteratureMenu.SendHelpsLiterature(botClient, message, buttons.IP);
            }
            else if (buttonsText.HealingLiterature.IndexOf(message.Text) == 3)
            {
                heplsLiteratureMenu.SendHelpsLiteraturePhoto(botClient, message, buttons.Cards);
            }
        }
        //Обработка всех групп в СПб
        List<string> list1 = ExcelRead.ReaadExcelAndSearchNamesAllGroupInSPb();
        if (list1.Contains(message.Text))
        {
            ExcelRead excel = new ExcelRead();
            List<double> answerMapPoint = excel.ReadExcelAndSearchInfoAboutMapPointGroupInSPb(message.Text);
            
            await botClient.SendLocationAsync(
                chatId: chatId,
                latitude: answerMapPoint[0],
                longitude: answerMapPoint[1]);

            await botClient.SendTextMessageAsync(message.Chat.Id, excel.ReadExcelAndSearchInfoAboutConrcetGroupInSPb(message.Text));        
        };
        //Обработка всех групп в ЛО
        List<string> list2 = ExcelRead.ReaadExcelAndSearchNamesAllGroupInLO();
        if (list2.Contains(message.Text))
        {
            ExcelRead excel = new ExcelRead();
            List<double> answerMapPoint = excel.ReadExcelAndSearchInfoAboutMapPointGroupLO(message.Text);

            await botClient.SendLocationAsync(
                chatId: chatId,
                latitude: answerMapPoint[0],
                longitude: answerMapPoint[1]);

            await botClient.SendTextMessageAsync(message.Chat.Id, excel.ReadExcelAndSearchInfoAboutConrcetGroupInLO(message.Text));
        };
}


