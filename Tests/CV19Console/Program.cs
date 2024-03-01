using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CV19Console
{
    class Program
    {
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
                if(string.IsNullOrEmpty(line)) continue;
                //генерируем строку (yield - генератор видимо)
                yield return line;
            }
        }

        //метод 3 получает необходимые нам данные (заранее посмотрели откуда считывать даты)
        private static DateTime[] GetDates() => GetDataLines()
            .First() //берем первую строку
            .Split(',')
            .Skip(4) //пропускаем 4 колонки
            .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture)) //преобразовываем строки в формат DateTime предполагая, что это даты
            .ToArray();




        static void Main(string[] args)
        {
            ////класс отправляет http-запросы - можно скачивать содержимое сайтов
            ////WebClient client = new WebClient(); //старая версия класса
            //var client = new HttpClient();

            //var response = client.GetAsync(data_url).Result;
            //var csv_str = response.Content.ReadAsStringAsync().Result;

            //foreach (var data_line in GetDataLines())
            //    Console.WriteLine(data_line);

            var dates = GetDates();
            Console.WriteLine(string.Join("\r\n", dates));

            Console.ReadLine();
        }
    }
}
