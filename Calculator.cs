using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_calculator
{
  class Calculator : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string fullOperation { get; set; }
    public string currentInput { get; set; }

    public Calculator()
    {
      fullOperation = "";
      currentInput = "0";
    }

    public ICommand InputNumber { get; }

    public class DelegateCommand : ICommand
    {
      public event EventHandler CanExecuteChanged;

      public bool CanExecute(object parameter)
      {
        throw new NotImplementedException();
      }

      public void Execute(object parameter)
      {
        throw new NotImplementedException();
      }
    }

  }
}
