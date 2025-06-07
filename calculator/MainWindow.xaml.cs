using System;
using System.Collections.Generic;
using System.Data;
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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stack<string> history = new Stack<string>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InputValidation(object sender, TextCompositionEventArgs e)
        {
            // Updated regex pattern to match valid arithmetic expressions
            string pattern = @"^\s*([+-]?(\d+([\.\d]+)?|[\.\d]+)([\s]*[*/+-][\s]*(?![*/+-])([+-]?(\d+([\.\d]+)?|[\.\d]+))*)*|\((\s*[+-]?(\d+([\.\d]+)?|[\.\d]+)([\s]*[*/+-][\s]*(?![*/+-])([+-]?(\d+([\.\d]+)?|[\.\d]+))*)*\s*)+\))?\s*$";
            Regex regex = new Regex(pattern);

            // Get the current text in the input field
            string currentText = (sender as TextBox).Text;

            // Combine the current text with the new input
            string newText = currentText + e.Text;

            // Validate the new text against the regex pattern
            e.Handled = !regex.IsMatch(newText);
        }

        private void NumberClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string exp = Expression.Text;
            char? lastChar = null;
            if(exp.Length >= 1)
                lastChar = exp.Last();

            if (lastChar.ToString() == "." && button.Content.ToString() == "."){}
            else
            {
                Expression.Text += button.Content;
            }

        }

        private void OperatorClicked(object sender, RoutedEventArgs e)
        {
            string exp = Expression.Text;
            Button button = sender as Button;
            char? lastChar = null;
            
            if (exp.Length >= 1)
            {   
                lastChar = exp[exp.Length - 1];
                if (lastChar != '+' && lastChar != '-' && lastChar != '×' && lastChar != '÷')
                {
                    Expression.Text += button.Content;
                }
                else if(lastChar == '.')
                {
                    DeleteLast(sender, e);
                }
            }
            else if(button.Content.ToString() == "-")
            {
                Expression.Text += button.Content;
            }
        }

        private void Calculate(object sender, RoutedEventArgs e)
        {
            string exp = Expression.Text;
            exp = exp.Replace('×', '*');
            exp = exp.Replace('÷', '/');
            var result = EvaluateException(exp);
            Expression.Text = result.ToString();
            exp = exp.Replace('*', '×');
            exp = exp.Replace('/', '÷');
            HistoryListBox.Items.Add(exp + "=");
            HistoryListBox.Items.Add(result.ToString());
        }

        private object EvaluateException(string exp)
        {
            try
            {
                var dataTable = new DataTable();
                var value = dataTable.Compute(exp, string.Empty);
                return value;
            }
            catch
            {
                return exp;
            }           
        }

        private void DeleteLast(object sender, RoutedEventArgs e)
        {
            string exp = Expression.Text;
            if(exp.Length >= 1)
            {
                exp = exp.ToString().Remove(Expression.Text.ToString().Length - 1);
            }
            if(exp.Length>0)
            { 
                exp.Remove(exp.Length - 1,1);
                 Expression.Text = exp;
            }
            else
            {
                Expression.Text = null;
            }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            Expression.Clear();
        }

        private void HistoryButtonClicked(object sender, RoutedEventArgs e)
        {
            if(HistoryListBox.IsVisible)
            {
                HistoryListBox.Visibility =Visibility.Collapsed;
            }
            else
            {
                HistoryListBox.Visibility = Visibility.Visible;
            }
        }

        private void HistoryItemSelect(object sender, MouseButtonEventArgs e)
        {
            string exp = HistoryListBox.SelectedItem.ToString();
            bool IsExpression = exp.Contains("=") ;
            if(IsExpression)
            {
                exp = exp.Remove(exp.Length - 1, 1);
            }
            exp = exp.Replace('*', '×');
            exp = exp.Replace('/', '÷');
            Expression.Text += exp;
        }

      
    }
}
