using System;
using System.Text;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using zad2.Utils;

namespace zad2
{
    public partial class MainWindow : Window
    {
        string DecimalSeparator => CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
        decimal FirstValue { get; set; }
        decimal? SecondValue { get; set; }

        IOperation CurrentOperation;

        private StringBuilder previousOperation = new StringBuilder();


        public MainWindow()
        {
            InitializeComponent();
            btnPoint.Content = DecimalSeparator;
            btnSum.Tag = new Sum();
            btnSubtraction.Tag = new Subtraction();
            btnDivision.Tag = new Division();
            btnMultiplication.Tag = new Multiplication();
        }

        private void UpdatePreviousOperationLabel()
        {
            if (CurrentOperation != null && SecondValue != null)
            {
                previousOperation.Clear();
                previousOperation.Append($"{FirstValue} {CurrentOperation.Symbol} {SecondValue}");
                displayPreviousOperation.Content = previousOperation.ToString();
            }
        }


        private void regularButtonClick(object sender, RoutedEventArgs e)
        {
            SendToInput(((Button)sender).Content.ToString());

            UpdatePreviousOperationLabel();
        }



        private void SendToInput(string content)
        {
            //0 nie pojawia się z lewej strony
            if (txtInput.Text == "0")
                txtInput.Text = "";

            txtInput.Text = $"{txtInput.Text}{content}";
        }

        private void btnPoint_Click(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text.Contains(this.DecimalSeparator))
                return;

            regularButtonClick(sender, e);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            //Nie usuwa zera
            if (txtInput.Text == "0")
                return;

            txtInput.Text = txtInput.Text.Substring(0, txtInput.Text.Length - 1);
            if (txtInput.Text == "")
                txtInput.Text = "0";
        }

        private void operationButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentOperation == null)
                FirstValue = Convert.ToDecimal(txtInput.Text);

            CurrentOperation = (IOperation)((Button)sender).Tag;
            SecondValue = null;
            txtInput.Text = "";

            UpdatePreviousOperationLabel();
        }

        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            switch (e.Text)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    SendToInput(e.Text);
                    break;

                case "*":
                    btnMultiplication.PerformClick();
                    break;

                case "-":
                    btnSubtraction.PerformClick();
                    break;

                case "+":
                    btnSum.PerformClick();
                    break;

                case "/":
                    btnDivision.PerformClick();
                    break;

                case "=":
                    btnEquals.PerformClick();
                    break;

                default:
                    if (e.Text == DecimalSeparator)
                        btnPoint.PerformClick();
                    else if (e.Text[0] == (char)8)
                        btnBack.PerformClick();
                    else if (e.Text[0] == (char)13)
                        btnEquals.PerformClick();

                    break;
            }

            btnEquals.Focus();
        }

        private void btnEquals_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentOperation == null)
                return;

            if (txtInput.Text == "")
                return;

            decimal val2 = SecondValue ?? Convert.ToDecimal(txtInput.Text);
            try
            {
                txtInput.Text = (FirstValue = CurrentOperation.DoOperation(FirstValue, (decimal)(SecondValue = val2))).ToString();
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Can't divide by zero", "Divided by zero", MessageBoxButton.OK, MessageBoxImage.Error);
                btnClearAll.PerformClick();
            }

            UpdatePreviousOperationLabel();
        }

        private void btnClearEntry_Click(object sender, RoutedEventArgs e)
            => txtInput.Text = "0";

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            FirstValue = 0;
            CurrentOperation = null;
            txtInput.Text = "0";

            UpdatePreviousOperationLabel();
        }

        // Pierwiastek kwadratowy
        private void btnSquareRoot_Click(object sender, RoutedEventArgs e)
        {
            double value = double.Parse(txtInput.Text);
            double result = Math.Sqrt(value);
            txtInput.Text = result.ToString();

        }

        // (1/x)
        private void btnReciprocal_Click(object sender, RoutedEventArgs e)
        {
            double value = double.Parse(txtInput.Text);
            double result = 1.0 / value;
            txtInput.Text = result.ToString();
        }

        // Silnia
        private void btnFactorial_Click(object sender, RoutedEventArgs e)
        {
            int value = int.Parse(txtInput.Text);
            int result = CalculateFactorial(value);
            txtInput.Text = result.ToString();
        }

        // Funkcja rekurencyjna do obliczania silni
        private int CalculateFactorial(int n)
        {
            if (n == 0 || n == 1)
                return 1;
            else
                return n * CalculateFactorial(n - 1);
        }

        // Logarytm
        private void btnLogarithm_Click(object sender, RoutedEventArgs e)
        {
            double value = double.Parse(txtInput.Text);
            double result = Math.Log10(value); // Logarytm dziesiętny
            txtInput.Text = result.ToString();
        }

        // podłoga
        private void btnFloor_Click(object sender, RoutedEventArgs e)
        {
            double value = double.Parse(txtInput.Text);
            double result = Math.Floor(value);
            txtInput.Text = result.ToString();
        }
        // sufit
        private void btnCeiling_Click(object sender, RoutedEventArgs e)
        {
            double value = double.Parse(txtInput.Text);
            double result = Math.Ceiling(value);
            txtInput.Text = result.ToString();
        }
    }
}
