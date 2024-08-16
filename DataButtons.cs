using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.WebRequestMethods;

namespace RS_bot
{
    class DataButtons
    {
        public static List<string> reverseMenu = new List<string>() { "↩️На главную", "↩️К подкомитетам", "↩️К литературе", "↩️К расписанию" };

        private static List<string> mainMenu = new List<string>() { "⚙️Подкомитеты", "О Комитетах\U0001f6de", "🕰Расписание", "🗄Литература", "📖Eжедневник", "🤝Волонтерство" };
        private static List<string> subComitees = new List<string>() { "🔗Подкомитеты МКО АН СПБ", "🔗Подкомитеты МКО ЮГ", "🔗Подкомитеты МКО Центр" };
        private static List<string> comitees = new List<string>() { "🔗МКО АН СПБ", "🔗МКО ЮГ", "🔗МКО Центр" };
        private static List<string> subcometeesCenter = new List<string> { "🏨ПБУ центр", "🥋ПРС Центр", "📚Литком Центр" };
        private static List<string> subcometeesSouth = new List<string> { "🏨ПБУ Юг", "🥋ПРС Юг", "📚Литком Юг" };
        private static List<string> subcometeesNaSPb = new List<string> { "🥋ПРС АН СПб", "🏬ПСО АН СПб", "📚ЛитКом АН СПб", "🏨ПБУ АН СПб", "📞Инфолиния", "📇Визитка", "📹PRO-Экран" };
        private static List<string> sheduleSPbSearch = new List<string> { "По диапазону времени⌚️", "По районам📍", "Метро Ⓜ️" };
        private static List<string> literatureMenu = new List<string> { "📓Литература выздоровления", "📚Служебная литература" };
        private static List<string> sheduleMain = new List<string> { "📋Расписание собраний сегодня", "🤳Ближайшие собрания", "🔓Открытые собрания", "🏘Собрания в ЛО", "💻Собрания онлайн"};
        //private static List<string> literatureMenu = new List<string> { "Литература выздоровления", "Служебная литература" };
        private static List<string> healingLiterature = new List<string> { "📘Книги🤭", "📰Буклеты", "📕IP(Информационные проспекты)", "📔Карточки для чтения на группах" };
        private static List<string> serviceLiterature = new List<string> { "📒SP(Служебные проспекты)", "📓Литература БУ", "📗Бюллетени", "📚Прочие материалы" };
        /*private static List<string> booklets = new List<string> { "Белый буклет", "В неволе", "Двенадцать концепций обслуживания АН", "Работа над 4-м шагом в АН","Буклеты для группы","Ознакомительное руководство к АН" };*/
        private static List<string> groupCards = new List<string> { "Кто такой зависимый", "Как это работает", "Двенадцать Традиций АН", "Мы действительно выздоравливаем", "Что такое программа АН?", "Почему мы здесь?","Только сегодня" };

        private static List<string> booklets = new List<string> { "Белый буклет.pdf", "Буклет для групп.pdf", "В неволе.pdf", "Двенадцать концепций обслуживания АН.pdf", "Ознакомительное руководство в АН.pdf", "Работа над 4-ым шагом АН.pdf" };

        private static List<string> ip = new List<string> { "Кто, что, как и почему.pdf", "Группа IP# 2.pdf", "Другой взгляд IP# 5.pdf", "Выздоровление и срыв IP# 6.pdf", "Зависимвый ли я IP# 7.pdf", "Только сегодня IP# 8.pdf", "Жить программой IP# 9.pdf", "Спонсорство IP№ 11.pdf", "Треугольник одержимости своими желаниями IP# 12.pdf" };

        private static List<string> sp = new List<string> 
        { 
            "Группы_АН_и_лекарственные_препараты.pdf", 
            "Доверенные служители группы.pdf", 
            "Принципы_и_лидерство_в_служении_АН.pdf", 
            "Принципы_и_лидерство_в_служении_АН.pdf", 
            "Рабочие собрания группы.pdf", 
            "Социальные_сети_и_наши_путеводные_принципы.pdf", 
            "Шумное_и_буйное_поведение_на_собраниях.pdf"
        };

        private static List<string> byLiterature = new List<string>
        {
            "Программа_АН_онлайн_в_тюрьмах_и_закрытых_учреждениях.pdf",
            "Служебный_бюллетень_БУ_№6_Несение_вести_в_учреждениях_для_подростков.pdf",
            "Служебный_бюллетень_№2–_Кто_лучше_всего_подходит_для_несения_вести.pdf",
            "Руководство_по_несению_вести_в_закрытые_учреждения.pdf",
            "Руководство по БУ.pdf",
            "Памятка для группы АН в ИУ.pdf"
        };


