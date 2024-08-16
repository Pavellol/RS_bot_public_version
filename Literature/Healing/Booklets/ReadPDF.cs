using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RS_bot.Literature.Healing.Booklets
{
    internal class ReadPDF
    {
        

        internal List<string> GetFileList(string directoryPath)
        {
            var files = new List<string>();

            try
            {
                foreach (string filePath in Directory.GetFiles(directoryPath))
                {
                    files.Add(Path.GetFileName(filePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading files: {ex.Message}");
            }

            return files;
        }

        internal ReplyKeyboardMarkup GenerateReplyKeyboard(List<string> files)
        {
            var buttons = files.Select(f => new KeyboardButton(f)).ToArray();
            var keyboardButtons = new List<KeyboardButton[]>();

            // Разбиваем кнопки на два столбца
            for (int i = 0; i < buttons.Length; i += 2)
            {
                if (i + 1 < buttons.Length)
                    keyboardButtons.Add(new KeyboardButton[] { buttons[i], buttons[i + 1] });
                else
                    keyboardButtons.Add(new KeyboardButton[] { buttons[i] });
            }

            // Добавляем кнопку "Назад"
            keyboardButtons.Add(new KeyboardButton[] { new KeyboardButton(DataButtons.reverseMenu[2]) });

            return new ReplyKeyboardMarkup(keyboardButtons);
        }
    }
}
