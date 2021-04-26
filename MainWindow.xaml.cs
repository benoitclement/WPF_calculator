using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.XPath;

namespace WPF_calculator
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    readonly char[] operators = { '+', '-', '*', '/' };
    //bool displayResult = false;
    public MainWindow()
    {
      InitializeComponent();
    }
    //public static double Evaluate(string expression)
    //{
    //  var xsltExpression = 
    //        string.Format("number({0})", 
    //            new Regex(@"([\+\-\*])").Replace(expression, " ${1} ")
    //                                    .Replace("/", " div ")
    //                                    .Replace("%", " mod "));

    //    return (double)new XPathDocument
    //        (new StringReader("<r/>"))
    //            .CreateNavigator()
    //            .Evaluate(xsltExpression);
    //}
    public static Double Evaluate(string expression)
    {
      DataTable dt = new DataTable();
      var eval = dt.Compute(expression, "").ToString();
      return Math.Round(Convert.ToDouble(eval), 2);

    }

    private void TapDigit(object sender, RoutedEventArgs e)
    {
      Button btn = sender as Button;
      string btnKey = btn.Content.ToString();
      
      if ((String.Equals(currentInput.Text, "0") && String.IsNullOrEmpty(fullInputOperation.Text)) || fullInputOperation.Text.Contains("="))
      {
        //displayResult = false;
        currentInput.Text = btnKey;
        fullInputOperation.Text = "";
      } else if (operators.Contains(currentInput.Text[^1])){ // if current input string contains operator there should only be one char (1st or last doesn't matter)
        fullInputOperation.Text += $"{currentInput.Text}"; // could surround string with spaces for clarity if Evaluate() can handle
        currentInput.Text = btnKey;
      }
      else currentInput.Text += btnKey;
    }

    private void TapOperator(object sender, RoutedEventArgs e)
    {
      Button btn = sender as Button;
      string btnKey = btn.Content.ToString();
      string inputOp = fullInputOperation.Text;
      char? last_char = null;
      if (!String.IsNullOrEmpty(fullInputOperation.Text)) last_char = fullInputOperation.Text[^2];

      if (last_char != null && last_char == '=')
      {
        fullInputOperation.Text = currentInput.Text;
        currentInput.Text = btnKey;
      } else if (!String.IsNullOrEmpty(currentInput.Text))
      {
        if(!operators.Contains(currentInput.Text[^1])) fullInputOperation.Text += currentInput.Text;
        currentInput.Text = btnKey;
      }
    }

    private void TapEqual(object sender, RoutedEventArgs e)
    {
      if (!String.IsNullOrEmpty(fullInputOperation.Text))
      {
        if (!operators.Contains(currentInput.Text[^1]))
        {
          fullInputOperation.Text += currentInput.Text;
        }
        //currentInput.Text = Math.Round(Evaluate(fullInputOperation.Text, "")), 2);
        currentInput.Text = Evaluate(fullInputOperation.Text).ToString();
        fullInputOperation.Text += " = ";
        //displayResult = true;
      }
    }

    private void TapReset(object sender, RoutedEventArgs e)
    {
      fullInputOperation.Text = "";
      currentInput.Text = "0";
    }
  }
}