        private static List<string> ballots = new List<string>
        {
            "ИП Переписка по шагам.pdf", "БЮЛЛЕТЕНЬ МИРОВЫХ СЛУЖБ АН №30.pdf",
            "БЮЛЛЕТЕНЬ_СОВЕТА_ПОПЕЧИТЕЛЕЙ_МИРОВОГО_ОБСЛУЖИВАНИЯ_‐_№18.pdf",
            "БЮЛЛЕТЕНЬ_СОВЕТА_ПОПЕЧИТЕЛЕЙ_МИРОВОГО_ОБСЛУЖИВАНИЯ_‐_№21.pdf",
            "БЮЛЛЕТЕНЬ_СОВЕТА_ПОПЕЧИТЕЛЕЙ_МИРОВОГО_ОБСЛУЖИВАНИЯ_‐_№29.pdf",
            "Бюллютень 13.pdf",
            "Бюллютень №17.pdf"
        };

        private static List<string> otherMaterials = new List<string>
        {
            "Видение_обслуживания_АН.pdf",
            "Глоссарий_по_обслуживанию.pdf",
            "Глоссарий_АН.pdf",
            "Основы_перевода_Руководство_для_ПК.pdf"
        };

        private static List<string> timeNamesRequest = new List<string>
        {
         "",
         "🕐10:00-11:00",
         "🕐10:15-11:15",
         "🕐10:30-11:30",
         "🕐10:45-11:45",
         "🕐11:00-12:00",
         "🕐11:15-12:15",
         "🕐11:30-12:30",
         "🕐11:45-12:45",
         "🕐12:00-13:00",
         "🕐12:15-13:15",
         "🕐12:30-13:30",
         "🕐12:45-13:45",
         "🕓13:00-14:00",
         "🕓13:15-14:15",
         "🕓13:30-14:30",
         "🕓13:45-14:45",
         "🕓14:00-15:00",
         "🕓14:15-15:15",
         "🕓14:30-15:30",
         "🕓14:45-15:45",
         "🕓15:00-16:00",
         "🕓15:15-16:15",
         "🕓15:30-16:30",
         "🕓15:45-16:45",
         "🕓16:00-17:00",
         "🕓16:15-17:15",
         "🕓16:30-17:30",
         "🕓16:45-17:45",
         "🕓17:00-18:00",
         "🕓17:15-18:15",
         "🕓17:30-18:30",
         "🕓17:45-18:45",
         "🕟18:00-19:00",
         "🕟18:15-19:15",
         "🕟18:30-19:30",
         "🕟18:45-19:45",
         "🕟19:00-20:00",
         "🕟19:15-20:15",
         "🕟19:30-20:30",
         "🕟19:45-20:45",
         "🕟20:00-21:00",
         "🕟20:15-21:15",
         "🕟20:30-21:30",
         "🕟20:45-21:45",
         "🕟21:00-22:00",
         "🕟21:15-22:15",
         "🕟21:30-22:30",
         "🕟21:45-22:45",
         "🕟22:00-23:00",
         "🕟22:15-23:15",
         "🕟22:30-23:30",
         "🕟22:45-23:45",
         "",
        };
        private static List<string> workDateSeminarArray = new List<string>()
         {
             "1-ое число🗓","2-ое число🗓","3-е число🗓","4-ое число🗓",
             "5-ое число🗓","6-ое число🗓","7-ое число🗓","8-ое число🗓",
             "9-ое число🗓","10-ое число🗓","11-ое число🗓","12-ое число🗓",
             "13-ое число🗓","14-ое число🗓","15-ое число🗓","16-ое число🗓",
             "17-ое число🗓","18-ое число🗓","19-ое число🗓","20-ое число🗓",
             "21-ое число🗓","22-ое число🗓","23-е число🗓","24-ое число🗓",
             "25-ое число🗓","26-ое число🗓","27-ое число🗓","28-ое число🗓",
             "29-ое число🗓","30-ое число🗓","31-ое число🗓",""
         };

