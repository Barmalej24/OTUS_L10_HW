using System.Collections;
using System.Text;

namespace OTUS_L10_HW
{
    internal class Program
    {
        private static readonly string[] _indexChar = { "a", "b", "c" };
        private static int _selectedValue;
        static void Main(string[] args)
        {
            var i = 0;
            var errorIndex = string.Empty;
            var indexInt = new int[3];

            var indexStr = FirstMenu();

            var data = new Dictionary<string, string>()
            {
                { _indexChar[0], indexStr[0]},
                { _indexChar[1], indexStr[1]},
                { _indexChar[2], indexStr[2]},
            };

            try
            {
                foreach (var arg in indexStr)
                {
                    errorIndex = _indexChar[i];
                    indexInt[i] = int.Parse(arg);
                    i++;
                }

                try
                {
                    Solution(indexInt);
                }
                catch (Exception e)
                {
                    FormatData(e.Message, Severity.Error, e.Data);
                }
            }
            catch (OverflowException e)
            {
                e.Data.Add("int", "хранит целое число от -2147483648 до 2147483647 и занимает 4 байта");
                FormatData($"Некорректный диапазон значений для вводимого коэффициента {errorIndex}", Severity.InformationError, e.Data);
            }
            catch (FormatException e)
            {
                FormatData($"Неверный формат параметра {errorIndex}", Severity.Error, data);
            }
            catch
            {
                FormatData($"Иная ошибка при обработки параметра {errorIndex}", Severity.Information, data);
            }
        }

        private static string[] FirstMenu()
        {
            ConsoleKeyInfo ki;

            do
            {
                Console.WriteLine("Выбери вариан меню:");
                Console.WriteLine("1. Построчно выводимое");
                Console.WriteLine("2. Интерактивное");
                ki = Console.ReadKey();
                switch (ki.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Console.Clear();
                        return ArgumentsMenuLine();

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        Console.Clear();
                        return ArgumentsMenuInteractive();
                }
                Console.Clear();
            }
            while (ki.Key != ConsoleKey.D1 || ki.Key != ConsoleKey.D2);
            return null;
        }

        private static void Solution(int[] index)
        {
            var d = Discriminant(index[0], index[1], index[2]);
            var data = new Dictionary<string, int>()
                {
                    { "a", index[0]},
                    { "b", index[1]},
                    { "c", index[2]},
                    { "D", d},
                };
            try
            {
                if (d < 0)
                    throw new SolutionException("Вещественных значений не найдено", data);
                
                if (d == 0)
                {
                    var x = (double)-index[1] / (2 * index[0]);
                    Console.WriteLine($"x = {x}");
                }
                else
                {
                    var x1 = (-index[1] + Math.Sqrt(d)) / (2 * index[0]);
                    var x2 = (-index[1] - Math.Sqrt(d)) / (2 * index[0]);
                    Console.WriteLine($"x1 = {x1}, x2 = {x2}");
                }
            }
            catch (SolutionException e)
            {
                FormatData(e.Message, Severity.Warning, e.Data);
                throw new Exception("Вещественных значений не найдено");
            }
        }

        private static int Discriminant(int a, int b, int c)
        {
            return b * b - 4 * a * c;
        }

        private static string[] ArgumentsMenuLine()
        {
            var result = new string[3];
            var sb = new StringBuilder();
            sb.AppendLine("Решение квадратного уравнение");
            sb.AppendLine("a * x^2 + b * x + c = 0");
            Console.WriteLine(sb.ToString());
            for (int i = 0; i < 3; i++)
            {
                Console.Write($"Введите значение аргумента {_indexChar[i]}: ");
                result[i] = Console.ReadLine();
            }
            sb.Clear();
            return result;
        }

