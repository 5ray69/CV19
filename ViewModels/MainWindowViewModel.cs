using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}

