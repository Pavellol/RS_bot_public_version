using iTextSharp.text;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.Common;

namespace RS_bot
{
    public class SheduleGroupOfExcel
    {
        internal async Task SendSheduleSubComittes(ITelegramBotClient botClient, Message message)
        {
            SheduleAllOpenGroupExcel shedule = new SheduleAllOpenGroupExcel(message.Text);
            DataButtons buttons = new DataButtons();

            int columnDayOfWeek = shedule.ReadExcelAndSearchLineDayOfWeekNowSheet1();

            List<string> NamesGroup = shedule.ReadExcelAndSearchAllNamesGroupWhoInLO(columnDayOfWeek);

            List<string> AddressOfOpenGroup = shedule.ReadExcelAndSearchAllInfoAboutAddressGroupWhoInLO(columnDayOfWeek);
            string request = "";
            foreach (var item in AddressOfOpenGroup)
            {
                request += item + "\n";
            }

            await botClient.SendTextMessageAsync(message.Chat.Id, request, replyMarkup: buttons.CreateButtonDoubleWhithEndButtonMarkup(NamesGroup.Distinct(), DataButtons.reverseMenu[3]));
            await botClient.SendTextMessageAsync(message.Chat.Id, "Расписание собраний на сегодня☝️");
        }

        internal async Task SendSheduleOpenGroup(ITelegramBotClient botClient, Message message)
        {
            SheduleAllOpenGroupExcel shedule = new SheduleAllOpenGroupExcel(message.Text);           
            DataButtons buttons = new DataButtons();
            
            int columnDayOfWeek = shedule.ReadExcelAndSearchLineDayOfWeekNowSheet0();
            List<string> NamesGroup = shedule.ReadExcelAndSearchAllNamesGroupWhoIsOpen(columnDayOfWeek);
            List<string> AddressOfOpenGroup = shedule.ReadExcelAndSearchAllInfoAboutAddressGroupWhoIsOpen(columnDayOfWeek);                        
            string request = "";
            foreach (var item in AddressOfOpenGroup)
            {
                request += item + "\n";
            }

            await botClient.SendTextMessageAsync(message.Chat.Id, request, replyMarkup: buttons.CreateButtonDoubleWhithEndButtonMarkup(NamesGroup.Distinct(), DataButtons.reverseMenu[3]));
        }
        public async Task SendMessageInChunksAsync(long chatId, string message, ITelegramBotClient botClient)
        {
            const int MaxMessageLength = 4096;

            if (message.Length <= MaxMessageLength)
            {
                await botClient.SendTextMessageAsync(chatId, message);
                return;
            }

            int offset = 0;
            while (offset < message.Length)
            {
                int length = Math.Min(MaxMessageLength, message.Length - offset);
                string chunk = message.Substring(offset, length);
                await botClient.SendTextMessageAsync(chatId, chunk);
                offset += length;
            }
        }

        internal async Task SendSheduleNearestGroup(ITelegramBotClient botClient, Message message)
        {
            SheduleAllOpenGroupExcel shedule = new SheduleAllOpenGroupExcel(message.Text);
            DataButtons buttons = new DataButtons();

            int columnDayOfWeek = shedule.ReadExcelAndSearchLineDayOfWeekNowSheet0();
            List<string> NamesGroup = new List<string>();            
            Dictionary<string, string> AddressOfOpenGroup = shedule.ReadExcelAndSearchAllInfoAboutAddressGroupWhoWorkNearest(columnDayOfWeek);

            var sortedByTime = AddressOfOpenGroup.OrderBy(kvp => ParseDateTimeNearest(kvp.Value)).ToList();

            string infoAboutGroup = "";

            foreach (var item in sortedByTime)
            {
                infoAboutGroup += (item.Key + " " + item.Value + "\n");
            }

            foreach (var item in sortedByTime)
            {
                NamesGroup.Add(item.Key + "\n");
            }

            try
            {                
                await SendMessageInChunksAsync(message.Chat.Id, infoAboutGroup, botClient);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Показаны группы в ближайшие 3 часа", replyMarkup: buttons.CreateButtonDoubleWhithEndButtonMarkup(NamesGroup.Distinct(), DataButtons.reverseMenu[3]));
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }            
        }