        private static List<string> seminarsName = new List<string>
        {
            "12 Простых истин в Служении🧭",
            "12 шагов АН📗",
            "Атмосфера выздоровления😶‍🌫️",
            "Бог, как каждый из нас понимает🙏🏻",
            "Вдохновленные нашей главной целью🎯",
            "Ведущий собрания АН✍️",
            "Выздоравливая в АН🏥",
            "Групповая совесть",
            "Добро пожаловать в АН🚪",
            "Добро пожаловать всем🏳️",
            "Духовный кризис📉",
            "Единство в АН🔵",
            "Заместительная терапия и медикаментозное лечение🏥",
            "История АН🏺",
            "История Базового текста📘",
            "История денег💰",
            "История появления и развития литературы АН в России📚🇷🇺",
            "Мотивация служащих📈",
            "Нести весть АН и делать АН привлекательным😍",
            "Новичку посвящается🆕",
            "Новичок в АН🆕",
            "Открывая новую группу🪑",
            "Привлечение членов сообщества в служение🗣",
            "Принцип ротаций в АН🟢",
            "Про деньги💰",
            "Простые истины в Служении🧐",
            "Руководящие принципы – дух наших традиций📓",
            "Самообеспечение в АН7️⃣",
            "Самообеспечение от А до Я 7️⃣",
            "Секретарь группы АН✒️",
            "Семинары ПГО 🧍‍♀️ 🧍‍♂️",
            "Сильная домашняя группа🦾",
            "Служение в АН💪🏻",
            "Служение, как неотъемлемая часть выздоровления",
            "Сопереживание, поддержка и любовь в сообществе",
            "Социальные сети🤳",
            "Спонсорство👥",
            "Строим Сообщество🏗",
            "Структура обслуживания в АН🕍",
            "Темная сторона в служении🌒",
            "Традиции АН📖",
            "Хищническое поведение🐅",
            "Шаги АН👣",
            "Ясность нашей вести🕯",
            "Гибкость и принципиальность📙",
        };

        private static List<string> treningsName = new List<string>
        {
            "Деревня🌳",
            "Для ведущих📋",
            "Единство. Игра. Стикеры🎮",
            "Единство. Костяк группы🦴",
            "Единство. Стулья🪑",
            "Мат на собрании🛑",
            "Новичок на группе🆕",
            "Про деньги💰",
            "Ротация. Передача опыта🤝",
            "Сильная домашняя группа💪🏻",
            "Смерть сообщества⚰️",
            "Третья традиция💵",
        };

        private static List<string> books = new List<string>
        {
            "Базовый текст (5-ое издание).pdf",
            "Руководство к работе по шагам в АН.pdf",
            "Книга о Традициях.pdf",
            "Спонсорство.pdf",
            "Это работает, как и почему.pdf",
            "Жить чистыми.pdf",
        };

        private static List<string> cardsFromGroup = new List<string> 
        { 
            "https://na-tranzit.org/wp-content/uploads/2017/10/Kto-takoj-zavisimyj.jpg", 
            "https://na-tranzit.org/wp-content/uploads/2017/10/CHto-takoe-programma-Anonimnye-Narkomany.jpg", 
            "https://na-tranzit.org/wp-content/uploads/2017/10/Pochemu-my-zdes.jpg",            
            "https://na-tranzit.org/wp-content/uploads/2017/10/Kak-eto-rabotaet.jpg", 
            "https://na-tranzit.org/wp-content/uploads/2017/10/Dvenadtsat-Traditsij-AN.jpg", 
            "https://na-tranzit.org/wp-content/uploads/2017/10/My-dejstvitelno-vyzdoravlivaem.jpg",
            "https://na-tranzit.org/wp-content/uploads/2017/10/Chto-takoe-sluzhenie-v-AN-v2.jpeg",
            "https://na-tranzit.org/wp-content/uploads/2017/10/Tolko-segodnya.jpg",
        };
        public static List<string> userRequestList = new List<string>(5);

        public void AddUserRequestForUs(string userRequest)
        {
            userRequestList.Add(userRequest);
        }
        public List<string> TreningsName { get { return treningsName; } }
        public List<string> UserRequestList { get { return userRequestList; } }
        public List<string> TimeNamesRequest { get { return timeNamesRequest; } }
        public List<string> MainMenu { get { return mainMenu; } }
        public List<string> WorkDateArray { get { return workDateSeminarArray; } }
        public List<string> SeminarsName { get { return seminarsName; } }
        public List<string> SubComitees { get { return subComitees; } }
        public List<string> Books { get { return books; } }
        public List<string> ReverseMenu { get { return reverseMenu; } }
        public List<string> Сomitees { get { return comitees; } }
        public List<string> SheduleMain { get { return sheduleMain; } }
        public List<string> LiteratureMenu { get { return literatureMenu; } }
        public List<string> SubcometeesNaSPb { get { return subcometeesNaSPb; } }    
        public List<string> SubcometeesSouth { get { return subcometeesSouth; } }
        public List<string> SubcometeesCenter { get { return subcometeesCenter; } }
        public List<string> HealingLiterature { get { return healingLiterature; } }        
        public List<string> ServiceLiterature { get { return serviceLiterature; } }
        public List<string> Booklets { get { return booklets; } }
        public List<string> SP { get { return sp; } }  

