using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
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
//using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Numerics;

namespace CalculatorBlack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon MyNotifyIcon;
        private double number1 = 0;
        private double number2 = 0;
        private double result = 0;
        private double memory;
        private int operation = (int)OperationType.NoOperation;
        private bool dotBool = false;
        private bool firstOperation = false;


        private CultureInfo cultureUK = new CultureInfo("en-UK");
        private CultureInfo cultureRO = new CultureInfo("ro-RO");
        private CultureInfo cultureUS = new CultureInfo("en-US");
        private CultureInfo culture;

        private bool cascade;

        private Stack operators = new Stack();
        private Stack numbers = new Stack();


        public enum OperationType
        {
            NoOperation = 0,
            Plus,
            Minus,
            Divide,
            Multiplication,
            Percent
        }

        public MainWindow()
        {
            InitializeComponent();
            MyNotifyIcon = new System.Windows.Forms.NotifyIcon();
            MyNotifyIcon.Icon = new System.Drawing.Icon(@"..\..\calculator_512_GZm_icon.ico");

            UKButton.IsChecked = CalculatorBlack.Properties.Settings.Default.UKDigit;
            ROButton.IsChecked = CalculatorBlack.Properties.Settings.Default.RODigit;
            NoneButton.IsChecked = CalculatorBlack.Properties.Settings.Default.USDigit;

            CascadeButton.IsChecked = CalculatorBlack.Properties.Settings.Default.cascade;
            NonCascadeButton.IsChecked = CalculatorBlack.Properties.Settings.Default.noncascade;

            cultureUS.NumberFormat.NumberGroupSeparator = "";

            if (UKButton.IsChecked == true)
                this.culture = cultureUK;
            else if (ROButton.IsChecked == true)
                this.culture = cultureRO;
            else if (NoneButton.IsChecked == true)
                this.culture = cultureUS;



            if (CascadeButton.IsChecked == true)
                this.cascade = true;
            else
                this.cascade = false;


            ButtonMemoryClear.IsEnabled = false;
            ButtonMemoryRecall.IsEnabled = false;
        }

        //Window Events
        void MyNotifyIcon_MouseClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = WindowState.Normal;
        }

        private void Minimize()
        {
            this.ShowInTaskbar = false;
            MyNotifyIcon.Visible = true;
            Hide();
            MyNotifyIcon.Click += MyNotifyIcon_MouseClick;

        }
        private void Maximize()
        {
            MyNotifyIcon.Visible = false;
            this.ShowInTaskbar = true;

        }

        protected override void OnStateChanged(EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Maximized:
                    {
                        Maximize();
                        break;
                    }
                case WindowState.Minimized:
                    {
                        Minimize();
                        break;
                    }
                case WindowState.Normal:
                    {
                        Maximize();
                        break;
                    }
            }

            base.OnStateChanged(e);
        }

        private void Close_Window(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            grid.Visibility = Visibility.Hidden;
        }

        //Menu buttons
        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }
        private String Format(String number)
        {
            int lenght = 0;
            if (number.Contains(",") || number.Contains("."))
            {
                if (culture == cultureUK)
                    lenght = number.Substring(number.IndexOf(".") + 1).Length;
                else if (culture == cultureRO)
                    lenght = number.Substring(number.IndexOf(",") + 1).Length;
                else
                    lenght = number.Substring(number.IndexOf(".") + 1).Length;
            }



            return "N" + lenght.ToString();
        }

        //Numbers buttons
        private void NumberButton(double number)
        {
            if (operation == 0)
            {

                if (TextDisplay.Text.Contains(".") && (culture.Equals(cultureUK) || culture.Equals(cultureUS)))
                {

                    TextDisplay.Text += number.ToString(); ;
                    number1 = Convert.ToDouble(TextDisplay.Text);

                }
                else if (TextDisplay.Text.Contains(",") && culture.Equals(cultureRO))
                {
                    culture.NumberFormat.NumberDecimalSeparator = ",";
                    System.Threading.Thread.CurrentThread.CurrentCulture = culture;

                    TextDisplay.Text += number.ToString(); ;
                    number1 = Convert.ToDouble(TextDisplay.Text);

                }

                else

                {
                    number1 = (number1 * 10) + number;
                    TextDisplay.Text = number1.ToString("N0", culture);

                }

            }
            else
            {

                if (dotBool == true)
                {

                    TextDisplay.Text += number.ToString();
                    number2 = Convert.ToDouble(TextDisplay.Text);

                }
                else

                {
                    number2 = (number2 * 10) + number;
                    TextDisplay.Text = number2.ToString("N0", culture);

                }


            }

        }

        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            NumberButton(0);

        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            NumberButton(1);

        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            NumberButton(2);

        }
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            NumberButton(3);

        }
        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            NumberButton(4);

        }
        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            NumberButton(5);

        }
        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            NumberButton(6);
        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            NumberButton(7);

        }

        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            NumberButton(8);
        }
        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            NumberButton(9);

        }

        //Operations events and buttons
        private void Operation_Switch()
        {
            bool divideByZero = false;


            if (cascade == true)
            {
                if (number2 == 0)
                    if (firstOperation == true)
                    {
                        divideByZero = true;
                    }
                    else
                        return;



                result = 0;

                switch (this.operation)
                {

                    case (int)OperationType.Plus:
                        {
                            result = number1 + number2;
                            break;
                        }
                    case (int)OperationType.Minus:
                        {
                            result = number1 - number2;
                            break;
                        }
                    case (int)OperationType.Multiplication:
                        {
                            result = number1 * number2;
                            break;
                        }
                    case (int)OperationType.Divide:
                        {
                            if (number2 != 0)
                            {
                                result = number1 / number2;
                            }

                            else if (divideByZero)
                            {
                                TextDisplay.Text = "Cannot divide by zero";
                                number1 = 0;
                                number2 = 0;
                                divideByZero = false;
                                return;

                            }

                            break;
                        }


                }


                number1 = result;
                number2 = 0;
                firstOperation = true;

                TextDisplay.Text = result.ToString(Format(result.ToString()), culture);



            }
            else
            {
                if (firstOperation == true)

                {
                    numbers.Push(number2);


                    if (numbers.Count >= 2)
                    {
                        if (Convert.ToInt64(operators.Peek()) == (int)OperationType.Multiplication)
                        {
                            double value1 = Convert.ToDouble(numbers.Pop());

                            double value2 = Convert.ToDouble(numbers.Pop());

                            numbers.Push(value1 * value2);
                            operators.Pop();

                        }
                        else
                        if (Convert.ToInt64(operators.Peek()) == (int)OperationType.Divide)
                        {
                            double value1 = Convert.ToDouble(numbers.Pop());

                            double value2 = Convert.ToDouble(numbers.Pop());

                            numbers.Push(value2 / value1);
                            operators.Pop();

                        }
                    }
                    //TextDisplay.Text = "";

                    number2 = 0;

                }
                else
                {
                    firstOperation = true;
                    // numbers.Push(number2);
                    return;
                }

            }

        }

        private void Operation_Select(int operation)
        {

            if (cascade == true)
            {
                if (this.operation == 0)
                {
                    this.operation = operation;

                    Operation_Switch();


                }
                else
                {
                    Operation_Switch();

                    this.operation = operation;
                }
            }
            else

            {

                if (this.operation == 0)
                {
                    this.operation = operation;
                    operators.Push(operation);
                    numbers.Push(number1);
                    Operation_Switch();


                }
                else
                {

                    Operation_Switch();
                    this.operation = operation;
                    operators.Push(operation);


                }

            }
        }


        private void ButtonPlus_Click(object sender, RoutedEventArgs e)
        {
            dotBool = false;
            Operation_Select((int)OperationType.Plus);


        }

        private void ButtonMinus_Click(object sender, RoutedEventArgs e)
        {
            Operation_Select((int)OperationType.Minus);
            dotBool = false;

        }

        private void ButtonMultiplication_Click(object sender, RoutedEventArgs e)
        {
            Operation_Select((int)OperationType.Multiplication);
            dotBool = false;

        }

        private void ButtonDivision_Click(object sender, RoutedEventArgs e)
        {
            Operation_Select((int)OperationType.Divide);
            dotBool = false;


        }

        private void Percent_Click(object sender, RoutedEventArgs e)
        {
            if (operation == 0)
            {
                number1 = ((number1 * number2) / 100);
                TextDisplay.Text = number1.ToString(Format(number1.ToString()), culture);

            }
            else
            {
                if (number2 != 0)
                {
                    number2 = ((number1 * number2) / 100);
                    TextDisplay.Text = number2.ToString(Format(number2.ToString()), culture);
                }
                else
                {
                    number2 = ((number1 * number2) / 100);
                    TextDisplay.Text = number2.ToString(Format(number2.ToString()), culture);
                }

            }

        }

        private void ButtonSqrt_Click(object sender, RoutedEventArgs e)
        {
            if (operation == 0)
            {
                if (number1 < 0)
                {
                    TextDisplay.Text = "Invalid input";
                    number1 = 0;
                }
                else
                {
                    number1 = Math.Sqrt(number1);
                    TextDisplay.Text = number1.ToString(Format(number1.ToString()), culture);
                }
            }
            else
            {
                if (number2 != 0)
                {
                    if (number2 < 0)
                    {
                        TextDisplay.Text = "Invalid input";
                        number2 = 0;
                    }
                    else
                    {
                        number2 = Math.Sqrt(number2);
                        TextDisplay.Text = number2.ToString(Format(number2.ToString()), culture);
                    }
                }
                else
                {
                    if (number1 < 0)
                    {
                        TextDisplay.Text = "Invalid input";
                        number1 = 0;
                    }
                    else
                    {
                        number1 = Math.Sqrt(number1);
                        TextDisplay.Text = number1.ToString(Format(number2.ToString()), culture);
                    }
                }
            }
        }

        private void ButtonPow2_Click(object sender, RoutedEventArgs e)
        {
            if (operation == 0)
            {
                number1 = (number1 * number1);

                TextDisplay.Text = number1.ToString(Format(number1.ToString()), culture);

            }
            else
            {
                if (number2 != 0)
                {
                    number2 = (number2 * number2);
                    TextDisplay.Text = number2.ToString(Format(number2.ToString()), culture);
                }
                else
                {
                    number1 = (number1 * number1);
                    TextDisplay.Text = number1.ToString(Format(number1.ToString()), culture);
                }
            }
        }

        private void ButtonInversion_Click(object sender, RoutedEventArgs e)
        {
            if (operation == 0)
            {
                number1 = 1 / (number1);
                TextDisplay.Text = number1.ToString(Format(number1.ToString()), culture);
            }
            else
            {
                if (number2 != 0)
                {
                    number2 = 1 / (number2);
                    TextDisplay.Text = number2.ToString(Format(number1.ToString()), culture);

                }
                else
                    TextDisplay.Text = "Cannot divide by zero";
            }
        }


        private void ButtonEqual_Click(object sender, RoutedEventArgs e)
        {
            if (cascade == true)
            {
                switch (operation)
                {
                    case (int)OperationType.Plus:

                        Operation_Select((int)OperationType.Plus);

                        break;
                    case (int)OperationType.Minus:
                        Operation_Select((int)OperationType.Minus);

                        break;
                    case (int)OperationType.Divide:
                        Operation_Select((int)OperationType.Divide);

                        break;
                    case (int)OperationType.Multiplication:
                        Operation_Select((int)OperationType.Multiplication);

                        break;
                    default:
                        TextDisplay.Text = string.Format("{0:G0}", Convert.ToDouble(TextDisplay.Text), culture);
                        number1 = 0;
                        break;

                }

            }
            else

            {
                //if (numbers.Count == 1)
                numbers.Push(number2);


                while (operators.Count > 0)
                {


                    if (Convert.ToInt64(operators.Peek()) == (int)OperationType.Plus)
                    {
                        operators.Pop();
                        double value1 = Convert.ToDouble(numbers.Pop());

                        double value2 = Convert.ToDouble(numbers.Pop());
                        if (Convert.ToInt64(operators.Count) > 0 && (Convert.ToInt64(operators.Peek()) == (int)OperationType.Minus))
                        {
                            value2 = -value2;
                            operators.Pop();
                            operators.Push((int)OperationType.Plus);
                        }


                        result = (value1 + value2);
                        numbers.Push(result);


                    }
                    else
                    if (Convert.ToInt64(operators.Peek()) == (int)OperationType.Minus)
                    {
                        operators.Pop();
                        double value1 = Convert.ToDouble(numbers.Pop());
                        double value2 = Convert.ToDouble(numbers.Pop());

                        result = (value2 - value1);
                        numbers.Push(result);


                    }
                    else
                    if (Convert.ToInt64(operators.Peek()) == (int)OperationType.Multiplication)
                    {
                        double value1 = Convert.ToDouble(numbers.Pop());

                        double value2 = Convert.ToDouble(numbers.Pop());

                        numbers.Push(value1 * value2);
                        operators.Pop();

                    }
                    else
                      if (Convert.ToInt64(operators.Peek()) == (int)OperationType.Divide)
                    {
                        double value1 = Convert.ToDouble(numbers.Pop());

                        double value2 = Convert.ToDouble(numbers.Pop());

                        numbers.Push(value2 / value1);
                        operators.Pop();

                    }


                }
                TextDisplay.Text = Convert.ToString(numbers.Pop());
            }

        }


        private void ButtonPlusMinus_Click(object sender, RoutedEventArgs e)
        {
            if (operation == 0)
            {
                number1 *= -1;
                TextDisplay.Text = number1.ToString(Format(number1.ToString()), culture);
            }
            else
            {
                number2 *= -1;
                TextDisplay.Text = number2.ToString(Format(number1.ToString()), culture);
            }
        }

        //Delete buttons
        private void ButtonBackSpace_Click(object sender, RoutedEventArgs e)
        {
            string pattern = @"^[a-zA-Z\s]+$";
            Regex rgx = new Regex(pattern);
            if (rgx.IsMatch(TextDisplay.Text))
            {
                TextDisplay.Text = "0";
                number1 = 0;
                number2 = 0;
                return;
            }
            else
            {
                if (operation == 0)
                {
                    TextDisplay.Text = TextDisplay.Text.Remove(TextDisplay.Text.Length - 1);
                    if (TextDisplay.Text.Length == 0)
                    {
                        TextDisplay.Text = "0";
                        number1 = 0;
                    }

                    else
                        if (TextDisplay.Text[TextDisplay.Text.Length - 1] == ',' || TextDisplay.Text[TextDisplay.Text.Length - 1] == '.')
                    {
                       
                        TextDisplay.Text = TextDisplay.Text.Remove(TextDisplay.Text.Length - 1);
                    }
                    else if (TextDisplay.Text[TextDisplay.Text.Length - 1] == '-')
                    {
                        TextDisplay.Text = "0";
                        number1 = 0;
                    }
                    if (culture.Equals(cultureRO))
                    {
                        culture.NumberFormat.NumberDecimalSeparator = ",";
                        System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    }
                    number1 = Convert.ToDouble(TextDisplay.Text);
                }
                else
                {
                    TextDisplay.Text = TextDisplay.Text.Remove(TextDisplay.Text.Length - 1);
                    if (TextDisplay.Text.Length == 0)
                    {
                        TextDisplay.Text = "0";
                        number2 = 0;
                    }

                    else
                        if (TextDisplay.Text[TextDisplay.Text.Length - 1] == ',' || TextDisplay.Text[TextDisplay.Text.Length - 1] == '.')
                    {
                        TextDisplay.Text = TextDisplay.Text.Remove(TextDisplay.Text.Length - 1);
                    }
                    else if (TextDisplay.Text[TextDisplay.Text.Length - 1] == '-')
                    {
                        TextDisplay.Text = "0";
                        number2 = 0;
                    }

                    if (culture.Equals(cultureRO))
                    {
                        culture.NumberFormat.NumberDecimalSeparator = ",";
                        System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    }
                    number2 = Convert.ToDouble(TextDisplay.Text);
                }
            }
        }
        private void ButtonClearEntry_Click(object sender, RoutedEventArgs e)
        {
            if (operation == 0)
            {
                number1 = 0;
            }
            else
            {
                number2 = 0;
            }
            TextDisplay.Text = "0";
        }

        private void ButtonAllClear_Click(object sender, RoutedEventArgs e)
        {
            number1 = 0;
            number2 = 0;
            operation = (int)OperationType.NoOperation;
            TextDisplay.Text = "0";
            firstOperation = false;
        }

        //Comma button
        private void ButtonComma_Click(object sender, RoutedEventArgs e)
        {

            if (this.culture.Equals(cultureUK) || this.culture.Equals(cultureUS))
            {


                TextDisplay.Text += ".";
                dotBool = true;
            }
            else
            {

                TextDisplay.Text += ",";
                dotBool = true;
            }

        }
        //Memory buttons
        private void ButtonMemory_Click(object sender, RoutedEventArgs e)
        {
            memory = Convert.ToDouble(TextDisplay.Text);
            ButtonMemoryClear.IsEnabled = true;
            ButtonMemoryRecall.IsEnabled = true;
            number1 = 0;
        }

        private void ButtonMemoryRecall_Click(object sender, RoutedEventArgs e)
        {
            TextDisplay.Text = memory.ToString();

            if (operation == 0)
            {
                number1 = memory;

            }
            else
            {


                number2 = memory;

            }
        }

        private void ButtonMemoryClear_Click(object sender, RoutedEventArgs e)
        {
            memory = 0;
            ButtonMemoryClear.IsEnabled = false;
            ButtonMemoryRecall.IsEnabled = false;
        }

        private void ButtonMemoryAddition_Click(object sender, RoutedEventArgs e)
        {
            memory += Convert.ToDouble(TextDisplay.Text);
        }

        private void ButtonMemorySubtraction_Click(object sender, RoutedEventArgs e)
        {
            memory -= Convert.ToDouble(TextDisplay.Text);
        }



        //About dialog box with links in textblock
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void Visible_Hidden(bool visibility)
        {
            if (visibility)
            {
                AboutDialogBox.Visibility = Visibility.Visible;
                ConfigDialogBox.Visibility = Visibility.Hidden;
                UKButton.Visibility = Visibility.Hidden;
                ROButton.Visibility = Visibility.Hidden;
                CascadeButton.Visibility = Visibility.Hidden;
                NonCascadeButton.Visibility = Visibility.Hidden;
                MethonBox.Visibility = Visibility.Hidden;
                UKBox.Visibility = Visibility.Hidden;
                ROBox.Visibility = Visibility.Hidden;
                CascadeBox.Visibility = Visibility.Hidden;
                OperatoxBox.Visibility = Visibility.Hidden;
                NoneButton.Visibility = Visibility.Hidden;
                NoneBox.Visibility = Visibility.Hidden;
            }
            else
            {
                AboutDialogBox.Visibility = Visibility.Hidden;
                ConfigDialogBox.Visibility = Visibility.Visible;
                UKButton.Visibility = Visibility.Visible;
                ROButton.Visibility = Visibility.Visible;
                CascadeButton.Visibility = Visibility.Visible;
                NonCascadeButton.Visibility = Visibility.Visible;
                MethonBox.Visibility = Visibility.Visible;
                UKBox.Visibility = Visibility.Visible;
                ROBox.Visibility = Visibility.Visible;
                CascadeBox.Visibility = Visibility.Visible;
                OperatoxBox.Visibility = Visibility.Visible;
                NoneButton.Visibility = Visibility.Visible;
                NoneBox.Visibility = Visibility.Visible;
            }
        }

        private void Show_System_Info(object sender, RoutedEventArgs e)
        {
            Visible_Hidden(true);

            String author = "Author: Kocsis Bogdan";
            int cpu = Environment.ProcessorCount;
            String s_cpu = "CPUs: " + cpu;
            String os = "OS: " + Environment.OSVersion.Version;

            AboutDialogBox.Inlines.Clear();
            AboutDialogBox.Inlines.Add(author + "\n" + s_cpu + "\n" + os + "\n" + "GitHub: ");
            Hyperlink hyperlink = new Hyperlink()
            {
                NavigateUri = new Uri("https://github.com/BogdanKocsis")
            };

            hyperlink.Inlines.Add("https://github.com/BogdanKocsis");
            hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
            AboutDialogBox.Inlines.Add(hyperlink);


            AboutDialogBox.Inlines.Add("\n" + "Intranet: ");
            Hyperlink hyperlink1 = new Hyperlink()
            {
                NavigateUri = new Uri("https://intranet.unitbv.ro/Kocsis_Bogdan")
            };

            hyperlink1.Inlines.Add("https://intranet.unitbv.ro/Kocsis_Bogdan");
            hyperlink1.RequestNavigate += Hyperlink_RequestNavigate;
            AboutDialogBox.Inlines.Add(hyperlink1);

        }

        //Keyboard
        private void On_Button_Key_Down(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad1:
                    {
                        Button1_Click(sender, e);
                        break;
                    }

                case Key.NumPad2:
                    {
                        Button2_Click(sender, e);
                        break;
                    }
                case Key.NumPad3:
                    {
                        Button3_Click(sender, e);
                        break;
                    }
                case Key.NumPad4:
                    {
                        Button4_Click(sender, e);
                        break;
                    }
                case Key.NumPad5:
                    {
                        Button5_Click(sender, e);
                        break;
                    }
                case Key.NumPad6:
                    {
                        Button6_Click(sender, e);
                        break;
                    }
                case Key.NumPad7:
                    {
                        Button7_Click(sender, e);
                        break;
                    }
                case Key.NumPad8:
                    {
                        Button8_Click(sender, e);
                        break;
                    }
                case Key.NumPad9:
                    {
                        Button9_Click(sender, e);
                        break;
                    }
                case Key.NumPad0:
                    {
                        Button0_Click(sender, e);
                        break;
                    }
                case Key.Add:
                    {
                        ButtonPlus_Click(sender, e);
                        break;
                    }
                case Key.Subtract:
                    {
                        ButtonMinus_Click(sender, e);
                        break;
                    }
                case Key.Multiply:
                    {
                        ButtonMultiplication_Click(sender, e);
                        break;
                    }
                case Key.Divide:
                    {
                        ButtonDivision_Click(sender, e);
                        break;
                    }
                case Key.Enter:
                    {

                        ButtonEqual_Click(sender, e);
                        break;
                    }
                case Key.Escape:
                    {
                        ButtonAllClear_Click(sender, e);
                        break;
                    }

                case Key.Decimal:
                    {
                        ButtonComma_Click(sender, e);
                        break;
                    }
                case Key.Back:
                    {
                        ButtonBackSpace_Click(sender, e);
                        break;
                    }
                case Key.R:
                    {
                        ButtonInversion_Click(sender, e);
                        break;
                    }
                case Key.Q:
                    {
                        ButtonPow2_Click(sender, e);
                        break;
                    }
                case Key.RightShift:
                    {
                        ButtonSqrt_Click(sender, e);
                        break;
                    }
                case Key.P:
                    {
                        Percent_Click(sender, e);
                        break;
                    }
                case Key.F9:
                    {
                        Percent_Click(sender, e);
                        break;
                    }

            }
        }

        //Copy
        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText(TextDisplay.Text.ToString());
        }

        //Cut
        private void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText(TextDisplay.Text.ToString());
            TextDisplay.Text = "0";
        }

        //Paste
        private void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private bool Check_Paste_Text(String paste)
        {
            String pattern = @"^[-+]?[0-9]*\.?[0-9]+$";
            Regex rgx = new Regex(pattern);
            if (rgx.IsMatch(paste))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            String paste = Clipboard.GetText();
            bool valid = Check_Paste_Text(paste);
            if (valid)
            {
                TextDisplay.Text = paste;
                if (operation == 1)
                    number1 = Convert.ToDouble(paste);
                else
                    number2 = Convert.ToDouble(paste);
            }
            else
            {
                TextDisplay.Text = "Invalid input";
            }
        }

        //Config dialog box
        private void ButtonConfig_Click(object sender, RoutedEventArgs e)
        {

            Visible_Hidden(false);
        }

        private void ToggleButtonUK_Checked(object sender, RoutedEventArgs e)
        {

            if (UKButton.IsChecked == true)
            {
                //ROButton.IsChecked = false;
                //NoneButton.IsChecked = false;
                this.culture = cultureUK;
                CalculatorBlack.Properties.Settings.Default.RODigit = false;
                CalculatorBlack.Properties.Settings.Default.USDigit = false;
                CalculatorBlack.Properties.Settings.Default.Save();

            }


        }



        private void ToggleButtonRO_Checked(object sender, RoutedEventArgs e)
        {
            if (ROButton.IsChecked == true)
            {
                UKButton.IsChecked = false;

                this.culture = cultureRO;
                CalculatorBlack.Properties.Settings.Default.UKDigit = false;
                CalculatorBlack.Properties.Settings.Default.USDigit = false;
                CalculatorBlack.Properties.Settings.Default.Save();
            }


        }
        private void ToggleButtonRO_UnChecked(object sender, RoutedEventArgs e)
        {


            UKButton.IsChecked = true;

            this.culture = cultureUK;
            CalculatorBlack.Properties.Settings.Default.RODigit = false;
            CalculatorBlack.Properties.Settings.Default.USDigit = false;
            CalculatorBlack.Properties.Settings.Default.Save();



        }

        private void ToggleButtonNoneCulture(object sender, RoutedEventArgs e)
        {
            if (NoneButton.IsChecked == true)
            {
                ROButton.IsChecked = false;
                UKButton.IsChecked = false;

                this.culture = cultureUS;

                CalculatorBlack.Properties.Settings.Default.UKDigit = false;
                CalculatorBlack.Properties.Settings.Default.RODigit = false;

                CalculatorBlack.Properties.Settings.Default.Save();
            }


        }
        private void ToggleButtonCascade_Checked(object sender, RoutedEventArgs e)
        {

            if (CascadeButton.IsChecked == true)
            {
                this.cascade = true;
                NonCascadeButton.IsChecked = false;
                CalculatorBlack.Properties.Settings.Default.cascade = true;
                CalculatorBlack.Properties.Settings.Default.noncascade = false;
                CalculatorBlack.Properties.Settings.Default.Save();

            }
            else
            {

                NonCascadeButton.IsChecked = true;
                this.cascade = false;
                CalculatorBlack.Properties.Settings.Default.cascade = false;
                CalculatorBlack.Properties.Settings.Default.noncascade = true;
                CalculatorBlack.Properties.Settings.Default.Save();

            }

        }

        private void ToggleButtonNonCascade_Checked(object sender, RoutedEventArgs e)
        {
            if (NonCascadeButton.IsChecked == true)
            {
                CascadeButton.IsChecked = false;

            }
            else
            {

                CascadeButton.IsChecked = true;

            }
        }



    }
}
