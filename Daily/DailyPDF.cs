using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace RS_bot.Daily
{
    class DailyPDF
    {        
        internal async Task MainPDFAsync(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendStickerAsync(
                    chatId: message.Chat.Id,
                    sticker: InputFile.FromFileId("CAACAgIAAxkBAAELzYdmBAXCfXURdG-FpDXV26wHTqRAzwACQgsAAr8d8En10koHIoNIwDQE"));

            DateTime currentDate = DateTime.Now;
            int pageToExtract = currentDate.DayOfYear;
            string filePath = "Ежик.pdf";
            try
            {
                PdfReader reader = new PdfReader(filePath);
                string pageContent = PdfTextExtractor.GetTextFromPage(reader, pageToExtract);       
                string[] words = pageContent.Split( new string[] {"\n \n" }, StringSplitOptions.None);
                
                string userAnswer = "";                
                foreach (string chapter in words)
                {
                    userAnswer += chapter.Trim().Replace("\n", "") + "\n\n";
                }
                await botClient.SendTextMessageAsync(message.Chat.Id, userAnswer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Произошла ошибка, обратитесь пожалуйста к координатору :^(");
            }
        }
    }
}