        internal async Task SendSheduleGroupToday(ITelegramBotClient botClient, Message message)
        {
            SheduleAllOpenGroupExcel shedule = new SheduleAllOpenGroupExcel(message.Text);
            DataButtons buttons = new DataButtons();

            int columnDayOfWeek = shedule.ReadExcelAndSearchLineDayOfWeekNowSheet0();
            List<string> NamesGroup = new List<string>();
            Dictionary<string, string> AddressOfOpenGroup = shedule.ReadExcelAndSearchAllInfoAboutAddressGroupWhoWorkNowAfter17(columnDayOfWeek);

            var sortedByTime = AddressOfOpenGroup.OrderBy(kvp => ParseDateTime(kvp.Value)).ToList();

            string infoAboutGroup = "";

            foreach (var item in sortedByTime)
            {
                infoAboutGroup += (item + "\n").Replace("[", "").Replace("]", "");
            }

            foreach (var item in sortedByTime)
            {
                NamesGroup.Add(item.Key + "\n");
            }

            try
            {
                await SendMessageInChunksAsync(message.Chat.Id, infoAboutGroup, botClient);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Все группы на сегодня", replyMarkup: buttons.CreateButtonDoubleWhithEndButtonMarkup(NamesGroup.Distinct(), DataButtons.reverseMenu[3]));
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
        /*private DateTime ParseDateTime(string value)
        {
            // Разделение строки на части
            string[] parts = value.Split('|');

            // Получение времени из первой части
            string timePart = parts[0].Trim();
           
            // Если время содержит только часы и минуты
            if (timePart.Length == 5)
            {
                // Добавляем ":00" для секунд
                timePart += ":00";
            }

            // Получение даты и времени из строки
            string dateTimeStr = $"{DateTime.Now.Date:yyyy-MM-dd} {timePart}";
            
            DateTime dateTime = Convert.ToDateTime(dateTimeStr);
            // Преобразование в объект DateTime
            //return DateTime.ParseExact(dateTimeStr, "yyyy-MM-dd HH:mm:ss", null);
            return dateTime;
        }*/
        private DateTime ParseDateTime(string value)
        {
            // Разделение строки на части
            string[] parts = value.Split('|')[0].Trim().Split('/');

            // Инициализация переменной для наименьшего времени
            DateTime earliestTime = DateTime.MaxValue;

            // Проход по всем частям времени
            foreach (string part in parts)
            {
                // Получение времени из текущей части
                string timePart = part.Trim();

                // Если время содержит только часы и минуты, добавляем ":00" для секунд
                if (timePart.Length == 5)
                {
                    timePart += ":00";
                }

                // Получение даты и времени из строки
                string dateTimeStr = $"{DateTime.Now.Date:yyyy-MM-dd} {timePart}";

                // Преобразование строки в объект DateTime
                DateTime dateTime = Convert.ToDateTime(dateTimeStr);

                // Если текущее время раньше ранее найденного, обновляем переменную earliestTime
                if (dateTime < earliestTime)
                {
                    earliestTime = dateTime;
                }
            }

            // Возвращаем наименьшее время
            return earliestTime;
        }
        private DateTime ParseDateTimeNearest(string value)
        {
            // Разделение строки на части
            string[] parts = value.Split('|')[0].Trim().Split('/');

            // Инициализация переменной для наименьшего времени
            DateTime earliestTime = DateTime.MaxValue;

            // Проход по всем частям времени
            foreach (string part in parts)
            {
                // Получение времени из текущей части
                string timePart = part.Trim();

                // Если время содержит только часы и минуты, добавляем ":00" для секунд
                if (timePart.Length == 5)
                {
                    timePart += ":00";
                }

                // Получение даты и времени из строки
                string dateTimeStr = $"{DateTime.Now.Date:yyyy-MM-dd} {timePart}";

                // Преобразование строки в объект DateTime
                DateTime dateTime = Convert.ToDateTime(dateTimeStr);

                // Если текущее время раньше ранее найденного, обновляем переменную earliestTime
                if (dateTime < earliestTime)
                {
                    earliestTime = dateTime;
                }
            }

            // Возвращаем наименьшее время
            return earliestTime;
        }
        internal async Task SendSheduleGroupLO(ITelegramBotClient botClient, Message message)
        {
            SheduleAllOpenGroupExcel shedule = new SheduleAllOpenGroupExcel(message.Text);
            DataButtons buttons = new DataButtons();

            int columnDayOfWeek = shedule.ReadExcelAndSearchLineDayOfWeekNowSheet1();

            List<string> NamesGroup = shedule.ReadExcelAndSearchAllNamesGroupWhoInLO(columnDayOfWeek);

            List<string> AddressOfOpenGroup = shedule.ReadExcelAndSearchAllInfoAboutAddressGroupWhoInLO(columnDayOfWeek);
            string request = "";
            foreach (var item in AddressOfOpenGroup)
            {
                request += item + "\n";
            }           
            
            await botClient.SendTextMessageAsync(message.Chat.Id, request, replyMarkup: buttons.CreateButtonDoubleWhithEndButtonMarkup(NamesGroup.Distinct(), DataButtons.reverseMenu[3]));
            await botClient.SendTextMessageAsync(message.Chat.Id, "Расписание собраний на сегодня☝️");
        }

        internal async Task SendSheduleGroupOnline(ITelegramBotClient botClient, Message message)
        {
            SheduleAllOpenGroupExcel shedule = new SheduleAllOpenGroupExcel(message.Text);
            DataButtons buttons = new DataButtons();

            int columnDayOfWeek = shedule.ReadExcelAndSearchLineDayOfWeekNowSheet2();

            List<string> NamesGroup = shedule.ReadExcelAndSearchAllNamesGroupWhoWorkOnline(columnDayOfWeek);

            List<string> AddressOfOpenGroup = shedule.ReadExcelAndSearchAllInfoAboutAddressGroupWhoOnline(columnDayOfWeek);
            string request = "";
            foreach (var item in AddressOfOpenGroup)
            {
                request += item + "\n";
            }

            await botClient.SendTextMessageAsync(message.Chat.Id, request, replyMarkup: buttons.CreateButtonDoubleWhithEndButtonMarkup(NamesGroup.Distinct(), DataButtons.reverseMenu[3]));
            await botClient.SendTextMessageAsync(message.Chat.Id, "Расписание собраний на сегодня☝️");
        }
    }
    
    public class SheduleAllOpenGroupExcel
    {       
        private string message;
        private string dayOfWeek;

        public SheduleAllOpenGroupExcel(string message) 
        {
            this.message = message;
        }

        internal List<double> ReadExcelForAllGroupMapPointsAsync()
        {
            /*nameSheet = "АН СПб";*/
            string excelFilePath = "SheduleGroup.xlsx";
            // Проверка существования файла
            if (!System.IO.File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл ExcelForSheduleComettes не найден!");
                return null;
            }
            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                Console.WriteLine(package.Workbook.Worksheets.ToString());
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Используем имя листа из параметра метода
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 3; row <= rowCount; row++) // Начинаем с 2 строки, предполагая, что 1 строка содержит заголовки
                {
                    string str = Convert.ToString(worksheet.Cells[row, 18].Value);
                    Console.WriteLine(Convert.ToString(worksheet.Cells[row, 18].Value));

                    double latitude = Convert.ToDouble(worksheet.Cells[row, 18].Value);// Широта
                    double longitude = Convert.ToDouble(worksheet.Cells[row, 19].Value); // Долгота

                    List<double> list = new List<double>() { latitude, longitude };

                    return list;
                }
            }
            return null;
        }

