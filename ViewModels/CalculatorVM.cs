using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_calculator.ViewModels.Commands;

namespace WPF_calculator
{
  class CalculatorVM : INotifyPropertyChanged
  {
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public string DisplayOperation { get; set; }
    public string DisplayInput { get; set; }
    public InputParamCommand InputDigitCommand { get; private set; }
    public InputParamCommand InputOperatorCommand { get; private set; }
    public InputCommand InputEqualCommand { get; private set; }
    public InputCommand InputResetCommand { get; private set; }
    public CalculatorVM()
    {
      DisplayOperation = "";
      DisplayInput = "0";
      InputDigitCommand = new InputParamCommand(InputDigit);
      InputOperatorCommand = new InputParamCommand(InputOperator);
      InputEqualCommand = new InputCommand(InputEqual);
      InputResetCommand = new InputCommand(InputReset);
    }

    readonly char[] operators = { '+', '-', '*', '/' };

    public event PropertyChangedEventHandler PropertyChanged;

    public static Double Evaluate(string expression)
    {
      DataTable dt = new DataTable();
      var eval = dt.Compute(expression, "").ToString();
      return Math.Round(Convert.ToDouble(eval), 2);
    }

    private void InputDigit(string Key)
    {
      if ((String.Equals(DisplayInput, "0") && String.IsNullOrEmpty(DisplayOperation)) || DisplayOperation.Contains("="))
      {
        DisplayInput = Key;
        DisplayOperation = "";
      } else if (operators.Contains(DisplayInput[^1])){ // if current input string contains operator there should only be one char (1st or last doesn't matter)
        DisplayOperation += $"{DisplayInput}"; // could surround string with spaces for clarity if Evaluate() can handle
        DisplayInput = Key;
      }
      else DisplayInput += Key;
      OnPropertyChanged("DisplayInput");
      OnPropertyChanged("DisplayOperation");
    }

    private void InputOperator(string Key)
    {
      char? last_char = null;
      if (!String.IsNullOrEmpty(DisplayOperation)) last_char = DisplayOperation[^2];

      if (last_char != null && last_char == '=')
      {
        DisplayOperation = DisplayInput;
        DisplayInput = Key;
      } else if (!String.IsNullOrEmpty(DisplayInput))
      {
        if (!operators.Contains(DisplayInput[^1])) DisplayOperation += DisplayInput;
        DisplayInput = Key;
      }
      OnPropertyChanged("DisplayInput");
      OnPropertyChanged("DisplayOperation");
    }

    private void InputEqual()
    {
      if (!String.IsNullOrEmpty(DisplayOperation))
      {
        if (!operators.Contains(DisplayInput[^1]))
        {
          DisplayOperation += DisplayInput;
        }
        DisplayInput = Evaluate(DisplayOperation).ToString();
        DisplayOperation += " = ";
      }
      OnPropertyChanged("DisplayInput");
      OnPropertyChanged("DisplayOperation");
    }

    private void InputReset()
    {
      DisplayOperation = "";
      DisplayInput = "0";
      OnPropertyChanged("DisplayInput");
      OnPropertyChanged("DisplayOperation");
    }

  }
}
