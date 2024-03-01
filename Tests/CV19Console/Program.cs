using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace CV19Console
{
    class Program
    {
        /*
         * ЭТО ЯДРО БИЗНЕС-ЛОГИКИ
         */
        
        private const string data_url = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
        //метод 1 формирует поток данных
        //метод позволит скачивать выборочно с сайта файл, если файл очень большой
        private static async Task<Stream> GetDataStream()
        {
            var client = new HttpClient(); //создаем клиента
            //HttpCompletionOption.ResponseHeadersRead узнаем только заголовки
            var response = await client.GetAsync(data_url, HttpCompletionOption.ResponseHeadersRead); //получаем ответ от удаленного сервера
            //возвращаем поток, для чтения полных данных, которые пока хранились в
            //сетевой карте, в то время как мы читали только заголовки полных данных
            return await response.Content.ReadAsStreamAsync();
        }

        //метод 2 разбиваем поток полученный в методе 1 на последовательность строк
        //читаем текстовые данные
        private static IEnumerable<string> GetDataLines()
        {
            //получаем поток
            using var data_stream = GetDataStream().Result;
            //скармливаем этому объекту поток
            using var data_reader = new StreamReader(data_stream);
            //после выполненного вверху читаем данные пока не встретится конец потока
            while (!data_reader.EndOfStream)
            {
                //извлекаем очередную строку из ридера
                var line = data_reader.ReadLine();
                //если строка пуста, то переходим к следующей итерации
                if (string.IsNullOrEmpty(line)) continue;
                //генерируем строку (yield - генератор видимо)
                yield return line.Replace("Korea,", "Korea -")
                    .Replace(",Australia", "-Australia")
                    .Replace(",Canada", "-Canada")
                    .Replace(",China", "-China")
                    .Replace(", Denmark", "-Denmark")
                    .Replace(",Denmark", "-Denmark")
                    .Replace(", France", "-France")
                    .Replace(",France", "-France")
                    .Replace(",Netherlands", "-Netherlands")
                    .Replace(", Netherlands", "-Netherlands")
                    .Replace(", New Zealand", "-New Zealand")
                    .Replace(",New Zealand", "-New Zealand")
                    .Replace("Saint Helena, Ascension and Tristan da Cunha, United Kingdom", "Saint Helena-Ascension and Tristan da Cunha-United Kingdom")
                    .Replace(", United Kingdom", "-United Kingdom")
                    .Replace(",United Kingdom", "-United Kingdom");
            }
        }

        //метод 3 получает необходимые нам данные (заранее посмотрели откуда считывать даты)
        private static DateTime[] GetDates() => GetDataLines()
            .First() //берем первую строку
            .Split(',')
            .Skip(4) //пропускаем 4 колонки
            .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture)) //преобразовываем строки в формат DateTime предполагая, что это даты
            .ToArray();

        //Метод 4 получаем данные по стране, уже из другого столбца файла
        private static IEnumerable<(string Contry, string Province, int[] Counts)> GetData()
        {
            var lines = GetDataLines()
                .Skip(1) //первую строку пропускаем, отбрасываем
                .Select(line => line.Split(',')); //разделяем строку по разделетилею запятая

            foreach (var row in lines) 
            {
                var province = row[0].Trim(); //у каждой строки обрезаем все лишнее - пробелы спецсимволы и тп
                var country_name = row[1].Trim(' ','"'); //у country_name обрезаем пробелы и кавычки
                //пропускаем 4 ячейки/строки и приводим к целому числу, и в массив
                var counts = row.Skip(4).Select(int.Parse).ToArray();

                yield return (country_name, province, counts);
            }

        }

        static void Main(string[] args)
        {
            ////класс отправляет http-запросы - можно скачивать содержимое сайтов
            ////WebClient client = new WebClient(); //старая версия класса
            //var client = new HttpClient();

            //var response = client.GetAsync(data_url).Result;
            //var csv_str = response.Content.ReadAsStringAsync().Result;

            //foreach (var data_line in GetDataLines())
            //    Console.WriteLine(data_line);

            //var dates = GetDates();
            //Console.WriteLine(string.Join("\r\n", dates));

            var russia_data = GetData()
                .First(v => v.Contry.Equals("Russia", StringComparison.OrdinalIgnoreCase));

            Console.WriteLine(string.Join("\r\n", GetDates().Zip(russia_data.Counts, (date, count) => $"{date:dd:MM} - {count}")));

            Console.ReadLine();
        }
    }
}
