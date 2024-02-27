using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CV19.Infrastructure.Commands.Base
{
    internal abstract class Command : ICommand
    {
        //событие происходит, когда метод CanExecute переходит
        //из одного состояния в другое
        public event EventHandler CanExecuteChanged 
        {
            //передаем управление этим событием системе wpf, реализовав два элемента события add и remove
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
            //таким образом управление событием передали классу CommandManager и wpf автоматически
            //генерирует это событие у всех команд, когда что-то происходит
        }

        //если метод возвращает false, то команду выполнить нельзя и элемент,
        //к которому привязана команда отключается автоматически
        public abstract bool CanExecute(object parameter);
        
        //то что должно быть выполнено самой командой,
        //это основная логика которую должна выполнять команда
        public abstract void Execute(object parameter);

        //сделали методы абстрактными и их реализацией займется наследник
    }
}