        internal int ReadExcelAndSearchLineDayOfWeekNowSheet1()
        {            
            string excelFilePath = "SheduleGroup.xlsx";            
            // Проверка существования файла
            if (!System.IO.File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл ExcelForSheduleComettes не найден!");
                return 0;
            }
            int result;
            DateTime date = DateTime.Now;
            string textToFind = Convert.ToString(date.DayOfWeek);
            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                Console.WriteLine(package.Workbook.Worksheets.ToString());
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Rows;
                
                for (int line = 8; line < 15; line++)
                {
                    string dayOfWeek = Convert.ToString(worksheet.Cells[2, line].Value);

                    if (textToFind  == dayOfWeek)
                    {
                        result = line;
                        return result;
                    }
                }
                return 0;                
            }           
        }
        internal int ReadExcelAndSearchLineDayOfWeekNowSheet2()
        {
            string excelFilePath = "SheduleGroup.xlsx";
            // Проверка существования файла
            if (!System.IO.File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл ExcelForSheduleComettes не найден!");
                return 0;
            }

            int result;
            DateTime date = DateTime.Now;
            string textToFind = Convert.ToString(date.DayOfWeek);

            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                Console.WriteLine(package.Workbook.Worksheets.ToString());
                ExcelWorksheet worksheet = package.Workbook.Worksheets[2];
                int rowCount = worksheet.Dimension.Rows;

                for (int line = 5; line < 12; line++)
                {
                    string dayOfWeek = Convert.ToString(worksheet.Cells[2, line].Value);

                    if (textToFind == dayOfWeek)
                    {
                        result = line;
                        return result;
                    }
                }
                return 0;
            }
        }

        internal int ReadExcelAndSearchLineDayOfWeekNowSheet0()
        {
            string excelFilePath = "SheduleGroup.xlsx";
            // Проверка существования файла
            if (!System.IO.File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл ExcelForSheduleComettes не найден!");
                return 0;
            }

            int result;
            DateTime date = DateTime.Now;
            string textToFind = Convert.ToString(date.DayOfWeek);

            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                Console.WriteLine(package.Workbook.Worksheets.ToString());
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int line = 10; line <= 16; line++)
                {
                    string dayOfWeek = Convert.ToString(worksheet.Cells[2, line].Value);

                    if (textToFind == dayOfWeek)
                    {
                        result = line;
                        return result;
                    }
                }
                return 0;
            }
        }

        internal List<string> ReadExcelAndSearchAllNamesGroupWhoIsOpen(int columnDayOfWeek)
        {
            string excelFilePath = "SheduleGroup.xlsx";
            
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {                
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;                
                List<string> NamesGroup = new List<string>();                

                for (int line = 3; line < rowCount; line++)
                {
                    for (int i = 10; i < 16; i++)
                    {
                        if ((Convert.ToString(worksheet.Cells[line, i].Value)).Contains("Открытое собрание"))
                        {
                            NamesGroup.Add(Convert.ToString(worksheet.Cells[line, 1].Value));
                        }
                    }                    
                }
                return NamesGroup;
            }
        }
        
        internal List<string> ReadExcelAndSearchAllNamesGroupWhoInLO(int columnDayOfWeek)
        {
            string excelFilePath = "SheduleGroup.xlsx";

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Rows;
                List<string> NamesGroup = new List<string>();

                for (int line = 3; line < rowCount; line++)
                {
                    if (worksheet.Cells[line, columnDayOfWeek].Value != null)
                    {
                        NamesGroup.Add(Convert.ToString(worksheet.Cells[line, 1].Value));
                    }
                }
                return NamesGroup;
            }
        }

        internal List<string> ReadExcelAndSearchAllNamesGroupWhoWorkOnline(int columnDayOfWeek)
        {
            string excelFilePath = "SheduleGroup.xlsx";

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[2];
                int rowCount = worksheet.Dimension.Rows;
                List<string> NamesGroup = new List<string>();

                for (int line = 3; line < rowCount; line++)
                {
                    if (Convert.ToString(worksheet.Cells[line, columnDayOfWeek].Value) != null && Convert.ToString(worksheet.Cells[line, columnDayOfWeek].Value) != "null")
                    {
                        NamesGroup.Add(Convert.ToString(worksheet.Cells[line, 1].Value));
                    }
                }
                return NamesGroup;
            }
        }

        internal List<string> ReadExcelAndSearchAllInfoAboutGroupWhoIsOpen(int columnDayOfWeek)
        {
            string excelFilePath = "SheduleGroup.xlsx";

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                List<string> InfoAboutGroup = new List<string>();

                for (int line = 3; line < rowCount; line++)
                {
                    if ((Convert.ToString(worksheet.Cells[line, columnDayOfWeek].Value)).Contains("Открытое собрание"))
                    {                       
                        InfoAboutGroup.Add(Convert.ToString(worksheet.Cells[line, columnDayOfWeek].Value));
                    }
                }
                return InfoAboutGroup;
            }
        }
        
        internal List<string> ReadExcelAndSearchAllInfoAboutAddressGroupWhoIsOpen(int columnDayOfWeek)
        {
            string excelFilePath = "SheduleGroup.xlsx";

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                List<string> InfoAboutGroup = new List<string>();

                for (int line = 10; line < 16; line++)
                {
                    for (int i = 3; i < rowCount ; i++)
                    {
                        if (Convert.ToString(worksheet.Cells[i , line].Value).Contains("Открытое собрание"))
                        {
                            InfoAboutGroup.Add("День недели: " + Convert.ToString(worksheet.Cells[2, line].Value)
                                .Replace("Monday","Понедельник")
                                .Replace("Tuesday","Вторник")
                                .Replace("Wednesday", "Среда")
                                .Replace("Thursday", "Четверг")
                                .Replace("Friday", "Пятница")
                                .Replace("Saturday", "Суббота")
                                .Replace("Sunday", "Воскресенье"));
                            InfoAboutGroup.Add("Группа: " + Convert.ToString(worksheet.Cells[i, 1].Value));
                            InfoAboutGroup.Add("Время/Тема: " + Convert.ToString(worksheet.Cells[i, line].Value));
                            InfoAboutGroup.Add("Метро: " + Convert.ToString(worksheet.Cells[i, 4].Value));
                            InfoAboutGroup.Add("Улицца: " + Convert.ToString(worksheet.Cells[i, 5].Value));
                            InfoAboutGroup.Add("Дом: " + Convert.ToString(worksheet.Cells[line, 6].Value) + "\n");
                        }
                    }                    
                }
                return InfoAboutGroup;
            }
        }
        /*internal List<string> ReadExcelAndSearchAllInfoAboutAddressGroupWhoWorkNow(int columnDayOfWeek)
        {
            string excelFilePath = "SheduleGroup.xlsx";

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                List<string> InfoAboutGroup = new List<string>(2);

                for (int line = 10; line < 16; line++)
                {
                    for (int i = 3; i < rowCount; i++)
                    {
                        if (Convert.ToString(worksheet.Cells[i, line].Value) != null && Convert.ToString(worksheet.Cells[i, line].Value) != "-")
                        {
                            InfoAboutGroup.Add("Группа: " + Convert.ToString(worksheet.Cells[i, 1].Value));
                            try
                            {
                                InfoAboutGroup.Add(Convert.ToString(worksheet.Cells[i, line].Value));
                            }
                            catch (Exception)
                            {

                                InfoAboutGroup.Add(Convert.ToString(worksheet.Cells[i, line].Value));
                            }                                                      
                        }
                    }
                }
                return InfoAboutGroup;
            }
        }*/
        internal DateTime ParseDateTimeNearest(string dateTimeString)
        {
            return DateTime.Parse(dateTimeString.Remove(5));
        }
        internal Dictionary<string, string> ReadExcelAndSearchAllInfoAboutAddressGroupWhoWorkNearest(int columnDayOfWeek)
        {
            DateTime now = DateTime.Now;   
            now = now.AddHours(3);
            DateTime threeHoursLater = now.AddHours(3);
            string excelFilePath = "SheduleGroup.xlsx";

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                Dictionary<string, string> groupAddresses = new Dictionary<string, string>();

                for (int i = 3; i < rowCount; i++)
                {
                    if (worksheet.Cells[i, columnDayOfWeek].Value != null)
                    {
                        string timeValue = Convert.ToString(worksheet.Cells[i, columnDayOfWeek].Value);
                        DateTime groupTime = ParseDateTimeNearest(timeValue);

                        if (groupTime >= now && groupTime <= threeHoursLater)
                        {
                            groupAddresses[Convert.ToString(worksheet.Cells[i, 1].Value)] = timeValue;
                        }
                    }
                }
                return groupAddresses;
            }
        }
        internal Dictionary<string, string> ReadExcelAndSearchAllInfoAboutAddressGroupWhoWorkNowAfter17(int columnDayOfWeek)
        {
            string excelFilePath = "SheduleGroup.xlsx";

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                List<string> InfoAboutGroup = new List<string>(2);
                Dictionary<string, string> groupAddresses = new Dictionary<string, string>();

                for (int i = 3; i < rowCount; i++)
                {
                    /*if (worksheet.Cells[i, columnDayOfWeek].Value != null*//*
                        && Convert.ToString(worksheet.Cells[i, columnDayOfWeek].Value).Contains("17:")*//*)
                    {
                        InfoAboutGroup.Add("Группа|тема: " + Convert.ToString(worksheet.Cells[i, 1].Value));
                        try
                        {
                            InfoAboutGroup.Add(Convert.ToString(worksheet.Cells[i, columnDayOfWeek].Value));
                        }
                        catch (Exception)
                        {
                            InfoAboutGroup.Add(Convert.ToString(worksheet.Cells[i, columnDayOfWeek].Value));
                        }
                    }*/
                    if (worksheet.Cells[i, columnDayOfWeek].Value != null)
                    {
                        InfoAboutGroup.Insert(0, Convert.ToString(worksheet.Cells[i, columnDayOfWeek].Value));
                        groupAddresses[Convert.ToString(worksheet.Cells[i, 1].Value)] = Convert.ToString(worksheet.Cells[i, columnDayOfWeek].Value);
                    }
                }
                return groupAddresses;
            }
        }
        internal List<string> ReadExcelAndSearchAllInfoAboutAddressGroupWhoInLO(int columnDayOfWeek)
        {
            string excelFilePath = "SheduleGroup.xlsx";

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Columns;
                List<string> InfoAboutGroup = new List<string>(2);
                for (int i = 3; i < rowCount; i++)
                {
                    if(worksheet.Cells[i, columnDayOfWeek].Value != null)
                    {
                        InfoAboutGroup.Add("Группа: " + Convert.ToString(worksheet.Cells[i, 1].Value));
                        InfoAboutGroup.Add("Время/Тема: " + Convert.ToString(worksheet.Cells[i, columnDayOfWeek].Value) + "\n");
                    }                    
                }
                return InfoAboutGroup;
            }
        }

        internal List<string> ReadExcelAndSearchAllInfoAboutAddressGroupWhoOnline(int columnDayOfWeek)
        {
            string excelFilePath = "SheduleGroup.xlsx";

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[2];
                int rowCount = worksheet.Dimension.Rows;
                List<string> InfoAboutGroup = new List<string>(2);
                for (int i = 3; i < rowCount; i++)
                {
                    if (Convert.ToString(worksheet.Cells[i, columnDayOfWeek].Value).Contains("null") != true)
                    {
                        InfoAboutGroup.Add("Группа: " + Convert.ToString(worksheet.Cells[i, 1].Value));
                        InfoAboutGroup.Add("Время/Тема: " + Convert.ToString(worksheet.Cells[i, columnDayOfWeek].Value).Replace("•", "\n•"));
                        InfoAboutGroup.Add("Ссылка :"+ Convert.ToString(worksheet.Cells[i, 2].Value) + "\n");
                    }
                }
                return InfoAboutGroup;
            }
        }
    }
}
