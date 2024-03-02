using CV19.Infrastructure.Commands;
using CV19.Models;
using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CV19.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        ////Пример переопределения метода Dispose и освобождения ресурсов, которые модель захватит
        //protected override void Dispose(bool Disposing){ base.Dispose(Disposing); }

        //Проверяем на работоспособность созданием свойство и подцепляем на него
        //какие-то элементы, чтобы они из него подцепили данные.
        //Для проверки создаем свойство с заголовком окна и статус, который должен отображаться внизу
        //в статус баре

        #region TestDataPoints : IEnumerable<DataPoint> - Тестовый набор данных для визуализации графиков

        /// <summary>Тестовый набор данных для визуализации графиков/// </summary>
        private IEnumerable<DataPoint> _TestDataPoints;

        /// <summary>Тестовый набор данных для визуализации графиков/// </summary>
        private IEnumerable<DataPoint> TestDataPoints
        {
            get => _TestDataPoints;
            set=> Set(ref _TestDataPoints, value);
        }
        #endregion

        #region Заголовок окна
        private string _Title = "Анализ статистики CV19"; //поле для данных заголовка окна
        //В каждом свойстве должен быть такой код
        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _Title;
            //set 
            //{
            //    if (Equals(_Title != value)) return;
            //    _Title = value;
            //    OnPropertyChanged();
            //}

            //    Set(ref _Title, value); //упрощаем, тк мы написали метод Set
            set => Set(ref _Title, value);
        }
        #endregion

        #region Status : string - Статус программы

        /// <summary>Статус программы</summary>
        private string _Status = "Готов!";

        /// <summary>Статус программы</summary>
        public string Status 
        {
            get => _Status;
            set => Set(ref _Status, value); 
        }

        #endregion


        #region Команды
        #region CloseApplicationCommand
        //команда, которая позволит закрывать нашу программу
        public ICommand CloseApplicationCommand { get; } //это сама команда

        //этот метод выполняется в тот момент когда команда выполняется
        private void OnCloseApplicationCommandExecuted(object p) //это метод определяющий команду
        {
            Application.Current.Shutdown();
        }

        // => true обозначает, что метод всегда возвращает true (return true)
        // написали вметсо тела метода в скобках {}
        private bool CanCloseApplicationCommandExecute(object p) => true; //это метод определяющий команду

        #endregion
        #endregion

        public MainWindowViewModel()
        {
            #region Команды
            
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

            #endregion

            var data_points = new List<DataPoint>((int)(360 / 0.1));
            for (var x = 0d; x <= 360; x += 0.1)
            {
                const double to_rad = Math.PI / 180;
                var y = Math.Sin(x * to_rad);
                data_points.Add(new DataPoint { XValue = x, YValue = y });
            }

            TestDataPoints = data_points;
        }

    }
}

