using CV19.Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV19.Infrastructure.Commands
{
    internal class LambdaCommand : Command
    {
        //readonly чтоб никто их поменять не мог
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;

        //Action<object> этот делегат получает параметр класса object
        public LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;
        }

        //_CanExecute?.Invoke(parameter) считаем, что может быть пустая ссылка и 
        // ?? true и если нет этого делегата, то считаем, что команду можно выполнить в любом случае
        public override bool CanExecute(object parameter) => _CanExecute?.Invoke(parameter) ?? true;

        public override void Execute(object parameter) => _Execute(parameter);
    }
}
