using OfficeOpenXml;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS_bot
{
    public class ExcelRead
    {

        internal List<string> ReadExcelForComittesMainInformation(string nameSheet)
        {
            /*nameSheet = "АН СПб";*/
            string excelFilePath = "SheduleComettes.xlsx";

            // Проверка существования файла
            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл ExcelForSheduleComettes не найден!");
                return null;
            }

            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[nameSheet]; // Используем имя листа из параметра метода
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Начинаем с 2 строки, предполагая, что 1 строка содержит заголовки
                {
                    string name = worksheet.Cells[row, 1].Value?.ToString(); // Название
                    string info = worksheet.Cells[row, 2].Value?.ToString(); // Информация
                    string time = worksheet.Cells[row, 3].Value?.ToString(); // Время проведения
                    string address = worksheet.Cells[row, 4].Value?.ToString(); // Адрес
                    /*double latitude = Convert.ToDouble(worksheet.Cells[row, 5].Value); // Широта
                    double longitude = Convert.ToDouble(worksheet.Cells[row, 6].Value); // Долгота
*/                    
                    List<string> list = new List<string>() { name, info, time, address };
                    return list;
                }
            }
            return null;
        }
        internal string ReadExcelAndSearchInfoAboutConrcetGroupInSPb(string nameGroup)
        {
            /*nameSheet = "АН СПб";*/
            string excelFilePath = "SheduleGroup.xlsx";

            // Проверка существования файла
            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл Excel SheduleGroup не найден!");
                return null;
            }

            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Используем имя листа из параметра метода
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 3; row <= rowCount; row++) // Начинаем с 2 строки, предполагая, что 1 строка содержит заголовки
                {
                    if(nameGroup == worksheet.Cells[row, 1].Value?.ToString())
                    {
                        string name = "Группа: " + worksheet.Cells[row, 1].Value?.ToString(); 
                        string info = "Город: " + worksheet.Cells[row, 2].Value?.ToString(); 
                        string time = "Район: " + worksheet.Cells[row, 3].Value?.ToString(); 
                        string metro = "Метро: " + worksheet.Cells[row, 4].Value?.ToString(); 
                        string streetName = "Улица: " + worksheet.Cells[row, 5].Value?.ToString(); 
                        string numberHouse = "дом: " + worksheet.Cells[row, 6].Value?.ToString(); 
                        string dopInfo = "Дополнительная информация: " + worksheet.Cells[row, 7].Value?.ToString();
                        string workInfo = "Рабочее собрание:" + worksheet.Cells[row, 8].Value?.ToString();
                        string mainInfo = "Расписание на неделю:" + worksheet.Cells[row, 7].Value?.ToString();
                        string workTime = "Пн: " + worksheet.Cells[row, 10].Value?.ToString() + "\n";

                        workTime += "Вт: " + worksheet.Cells[row, 11].Value?.ToString() + "\n";
                        workTime += "Ср: " + worksheet.Cells[row, 12].Value?.ToString() + "\n";
                        workTime += "Чт: " + worksheet.Cells[row, 13].Value?.ToString() + "\n";
                        workTime += "Пт: " + worksheet.Cells[row, 14].Value?.ToString() + "\n";
                        workTime += "Сб: " + worksheet.Cells[row, 15].Value?.ToString() + "\n";
                        workTime += "Вс: " + worksheet.Cells[row, 16].Value?.ToString() + "\n";                        

                        string answer = name + "\n" + info + "\n" + time + "\n" + metro + "\n" + streetName + "\n" + numberHouse + "\n" + dopInfo + "\n" + mainInfo + "\n" + workInfo + "\n" + workTime;
                        return answer;
                    }                                        
                }
            }
            return null;
        }

        internal string ReadExcelAndSearchInfoAboutConrcetGroupInLO(string nameGroup)
        {
            /*nameSheet = "АН СПб";*/
            string excelFilePath = "SheduleGroup.xlsx";

            // Проверка существования файла
            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл Excel SheduleGroup не найден!");
                return null;
            }

            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1]; // Используем имя листа из параметра метода
                int rowCount = worksheet.Dimension.Columns;

                for (int row = 3; row <= rowCount; row++) // Начинаем с 2 строки, предполагая, что 1 строка содержит заголовки
                {
                    if (nameGroup == worksheet.Cells[row, 1].Value?.ToString())
                    {
                        string name = "Группа: " + worksheet.Cells[row, 1].Value?.ToString();
                        string info = "Город: " + worksheet.Cells[row, 2].Value?.ToString();
                        string streetName = "Улица: " + worksheet.Cells[row, 3].Value?.ToString();
                        string numberHouse = "дом: " + worksheet.Cells[row, 4].Value?.ToString();
                        string dopInfo = "Дополнительная информация: " + worksheet.Cells[row, 5].Value?.ToString();
                        string workInfo = worksheet.Cells[row, 6].Value?.ToString();
                        string workTime = "Пн: " + worksheet.Cells[row, 8].Value?.ToString() + "\n";
                        workTime += "Вт: " +  worksheet.Cells[row, 9].Value?.ToString() + "\n";
                        workTime += "Ср: " + worksheet.Cells[row, 10].Value?.ToString() + "\n";
                        workTime += "Чт: " + worksheet.Cells[row, 11].Value?.ToString() + "\n";
                        workTime += "Пт: " + worksheet.Cells[row, 12].Value?.ToString() + "\n";
                        workTime += "Сб: " + worksheet.Cells[row, 13].Value?.ToString() + "\n";
                        workTime += "Вс: " + worksheet.Cells[row, 14].Value?.ToString() + "\n";

                        string answer = name + "\n" + info + "\n" + streetName + "\n" + numberHouse + "\n" + dopInfo + "\n" + workInfo + "\n" + "\n" + workTime;
                        return answer;
                    }
                }
            }
            return null;
        }
        internal List<double> ReadExcelAndSearchInfoAboutMapPointGroupInSPb(string nameGroup)
        {
            /*nameSheet = "АН СПб";*/
            string excelFilePath = "SheduleGroup.xlsx";

            // Проверка существования файла
            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл Excel SheduleGroup не найден!");
                return null;
            }

            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Используем имя листа из параметра метода
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 3; row <= rowCount; row++) // Начинаем с 2 строки, предполагая, что 1 строка содержит заголовки
                {
                    if (nameGroup == worksheet.Cells[row, 1].Value?.ToString())
                    {
                        double latitude = Convert.ToDouble(worksheet.Cells[row, 18].Value?.ToString());
                        double longitude = Convert.ToDouble(worksheet.Cells[row, 19].Value?.ToString());
                        List<double> doubles = new List<double>() { latitude, longitude};
                        return doubles;
                    }
                }
            }
            return null;
        }
        internal List<double> ReadExcelAndSearchInfoAboutMapPointGroupLO(string nameGroup)
        {            
            string excelFilePath = "SheduleGroup.xlsx";
            // Проверка существования файла
            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл Excel SheduleGroup не найден!");
                return null;
            }
            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1]; // Используем имя листа из параметра метода
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 3; row <= rowCount; row++) // Начинаем с 2 строки, предполагая, что 1 строка содержит заголовки
                {
                    if (nameGroup == worksheet.Cells[row, 1].Value?.ToString())
                    {
                        double latitude = Convert.ToDouble(worksheet.Cells[row, 16].Value?.ToString());
                        double longitude = Convert.ToDouble(worksheet.Cells[row, 17].Value?.ToString());
                        List<double> doubles = new List<double>() { latitude, longitude };
                        return doubles;
                    }
                }
            }
            return null;
        }
        internal List<double> ReadExcelForComittesMapPoint(string nameSheet)
        {
            /*nameSheet = "АН СПб";*/
            string excelFilePath = "SheduleComettes.xlsx";

            // Проверка существования файла
            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл ExcelForSheduleComettes не найден!");
                return null;
            }

            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[nameSheet]; // Используем имя листа из параметра метода
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Начинаем с 2 строки, предполагая, что 1 строка содержит заголовки
                {                    
                    double latitude = double.Parse((string)worksheet.Cells[row, 5].Value, System.Globalization.CultureInfo.InvariantCulture);// Широта
                    double longitude = double.Parse((string)worksheet.Cells[row, 6].Value, System.Globalization.CultureInfo.InvariantCulture); // Долгота
                                                                                                                                               // 
                    List<double> list = new List<double>() { latitude, longitude };

                    return list;
                }
            }
            return null;
        }

        internal List<double> ReadExcelForSubComittesMapPoint(string nameSheet, int numberSheet)
        {
            string excelFilePath = "SheduleComettes.xlsx";

            // Проверка существования файла
            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл ExcelForSheduleComettes не найден!");
                return null;
            }

            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                if (package.Workbook.Worksheets.Count <= numberSheet)
                {
                    Console.WriteLine("Лист с индексом " + numberSheet + " не существует!");
                    return null;
                }

                ExcelWorksheet worksheet = package.Workbook.Worksheets[numberSheet]; // Индекс листа начинается с 0
                int rowCount = worksheet.Dimension.Rows;

                if (rowCount <= 1)
                {
                    Console.WriteLine("Лист с индексом " + numberSheet + " не содержит данных!");
                    return null;
                }

                for (int row = 2; row <= rowCount; row++) // Начинаем с 2 строки, предполагая, что 1 строка содержит заголовки
                {
                    string value = (string)worksheet.Cells[row, 1].Value;
                    if (string.IsNullOrEmpty(value))
                    {
                        // Пустое поле - значит, дальше нет данных
                        return null;
                    }

                    if (value == nameSheet)
                    {
                        if (worksheet.Cells[row, 5].Value != null && worksheet.Cells[row, 6].Value != null)
                        {
                            string latitudeStr = worksheet.Cells[row, 5].Value.ToString();
                            string longitudeStr = worksheet.Cells[row, 6].Value.ToString();

                            if (double.TryParse(latitudeStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double latitude) &&
                                double.TryParse(longitudeStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double longitude))
                            {
                                List<double> list = new List<double>() { latitude, longitude };
                                return list;
                            }
                            else
                            {
                                // Обработка ситуации, когда значение не удалось преобразовать в число
                                Console.WriteLine("Ошибка: Не удалось преобразовать значение в число для строки " + row);
                            }
                        }
                    }
                }
                
            }

            // Если не найдено совпадений или нет данных на листе, возвращаем null
            return null;
        }


        internal List<string> ReadExcelForSubComittes(string searchValue, int sheetIndex)
        {
            string excelFilePath = "SheduleComettes.xlsx";

            // Проверка существования файла
            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл ExcelForSheduleComettes не найден!");
                return null;
            }

            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                if (package.Workbook.Worksheets.Count <= sheetIndex)
                {
                    Console.WriteLine("Лист с индексом " + sheetIndex + " не существует!");
                    return null;
                }

                ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetIndex];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Начинаем с 2 строки, предполагая, что 1 строка содержит заголовки
                {
                    string value = (string)worksheet.Cells[row, 1].Value;
                    if (string.IsNullOrEmpty(value))
                    {
                        // Пустое поле - значит, дальше нет данных
                        return null;
                    }

                    if (value == searchValue)
                    {
                        string mainInfo = (string)worksheet.Cells[row, 2].Value;// Информация что делают
                        string timeWork = (string)worksheet.Cells[row, 3].Value; // Время проведения
                        string address = (string)worksheet.Cells[row, 4].Value; // Адрес

                        List<string> list = new List<string>() { mainInfo, timeWork, address };

                        return list;
                    }
                }
            }

            // Если не найдено совпадений или нет данных на листе, возвращаем null
            return null;
        }
        public static List<string> ReaadExcelAndSearchNamesAllGroupInSPb()
        {
            string excelFilePath = "SheduleGroup.xlsx";

            // Проверка существования файла
            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл ExcelForSheduleComettes не найден!");
                return null;
            }

            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {               
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                List<string> answerList = new List<string>();
                for (int row = 3; row <= rowCount; row++) // Начинаем с 2 строки, предполагая, что 1 строка содержит заголовки
                {
                    string value = (string)worksheet.Cells[row, 1].Value;
                    answerList.Add(value);
                                     
                }
                return answerList;
            }         
        }

        public static List<string> ReaadExcelAndSearchNamesAllGroupInLO()
        {
            string excelFilePath = "SheduleGroup.xlsx";

            // Проверка существования файла
            if (!File.Exists(excelFilePath))
            {
                Console.WriteLine("Файл ExcelForSheduleComettes не найден!");
                return null;
            }

            // Чтение данных из Excel-файла
            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Rows;

                List<string> answerList = new List<string>();
                for (int row = 3; row <= rowCount; row++) // Начинаем с 2 строки, предполагая, что 1 строка содержит заголовки
                {
                    if(worksheet.Cells[row, 1].Value != null)
                    {
                        string value = (string)worksheet.Cells[row, 1].Value;
                        answerList.Add(value);
                    }                    
                }
                return answerList;
            }
        }
    }
}
