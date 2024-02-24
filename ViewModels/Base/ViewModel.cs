using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CV19.ViewModels.Base
{
    //INotifyPropertyChanged следит за изменениями
    internal abstract class ViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //средство для генерации события, чтобы все наследники смогли им воспользоваться
        //[CallerMemberName] string PropertyName = null компилятор автоматически подставит в PropertyName имя метода
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        //Метод Set будет обновлять значение свойства для которого определено поле, 
        //в котором это свойство хранит свои данные.
        //Задача этого метода разрешить кольцевые изменения свойств, которые могут возникать
        // - чтоб небыло кольцевого зацикливания и переполнения стека
        //ref T field это ссылка на поле свойства
        //T value это новое значение, которое мы хотим установить
        //PropertyName это будет имя свойства, которое мы передадим в OnPropertyChanged
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null) 
        {
            //если значение поля соответствует тому значению, на которое мы
            //хотим изменить это поле, то мы ничего не делаем
            if(Equals(field, value)) return false;
            field = value; //если значение изменилось, то обновляем поле и генерируем OnPropertyChanged
            OnPropertyChanged(PropertyName);
            return true;
        }

        //это пример как надо этот метод вызывать
        //~ViewModel()
        //{
        //    Dispose(false);
        //}

        public void Dispose() 
        {
            Dispose(true);
        }

        private bool _Disposed;
        protected virtual void Dispose(bool Disposing) 
        {
            if(!Disposing || _Disposed) return;
            _Disposed = true;
            //Освобождение управляемых ресурсов
        }

    }
}