        private static string[] ArgumentsMenuInteractive()
        {
            var result = new string[3];
            ConsoleKeyInfo ki;

            _selectedValue = 1;
            PrintMenu();
            PrintEquation(result);
            WriteCursor(_selectedValue);

            do
            {
                ki = Console.ReadKey();
                ClearCursor(_selectedValue);
                switch(ki.Key)
                {   
                    case ConsoleKey.UpArrow:
                        SetUp(); 
                        break;
                    case ConsoleKey.DownArrow: 
                        SetDown(); 
                        break;
                    case ConsoleKey.D0:
                    case ConsoleKey.D1:
                    case ConsoleKey.D2:
                    case ConsoleKey.D3:
                    case ConsoleKey.D4:
                    case ConsoleKey.D5:
                    case ConsoleKey.D6:
                    case ConsoleKey.D7:
                    case ConsoleKey.D8:
                    case ConsoleKey.D9:
                    case ConsoleKey.NumPad0:
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.NumPad3:
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.NumPad5:
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.NumPad7:
                    case ConsoleKey.NumPad8:
                    case ConsoleKey.NumPad9:
                    case ConsoleKey.OemMinus:
                    case ConsoleKey.Subtract:
                        result[_selectedValue - 1] = SetArgument(ki.KeyChar.ToString());
                        PrintEquation(result);
                        SetDown();
                        break;
                    case ConsoleKey.Enter:
                        SetPrintPoint();
                        return result;
                }
                WriteCursor(_selectedValue);
            }
            while (ki.Key != ConsoleKey.Escape);
            return result;
        }

        private static void PrintEquation(string[] index)
        {   
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            var a = (!string.IsNullOrEmpty(index[0])) ? index[0] : "a";
            var b = (!string.IsNullOrEmpty(index[1])) ? index[1] : "b";
            var c = (!string.IsNullOrEmpty(index[2])) ? index[2] : "c";
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"({a}) * x^2 + ({b}) * x + ({c}) = 0");
            
        }
        
        private static string SetArgument(string first)
        {
            Console.SetCursorPosition(5, _selectedValue);
            Console.Write(new string(' ', Console.WindowWidth - 5));
            Console.SetCursorPosition(5, _selectedValue);
            var sb = new StringBuilder();
            sb.Append(first);
            Console.SetCursorPosition(5, _selectedValue);
            var line = Console.ReadLine();
            sb.Append(line);
            return sb.ToString();
        }
       
        private static void SetPrintPoint()
        {
            Console.SetCursorPosition(0, 4);
        }
        
        private static void SetDown()
        {
            if (_selectedValue < _indexChar.Length)
            {
                _selectedValue++;
            }
            else
            {
                _selectedValue = 1;
            }
        }
        
        private static void SetUp()
        {
            if (_selectedValue > 1)
            {
                _selectedValue--;
            }
            else
            {
                _selectedValue = 3;
            }
        }
        
        private static void PrintMenu()
        {
            Console.WriteLine("");
            for (int i = 0; i < _indexChar.Length; i++)
            {
                Console.WriteLine($" {_indexChar[i]}: ");
            }
        }
        
        private static void WriteCursor(int pos)
        {
            Console.SetCursorPosition(0, pos);
            Console.Write(">");
            Console.SetCursorPosition(4, pos);
        }
        
        private static void ClearCursor(int pos)
        {
            Console.SetCursorPosition(0,pos);
            Console.Write(" ");
            Console.SetCursorPosition(4, pos);
        }

        class SolutionException : ArgumentException
        {
            public new IDictionary Data { get; }
            public SolutionException(string message, IDictionary? data)
                : base(message)
            {
                Data = data;
            }
        }

        static void FormatData(string message, Severity severity, IDictionary data)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{message}");
            sb.AppendLine(DemarcationLine(50));
            foreach (DictionaryEntry item in data)
            {
                sb.AppendLine($"{item.Key} = {item.Value}");
            }

            switch (severity)
            {
                case Severity.Warning:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(sb);
                    Console.ResetColor();
                    break;
                case Severity.Error:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(sb);
                    Console.ResetColor();
                    break;
                case Severity.Information:
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(sb);
                    Console.ResetColor();
                    break;
                default:
                    Console.WriteLine(sb);
                    break;
            }
        }

        static string DemarcationLine(int count)
        {
            var sb = new StringBuilder();
            var i = 1;
            while (i <= count)
            {
                sb.Append("_");
                i++;
            }
            return sb.ToString();
        }

        enum Severity
        {
            Warning,
            Error,
            Information,
            InformationError,
        }
    }
}