        public List<string> SheduleSPbSearch { get { return sheduleSPbSearch; } }
        public List<string> IP { get { return ip; } }
        public List<string> Cards { get { return cardsFromGroup; } }

        public List<string> ByLiterature { get { return byLiterature; } }   

        public List<string> Ballots { get { return ballots; } } 

        public List<string> OtherMaterials { get { return otherMaterials; } }   


        public ReplyKeyboardMarkup CreateButtonMarkup(IEnumerable<string> names)
        {
            var keyboardButtons = new List<List<KeyboardButton>>();
            foreach (var name in names)
            {
                var keyboardButtonList = new List<KeyboardButton> { name };
                keyboardButtons.Add(keyboardButtonList);
            }

            var keyboardMarkup = new ReplyKeyboardMarkup(keyboardButtons);
            keyboardMarkup.ResizeKeyboard = true;

            return keyboardMarkup;
        }
        //два столбца кнопок с определением последней
        public ReplyKeyboardMarkup CreateButtonDoubleWhithEndButtonMarkup(IEnumerable<string> names, string additionalButton)
        {
            var keyboardButtons = new List<List<KeyboardButton>>();
            var rowCount = 2;
            var currentRow = new List<KeyboardButton>();

            foreach (var name in names)
            {
                currentRow.Add(name);
                if (currentRow.Count == rowCount)
                {
                    keyboardButtons.Add(currentRow);
                    currentRow = new List<KeyboardButton>();
                }
            }

            if (!string.IsNullOrEmpty(additionalButton))
            {
                currentRow.Add(additionalButton);
            }

            if (currentRow.Count > 0)
            {
                keyboardButtons.Add(currentRow);
            }

            var keyboardMarkup = new ReplyKeyboardMarkup(keyboardButtons);
            keyboardMarkup.ResizeKeyboard = true;

            return keyboardMarkup;
        }

        public ReplyKeyboardMarkup CreateButtonDoubleWithEndButtonMarkup(List<string> names, string additionalButton)
        {
            var keyboardButtons = new List<List<KeyboardButton>>();
            var rowCount = 2;
            var currentRow = new List<KeyboardButton>();

            foreach (var name in names)
            {
                currentRow.Add(new KeyboardButton(name)); // Создаем экземпляр KeyboardButton для каждой строки
                if (currentRow.Count == rowCount)
                {
                    keyboardButtons.Add(currentRow);
                    currentRow = new List<KeyboardButton>();
                }
            }

            if (!string.IsNullOrEmpty(additionalButton))
            {
                currentRow.Add(new KeyboardButton(additionalButton)); // Создаем экземпляр KeyboardButton для дополнительной кнопки
            }

            if (currentRow.Count > 0)
            {
                keyboardButtons.Add(currentRow);
            }

            var keyboardMarkup = new ReplyKeyboardMarkup(keyboardButtons);
            keyboardMarkup.ResizeKeyboard = true;

            return keyboardMarkup;
        }

        //Два столбца кнопок без определения последней
        public ReplyKeyboardMarkup CreateButtonDoubleButtonMarkup(IEnumerable<string> names)
        {
            var keyboardButtons = new List<List<KeyboardButton>>();
            var rowCount = 2;
            var currentRow = new List<KeyboardButton>();

            foreach (var name in names)
            {
                currentRow.Add(name);
                if (currentRow.Count == rowCount)
                {
                    keyboardButtons.Add(currentRow);
                    currentRow = new List<KeyboardButton>();
                }
            }

            if (currentRow.Count > 0)
            {
                keyboardButtons.Add(currentRow);
            }

            var keyboardMarkup = new ReplyKeyboardMarkup(keyboardButtons);
            keyboardMarkup.ResizeKeyboard = true;

            return keyboardMarkup;
        }        
    }    
}
