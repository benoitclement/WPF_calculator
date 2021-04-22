using System;
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
    readonly string[] operators = { "+", "-", "*", "/" };
    bool operationDone = true;
    public MainWindow()
    {
      InitializeComponent();
    }
    public static double Evaluate(string expression)
    {
      var xsltExpression = 
            string.Format("number({0})", 
                new Regex(@"([\+\-\*])").Replace(expression, " ${1} ")
                                        .Replace("/", " div ")
                                        .Replace("%", " mod "));

        return (double)new XPathDocument
            (new StringReader("<r/>"))
                .CreateNavigator()
                .Evaluate(xsltExpression);
    }

    private void TapDigit(object sender, RoutedEventArgs e)
    {
      Button btn = sender as Button;
      string btnKey = btn.Content.ToString();
      if (String.Equals(currentInput.Text, "0") && String.IsNullOrEmpty(fullInputOperation.Text))
      {
        currentInput.Text = btnKey;
      } else if (operators.Contains(currentInput.Text)){
        fullInputOperation.Text += $"{currentInput.Text}"; // could surround with spaces for clarity if Evaluate() can handle
        currentInput.Text = btnKey;
      }
      else currentInput.Text += btnKey;
    }

    private void TapOperator(object sender, RoutedEventArgs e)
    {
      Button btn = sender as Button;
      string btnKey = btn.Content.ToString();
      if (!String.IsNullOrEmpty(currentInput.Text))
      {
        fullInputOperation.Text += currentInput.Text;
        currentInput.Text = btnKey;
      }
    }

    private void TapEqual(object sender, RoutedEventArgs e)
    {
      if (!String.IsNullOrEmpty(fullInputOperation.Text))
      {
        if (!operators.Contains(currentInput.Text))
        {
          fullInputOperation.Text += currentInput.Text;
        }
        currentInput.Text = Math.Round(Evaluate(fullInputOperation.Text), 2).ToString();
      }
    }

    private void TapReset(object sender, RoutedEventArgs e)
    {
      fullInputOperation.Text = "";
      currentInput.Text = "0";
    }
  }
}
